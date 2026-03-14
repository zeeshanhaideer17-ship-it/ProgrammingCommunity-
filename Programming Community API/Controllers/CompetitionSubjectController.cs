using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionSubjectController : ControllerBase
    {
        private readonly string _connectionString;

        public CompetitionSubjectController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ ADD SUBJECT TO COMPETITION
        [HttpPost]
        public IActionResult AddSubject(CompetitionSubject cs)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO CompetitionSubject (competition_id, subject_id) VALUES (@competition_id, @subject_id)", con);

                cmd.Parameters.AddWithValue("@competition_id", cs.competition_id);
                cmd.Parameters.AddWithValue("@subject_id", cs.subject_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Subject added to competition");
        }

        // ✅ GET SUBJECTS FOR COMPETITION
        [HttpGet("competition/{competitionId}")]
        public IActionResult GetSubjectsByCompetition(int competitionId)
        {
            List<CompetitionSubject> list = new List<CompetitionSubject>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CompetitionSubject WHERE competition_id = @competition_id", con);
                cmd.Parameters.AddWithValue("@competition_id", competitionId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new CompetitionSubject
                    {
                        competition_id = (int)reader["competition_id"],
                        subject_id = (int)reader["subject_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ REMOVE SUBJECT FROM COMPETITION
        [HttpDelete]
        public IActionResult RemoveSubject([FromBody] CompetitionSubject cs)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM CompetitionSubject WHERE competition_id = @competition_id AND subject_id = @subject_id", con);

                cmd.Parameters.AddWithValue("@competition_id", cs.competition_id);
                cmd.Parameters.AddWithValue("@subject_id", cs.subject_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Subject not found in competition");
            }

            return Ok("Subject removed from competition");
        }
    }
}