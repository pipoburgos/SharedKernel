//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using System.ComponentModel;

//namespace SharedKernel.Infrastructure.RedsysTPV.Helpers;

///// <summary> . </summary>
//public static class EnumExtensions
//{
//    /// <summary> . </summary>
//    public static string GetDescription(this Enum value)
//    {
//        var field = value.GetType().GetField(value.ToString());

//        return !(Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) ? value.ToString() : attribute.Description;
//    }
//}

///// <summary> . </summary>
//internal sealed class EnumDescriptionConverter : StringEnumConverter
//{
//    /// <summary> . </summary>
//    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//    {
//        writer.WriteValue((value as Enum)?.GetDescription() ?? value);
//    }
//}