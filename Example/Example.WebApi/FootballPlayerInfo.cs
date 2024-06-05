namespace Example.WebApi
{
    public class FootballPlayerInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public int Age { get; set; }
        public Guid? ClubId { get; set; }

        public string ClubName { get; set; }
        public string Country { get; set; }



    }
}
