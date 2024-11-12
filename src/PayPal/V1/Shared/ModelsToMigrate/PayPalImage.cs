
namespace PayPal.V1.Shared.ModelsToMigrate;

/// <summary>
/// A QR code image for an invoice.
/// <para>
/// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
/// </para>
/// </summary>
public class PayPalImage //: PayPalSerializableObject
{
    /// <summary>A base-64 encoded string representing a PNG image.</summary>
    public string Image { get; set; }

    /// <summary>Saves the image data to a file on disk.</summary>
    /// <param name="filename">The path to the file where the image will be saved.</param>
    public void Save(string filename)
    {
        Save(filename, this);
    }

    /// <summary>Saves the image data to a file on disk.</summary>
    /// <param name="filename">The path to the file where the image will be saved.</param>
    /// <param name="image">Image object containing the image data sent from PayPal.</param>
    public static void Save(string filename, PayPalImage image)
    {
        if (string.IsNullOrEmpty(image.Image))
            return;

        File.WriteAllBytes(filename, Convert.FromBase64String(image.Image));
    }
}