using System.Text.Json;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary>  </summary>
public class PascalCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>  </summary>
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        // Convierte el nombre de la propiedad a PascalCase
        var chars = name.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);

        return new string(chars);
    }
}
