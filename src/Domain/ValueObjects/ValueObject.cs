using System;
using System.Linq;

namespace SharedKernel.Domain.ValueObjects
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for value objects in domain.
    /// Value
    /// </summary>
    /// <typeparam name="TValueObject">The type of this value object</typeparam>
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {

        #region IEquatable and Override Equals operators

        /// <inheritdoc />
        /// <summary>
        /// <see cref="M:System.Object.IEquatable{TValueObject}" />
        /// </summary>
        /// <param name="other"><see cref="M:System.Object.IEquatable{TValueObject}" /></param>
        /// <returns><see cref="M:System.Object.IEquatable{TValueObject}" /></returns>
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            //compare all public properties
            var publicProperties = GetType().GetProperties();

            if (publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);


                    return left is TValueObject ? ReferenceEquals(left, right) : left.Equals(right);
                });
            }
            return true;
        }

        /// <summary>
        /// <see cref="M:System.Object.Equals"/>
        /// </summary>
        /// <param name="obj"><see cref="M:System.Object.Equals"/></param>
        /// <returns><see cref="M:System.Object.Equals"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ValueObject<TValueObject> item && Equals((TValueObject)item);
        }

        /// <summary>
        /// <see cref="M:System.Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="M:System.Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            var hashCode = 31;
            var changeMultiplier = false;
            const int index = 1;

            //compare all public properties
            var publicProperties = GetType().GetProperties();


            if (!publicProperties.Any()) return hashCode;

            foreach (var item in publicProperties)
            {
                var value = item.GetValue(this, null);

                if (value != null)
                {

                    hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();

                    changeMultiplier = !changeMultiplier;
                }
                else
                {
                    hashCode ^= index * 13;//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Return true if the value objects are the same
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        /// <summary>
        /// Return true if the value objects are distinct
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
