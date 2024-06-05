using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Net;


namespace Example.WebApi.Controllers
{



    [ApiController]
    [Route("[controller]")]

    public class FootBallPlayerController : ControllerBase
    {
        string connectionString = "Host = localhost; Port=5433;Database=FootballClub;Username=postgres;Password=mono";

        [HttpGet]
        [Route("checkconnection")]
        public IActionResult CheckConnection()
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {

                    connection.Open();
                    return Ok("Database connection successful.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error connecting to database: {ex.Message}");
            }
        }

       



        [HttpPost("PostFootballPlayer")]

        public ActionResult Post([FromBody] FootballPlayer player)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                var commandText = "INSERT INTO \"FootballPlayer\"VALUES(@id,@FirstName,@LastName,@Nationality,@Age,@ClubId);";
                using var command = new NpgsqlCommand(commandText, connection);


                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@FirstName", player.FirstName);
                command.Parameters.AddWithValue("@LastName", player.LastName);
                command.Parameters.AddWithValue("@Nationality", player.Nationality);
                command.Parameters.AddWithValue("@Age", player.Age);
                command.Parameters.AddWithValue("@ClubId", player.ClubId ?? (object)DBNull.Value);


                connection.Open();

                var numberOfCommits = command.ExecuteNonQuery();

                if (numberOfCommits == 0) {
                    return NotFound();
                }
                return Ok("Successfully added");


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }



        }
        [HttpGet("GetFootballPlayerByGUID")]

        public ActionResult GetById(Guid id) {


            try { 
            var footballPlayer = new FootballPlayer();
                using var connection = new NpgsqlConnection(connectionString);
            var commandText = "SELECT * FROM \"FootballPlayer\" WHERE\"Id\" = @id;";
            using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using var reader = command.ExecuteReader();

                if (reader.HasRows) {
                    reader.Read();

                    footballPlayer = new FootballPlayer
                    {
                        Id = Guid.Parse(reader[0].ToString()),
                        FirstName = reader[1].ToString(),
                        LastName = reader[2].ToString(),
                        Nationality = reader[3].ToString(),
                        Age = reader[4] == DBNull.Value ? 0 : Convert.ToInt32(reader[4]),
                        ClubId = reader[5] == DBNull.Value ? Guid.Empty : Guid.Parse(reader[5].ToString())
                        
                    };
                    
                }
                if (footballPlayer == null)
                {
                    return NotFound();
                }
                return Ok(footballPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Could not get player by ID: {ex.Message}");
            }
        }


        [HttpGet("GetFootballPlayers")]

        public ActionResult Get()
        {
            try
            {
                var footballPlayers = new List<FootballPlayerInfo>();

                using (var connection = new NpgsqlConnection(connectionString))
                using (var command = new NpgsqlCommand(@"SELECT fp.*, c.""Name"" AS ""ClubName"", c.""Country"" 
                                                FROM ""FootballPlayer"" fp
                                                INNER JOIN ""Club"" c ON fp.""ClubId"" = c.""Id"";",connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var footballPlayer = new FootballPlayerInfo
                            {
                                Id = Guid.Parse(reader[0].ToString()),
                                FirstName = reader[1].ToString(),
                                LastName = reader[2].ToString(),
                                Nationality = reader[3].ToString(),
                                Age = reader[4] == DBNull.Value ? 0 : Convert.ToInt32(reader[4]),
                                ClubId = Guid.Parse(reader["ClubId"].ToString()),
                                ClubName = reader["ClubName"].ToString(),
                                Country = reader["Country"].ToString()
                            };

                            footballPlayers.Add(footballPlayer);
                        }
                    }
                }

                return Ok(footballPlayers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Could not get football players: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlayer(Guid id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var commandText = "DELETE FROM \"FootballPlayer\" WHERE \"Id\" = @Id;";
            using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return Ok("Player deleted successfully");
            }
            else
            {
                return NotFound();
            }
        }
    } 
}
