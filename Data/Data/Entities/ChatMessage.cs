namespace Data.Data.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Message { get; set; }
    public DateTime Created { get; set; }
}
