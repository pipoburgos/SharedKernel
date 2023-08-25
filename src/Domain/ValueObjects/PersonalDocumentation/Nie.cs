using System.Text.RegularExpressions;

namespace SharedKernel.Domain.ValueObjects.PersonalDocumentation
{
    /// <summary> </summary>
    public class Nie
    {
        private const string Correspondencia = "TRWAGMYFPDXBNJZSQVHLCKET";

        /// <summary> </summary>
        protected Nie() { }

        /// <summary> </summary>
        protected Nie(string value)
        {
            Value = value;
        }

        /// <summary> </summary>
        public static Nie Create(string value)
        {
            return new Nie(value);
        }

        /// <summary> NIE value. </summary>
        public string Value { get; private set; } = null!;

        /// <summary>
        /// Check if the value meets the validation rules or not
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Value))
                return true;

            // The excess characters are deleted.
            var nie = DeleteInvalidChars(Value);

            // Check NIE length.
            if (nie.Length != 9 && nie.Length != 11)
                return false;

            // Check NIE format.
            if (!Regex.IsMatch(nie, @"[K-MX-Z]\d{7}[" + Correspondencia + "]$"))
                return false;

            var initialLetter = nie.FirstOrDefault().ToString();
            int.TryParse(nie.Substring(1, 7), out var nieNumber);
            var controlDigit = nie.LastOrDefault().ToString();

            switch (initialLetter)
            {
                case "X":
                    break;
                case "Y":
                    nieNumber += 10000000;
                    break;
                case "Z":
                    nieNumber += 20000000;
                    break;
            }

            // Check letter.
            return controlDigit == GetNieLetter(nieNumber);
        }

        /// <summary>
        /// Removing all non-numeric characters or text string
        /// </summary>
        /// <param name="number">Number such that the user writes</param>
        /// <returns>String with no signs</returns>
        private static string DeleteInvalidChars(string number)
        {
            // All characters that are not numbers or letters.
            const string chars = @"[^\w]";
            var regex = new Regex(chars);
            return regex.Replace(number, string.Empty).ToUpper();
        }

        /// <summary>
        /// Returns the letter of a NIE.
        /// </summary>
        /// <param name="number">NIE Number</param>
        /// <returns>Control letter</returns>
        private static string GetNieLetter(int number)
        {
            var indice = number % 23;
            return Correspondencia[indice].ToString();
        }
    }
}