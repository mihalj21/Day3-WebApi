using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class FootBallPlayerController: ControllerBase   
    {
        
        private static List<FootballPlayer> players = new List<FootballPlayer>
        {
            new FootballPlayer { Id=0, FirstName = "John", LastName = "Doe", Nationality = "American", Club = "Club A", Age = 25 },
            
        };


        private readonly ILogger<FootBallPlayerController> _logger;

        public FootBallPlayerController(ILogger<FootBallPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetFootballPlayer")]
        public IEnumerable<FootballPlayer> Get()
        {
            return players;
        }

        [HttpPost("PostFootballPlayer")]

        public  ActionResult<FootballPlayer> Post(FootballPlayer footballPlayer) 
        {

           footballPlayer.Id = players.Count;
             players.Add(footballPlayer);
            return CreatedAtAction(nameof(Get), new { Id = footballPlayer.Id }, footballPlayer);

            
            
        }

        [HttpPut("UpdateFootballPlayer/{id}")]

        public bool Update(int id, FootballPlayer footballPlayer) {
         if(!players.Any(x => x.Id == id)) return false;

            players[id] = footballPlayer;
            players[id].Id = id;
            return true;
        }


        [HttpDelete("DeleteFootballPlayer/{id}")]
        public ActionResult Delete(int id)
        {
            // Find the player with the specified id
            var playerToDelete = players.FirstOrDefault(x => x.Id == id);

            // If player not found, return 404 Not Found
            if (playerToDelete == null)
            {
                return NotFound();
            }

            // Remove the player from the list
            players.Remove(playerToDelete);

            // Return 204 No Content
            return NoContent();
        }
    }
}
