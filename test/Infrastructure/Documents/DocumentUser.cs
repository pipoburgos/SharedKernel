namespace SharedKernel.Integration.Tests.Documents;
public class DocumentUser
{
    public int Identifier { get; set; }
    public string Username { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Date { get; set; }
}
