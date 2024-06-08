namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Rating { get; set; }

        public List<Bid> Bids { get; set; } = new();
    }
}
