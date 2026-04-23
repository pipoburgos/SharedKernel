using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Xml.XPath;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters;

public class XEnumNamesSchemaFilter : ISchemaFilter
{
    private readonly List<XPathNavigator> _xmlNavigators = new();

    public XEnumNamesSchemaFilter(IEnumerable<string> xmlPaths)
    {
        foreach (var path in xmlPaths)
        {
            if (File.Exists(path))
                _xmlNavigators.Add(new XPathDocument(path).CreateNavigator());
        }
    }

    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        // Solo actuamos si es un Enum
        if (context.Type.IsEnum)
        {
            // FORZAMOS EL CAST a la clase concreta para poder inicializar Extensions
            var concreteSchema = schema as OpenApiSchema;
            if (concreteSchema == null) return;

            // Si es null, le asignamos un nuevo diccionario
            if (concreteSchema.Extensions == null)
            {
                concreteSchema.Extensions = new Dictionary<string, IOpenApiExtension>();
            }

            if (concreteSchema.Extensions == null)
                concreteSchema.Extensions = new Dictionary<string, IOpenApiExtension>();

            var names = Enum.GetNames(context.Type).ToList();
            var values = Enum.GetValues(context.Type).Cast<int>().ToList();
            var descriptions = new List<string>();

            // 1. Extraer descripciones de los comentarios XML
            foreach (var name in names)
            {
                var memberInfo = context.Type.GetMember(name).FirstOrDefault();
                descriptions.Add(GetXmlSummary(memberInfo));
            }

            // 2. Inyectar extensiones x-
            concreteSchema.Extensions["x-enumNames"] = new OpenApiRawArray(names);
            concreteSchema.Extensions["x-enumDescriptions"] = new OpenApiRawArray(descriptions);

            // 3. Construir la descripción visual "Formas de pago..."
            var sb = new StringBuilder();
            var enumSummary = GetXmlSummary(context.Type); // Comentario general del Enum

            sb.Append(!string.IsNullOrEmpty(enumSummary) ? enumSummary : "Valores permitidos:");

            for (int i = 0; i < names.Count; i++)
            {
                var d = descriptions[i];
                // Formato: 1 = Efectivo (En efectivo.)
                sb.Append($"\n\n{values[i]} = {names[i]}");
                if (!string.IsNullOrEmpty(d)) sb.Append($" ({d})");
            }

            concreteSchema.Description = sb.ToString();
        }
    }

    private string GetXmlSummary(MemberInfo memberInfo)
    {
        if (memberInfo == null || !_xmlNavigators.Any()) return string.Empty;

        // F para campos (enum values), T para tipos (la clase enum)
        string prefix = memberInfo is Type ? "T:" : "F:";
        string memberName = $"{prefix}{memberInfo.DeclaringType?.FullName ?? memberInfo.ReflectedType?.FullName}.{memberInfo.Name}";

        // Si es el tipo en sí, el nombre es diferente
        if (memberInfo is Type t) memberName = $"T:{t.FullName}";

        foreach (var nav in _xmlNavigators)
        {
            var node = nav.SelectSingleNode($"/doc/members/member[@name='{memberName}']/summary");
            if (node != null) return node.InnerXml.Trim();
        }
        return string.Empty;
    }
}


internal class OpenApiRawArray : List<string>, IOpenApiExtension
{
    public OpenApiRawArray(IEnumerable<string> values) : base(values) { }

    public void SerializeAsV31(IOpenApiWriter writer) => Write(writer);
    public void SerializeAsV3(IOpenApiWriter writer) => Write(writer);
    public void SerializeAsV2(IOpenApiWriter writer) => Write(writer);

    public void Write(IOpenApiWriter writer)
    {
        writer.WriteStartArray();
        foreach (var val in this) writer.WriteValue(val);
        writer.WriteEndArray();
    }

    public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
    {
        Write(writer);
    }
}
