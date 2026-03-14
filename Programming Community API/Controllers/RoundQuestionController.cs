using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundQuestionController : ControllerBase
    {
        private readonly string _connectionString;

        public RoundQuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ ADD QUESTION TO ROUND
        [HttpPost]
        public IActionResult AddQuestion(RoundQuestion rq)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO RoundQuestion (round_id, question_id) VALUES (@round_id, @question_id)", con);

                cmd.Parameters.AddWithValue("@round_id", rq.round_id);
                cmd.Parameters.AddWithValue("@question_id", rq.question_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Question added to round");
        }

        // ✅ GET QUESTIONS BY ROUND
        [HttpGet("round/{roundId}")]
        public IActionResult GetQuestionsByRound(int roundId)
        {
            List<RoundQuestion> list = new List<RoundQuestion>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM RoundQuestion WHERE round_id = @round_id", con);

                cmd.Parameters.AddWithValue("@round_id", roundId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new RoundQuestion
                    {
                        round_id = (int)reader["round_id"],
                        question_id = (int)reader["question_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ REMOVE QUESTION FROM ROUND
        [HttpDelete]
        public IActionResult RemoveQuestion([FromBody] RoundQuestion rq)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM RoundQuestion WHERE round_id = @round_id AND question_id = @question_id", con);

                cmd.Parameters.AddWithValue("@round_id", rq.round_id);
                cmd.Parameters.AddWithValue("@question_id", rq.question_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Question not found in round");
            }

            return Ok("Question removed from round");
        }
    }
}