using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        public  HttpResponseMessage Post(FootballPlayer footballPlayer) 
        {

            footballPlayer.Id = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
            players.Add(footballPlayer);

            var response = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent($"Player with ID {footballPlayer.Id} has been created."),
                ReasonPhrase = "Player Created"
            };

            
            response.Headers.Location = new System.Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{footballPlayer.Id}");

            return response;


        }

        [HttpPut("UpdateFootballPlayer/{id}")]

        public HttpResponseMessage Update(int id, FootballPlayer footballPlayer)
        {

            var playerToUpdate = players.FirstOrDefault(x => x.Id == id);

            if (playerToUpdate == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Player with ID {id} not found."),
                    ReasonPhrase = "Player Not Found"
                };
            }
            players[id] = footballPlayer;

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"Player with ID {id} has been updated."),
                ReasonPhrase = "Player Updated"
            };
            
        }

            [HttpDelete("DeleteFootballPlayer/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            
            var playerToDelete = players.FirstOrDefault(x => x.Id == id);

           
            if (playerToDelete == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Player with ID {id} not found."),
                    ReasonPhrase = "Player Not Found"
                };
            }

           
            players.Remove(playerToDelete);


            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
