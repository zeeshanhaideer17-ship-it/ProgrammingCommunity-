using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly string _connectionString;

        public RoundController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE ROUND
        [HttpPost]
        public IActionResult CreateRound(Round round)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Round (competition_id, round_type, title, quiz_id)
                      VALUES (@competition_id, @round_type, @title, @quiz_id)", con);

                cmd.Parameters.AddWithValue("@competition_id", round.competition_id);
                cmd.Parameters.AddWithValue("@round_type", round.round_type ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@title", round.title ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@quiz_id", round.quiz_id ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Round created successfully");
        }

        // ✅ GET ALL ROUNDS
        [HttpGet]
        public IActionResult GetAllRounds()
        {
            List<Round> list = new List<Round>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Round", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Round
                    {
                        round_id = (int)reader["round_id"],
                        competition_id = (int)reader["competition_id"],
                        round_type = reader["round_type"]?.ToString(),
                        title = reader["title"]?.ToString(),
                        quiz_id = reader["quiz_id"] == DBNull.Value ? null : (int?)reader["quiz_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ GET ROUNDS BY COMPETITION
        [HttpGet("competition/{competitionId}")]
        public IActionResult GetRoundsByCompetition(int competitionId)
        {
            List<Round> list = new List<Round>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Round WHERE competition_id = @competition_id", con);

                cmd.Parameters.AddWithValue("@competition_id", competitionId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Round
                    {
                        round_id = (int)reader["round_id"],
                        competition_id = (int)reader["competition_id"],
                        round_type = reader["round_type"]?.ToString(),
                        title = reader["title"]?.ToString(),
                        quiz_id = reader["quiz_id"] == DBNull.Value ? null : (int?)reader["quiz_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ UPDATE ROUND
        [HttpPut("{id}")]
        public IActionResult UpdateRound(int id, Round round)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Round SET
                        competition_id = @competition_id,
                        round_type = @round_type,
                        title = @title,
                        quiz_id = @quiz_id
                      WHERE round_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@competition_id", round.competition_id);
                cmd.Parameters.AddWithValue("@round_type", round.round_type ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@title", round.title ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@quiz_id", round.quiz_id ?? (object)DBNull.Value);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Round not found");
            }

            return Ok("Round updated successfully");
        }

        // ✅ DELETE ROUND
        [HttpDelete("{id}")]
        public IActionResult DeleteRound(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Round WHERE round_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Round not found");
            }

            return Ok("Round deleted successfully");
        }
    }
}