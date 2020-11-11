namespace SharedKernel.Domain.Entities
{
    public interface IRowVersion
    {
        byte[] RowVersion { get; set; }
    }
}
