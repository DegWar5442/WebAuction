using WebApplication1.Models;

public class Bid
{
    public int Id { get; set; }
    public int LotId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }

    public Lot Lot { get; set; }
    public User User { get; set; }
}
