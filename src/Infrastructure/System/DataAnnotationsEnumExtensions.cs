using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Infrastructure.System
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Task<IEnumerable<ComboDto<int>>> FromEnumToComboListAsync<TEnum>() where TEnum : IConvertible
        {
            var list = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(g => new ComboDto<int>
                {
                    Value = (int)Convert.ChangeType(g, typeof(int)),
                    Text = g.DisplayAttrName()
                })
                .Where(g => !string.IsNullOrWhiteSpace(g.Text));

            return Task.FromResult(list);
        }

        private static string DisplayAttrName(this IConvertible value)
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

            return attr.ResourceType != null ? attr.GetName() : attr.Name;
        }
    }
}
