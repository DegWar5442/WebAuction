namespace WebApplication1.Models;
public class Comment
{
    public int Id { get; set; }

    public int LotId { get; set; }
    public Lot Lot { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
}