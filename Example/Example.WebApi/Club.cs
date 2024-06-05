namespace Example.WebApi
{
    public class Club
    {
        public Guid  Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public ICollection<FootballPlayer> Players { get; set; }
    }
}
