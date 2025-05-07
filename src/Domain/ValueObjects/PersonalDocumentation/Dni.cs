using System.Text.RegularExpressions;

namespace SharedKernel.Domain.ValueObjects.PersonalDocumentation;

/// <summary> </summary>
public class Dni : ValueObject<Dni>
{
    private const string Correspondencia = "TRWAGMYFPDXBNJZSQVHLCKET";

    /// <summary> </summary>
    protected Dni() { }

    /// <summary> </summary>
    protected Dni(string value)
    {
        Value = value;
    }

    /// <summary> </summary>
    public static Dni Create(string value)
    {
        return new Dni(value);
    }

    /// <summary> CIF value. </summary>
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
        var nif = DeleteInvalidChars(Value);

        // Check DNI length.
        if (nif.Length != 9)
            return false;

        // Check NIF format.
        if (!Regex.IsMatch(nif, @"[0-9]{8,10}[" + Correspondencia + "]$"))
            return false;

        int.TryParse(nif.Substring(0, 8), out var dniNumber);
        var controlDigit = nif.LastOrDefault().ToString();

        // Check letter.
        return controlDigit == GetDniLetter(dniNumber);
    }

    /// <summary>
    /// Removing all non-numeric characters or text string
    /// </summary>
    /// <param name="numero">Number such that the user writes</param>
    /// <returns>String with no signs</returns>
    private static string DeleteInvalidChars(string numero)
    {
        // All characters that are not numbers or letters.
#if NET40
        var regex = new Regex(@"[^\w]", RegexOptions.None);
#else
        var regex = new Regex(@"[^\w]", RegexOptions.None, TimeSpan.FromMinutes(1));
#endif
        return regex.Replace(numero, string.Empty).ToUpper();
    }

    /// <summary>
    /// Returns the letter of a DNI.
    /// </summary>
    /// <param name="number">DNI Number</param>
    /// <returns>Control letter</returns>
    private static string GetDniLetter(int number)
    {
        var index = number % 23;
        return Correspondencia[index].ToString();
    }
}