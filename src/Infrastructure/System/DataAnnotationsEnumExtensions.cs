#if !NETSTANDARD2_1
using SharedKernel.Application.Cqrs.Queries.Entities;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>  </summary>
    public static class EnumExtensions
    {
        /// <summary>  </summary>
        public static IEnumerable<ComboDto<TEnum>> FromEnumToComboList<TEnum>() where TEnum : IConvertible
        {
            var list = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(g => new ComboDto<TEnum>
                {
                    Value = g,
                    Text = g.DisplayAttrName(),
                })
                .Where(g => !string.IsNullOrWhiteSpace(g.Text));

            return list;
        }

        /// <summary>  </summary>
        public static IEnumerable<ComboDto<int>> FromEnumToIntComboList<TEnum>() where TEnum : IConvertible
        {
            var list = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(g => new ComboDto<int>
                {
                    Value = (int)Convert.ChangeType(g, typeof(int)),
                    Text = g.DisplayAttrName(),
                })
                .Where(g => !string.IsNullOrWhiteSpace(g.Text));

            return list;
        }

        /// <summary>  </summary>
        public static string DisplayAttrName(this IConvertible value)
        {
            var enumType = value.GetType();
            if (!enumType.IsEnum)
                throw new ArgumentNullException(nameof(value));

            var enumValue = Enum.GetName(enumType, value);
            var member = enumType.GetMember(enumValue ?? throw new InvalidOperationException()).First();

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (!attrs.Any())
                return string.Empty;

            var attr = (DisplayAttribute)attrs.First();

            return (attr.ResourceType != null ? attr.GetName() : attr.Name)!;
        }
    }
}
#endif