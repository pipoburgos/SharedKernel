namespace SharedKernel.Domain.ValueObjects.PersonalDocumentation;

/// <summary> </summary>
public class Nif
{
    /// <summary> </summary>
    protected Nif() { }

    /// <summary> </summary>
    protected Nif(string value)
    {
        Value = value;
    }

    /// <summary> </summary>
    public static Nif Create(string value)
    {
        return new Nif(value);
    }

    /// <summary> NIE value. </summary>
    public string Value { get; private set; } = null!;

    /// <summary> . </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return Dni.Create(Value).IsValid() || Nie.Create(Value).IsValid();
    }
}