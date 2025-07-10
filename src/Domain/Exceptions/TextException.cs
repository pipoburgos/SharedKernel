namespace SharedKernel.Domain.Exceptions;

[Serializable]
public class TextException : Exception
{
    /// <summary>
    /// Instanciates a new instance of the <see cref="TextException"/> class
    /// with a specified error code and an inner exception.
    /// </summary>
    public TextException() { }

    /// <summary>
    /// Instanciates a new instance of the <see cref="TextException"/> class
    /// with a specified error code and an inner exception.
    /// </summary>
    public TextException(string message) : base(message)
    {
    }
}