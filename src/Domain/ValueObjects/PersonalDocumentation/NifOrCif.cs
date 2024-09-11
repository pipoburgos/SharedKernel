namespace SharedKernel.Domain.ValueObjects.PersonalDocumentation;

/// <summary> </summary>
public class NifOrCif
{
    /// <summary> </summary>
    protected NifOrCif() { }

    /// <summary> </summary>
    public NifOrCif(string value)
    {
        Value = value;
    }

    /// <summary> </summary>
    public static NifOrCif Create(string value)
    {
        return new NifOrCif(value);
    }

    /// <summary> NIE value. </summary>
    public string Value { get; private set; } = null!;

    /// <summary> . </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return Nif.Create(Value).IsValid() || Cif.Create(Value).IsValid();
    }
}