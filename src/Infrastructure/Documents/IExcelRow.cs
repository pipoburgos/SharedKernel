namespace SharedKernel.Infrastructure.Documents;

/// <summary>  </summary>
public interface IExcelRow
{
    /// <summary>  </summary>
    T Get<T>(int index);

    /// <summary>  </summary>
    T Get<T>(string name);
}