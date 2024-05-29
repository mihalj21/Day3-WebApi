using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class FootBallPlayerController: ControllerBase   
    {


        private static readonly string[] FirstName = new[]
       {
            "Matko", "Mihovil", "Ivan", "Josip", "Messi", "Cristiano", "Sven", "Leon", "Matej", "Petar"
        };

        private static readonly string[] LastName = new[]
      {
            "Mihalj", "Leskovic", "Lovric", "Knezevic", "Bolic", "marek", "Petrovic", "Calis", "Guzvic", "Rozing"
        };
        private static readonly string[] Nationality = new[]
      {
            "Hrvat", "Spanjolac", "Srbin", "Makedonac", "Englez", "Švabo", "amerkinanac", "Japanac", "Indijac", "Kinez"
        };
        private static readonly string[] Club = new[]
      {
            "Barcelona", "real madrid", "City", "Bayern", "Chelsea", "United", "Dinamno", "Hajduk", "Osijek", "Rijeka"
        };
        

        private readonly ILogger<FootBallPlayerController> _logger;

        public FootBallPlayerController(ILogger<FootBallPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetFootballPlayer")]
        public IEnumerable<FootballPlayer> Get()
        {
            return Enumerable.Range(1, 7).Select(index => new FootballPlayer
            {
                FirstName = FirstName[Random.Shared.Next(FirstName.Length)],
                LastName = LastName[Random.Shared.Next(LastName.Length)],
                Nationality = Nationality[Random.Shared.Next(Nationality.Length)],
                Club = Club[Random.Shared.Next(Club.Length)],
                Age = Random.Shared.Next(18,35)
            })
            .ToArray();
        }
    }
}
