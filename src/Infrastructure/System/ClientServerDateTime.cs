using Microsoft.AspNetCore.Http;
using SharedKernel.Application.System;
using System;

namespace SharedKernel.Infrastructure.System
{
    /// <summary> Date time manager. </summary>
    public class ClientServerDateTime : IDateTime
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>  </summary>
        /// <param name="httpContextAccessor"></param>
        public ClientServerDateTime(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary> Get utc date time. </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary> Max date time value. </summary>
        public DateTime MaxValue => DateTime.MaxValue;

        /// <summary> Client date time value. </summary>
        public DateTime ClientNow => ConvertToClientDate(DateTime.UtcNow);

        /// <summary> Client date time value. </summary>
        public DateTime ConvertToClientDate(DateTime dateTime)
        {
            var timezoneOffset = _httpContextAccessor?.HttpContext?.Request.Headers["TimezoneOffset"].ToString();

            if (dateTime.Kind == DateTimeKind.Local)
                dateTime = dateTime.ToUniversalTime();

            if (!string.IsNullOrWhiteSpace(timezoneOffset) &&
                int.TryParse(timezoneOffset, out var minutes))
                return dateTime.AddMinutes(minutes * -1);

            return dateTime;
        }
    }
}
