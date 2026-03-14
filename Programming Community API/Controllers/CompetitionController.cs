using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly string _connectionString;

        public CompetitionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE COMPETITION
        [HttpPost]
        public IActionResult CreateCompetition(Competition comp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Competition (title, password, start_date, end_date, created_by, rounds)
                      VALUES (@title, @password, @start_date, @end_date, @created_by, @rounds)", con);

                cmd.Parameters.AddWithValue("@title", comp.title);
                cmd.Parameters.AddWithValue("@password", comp.password ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@start_date", comp.start_date ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@end_date", comp.end_date ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@created_by", comp.created_by ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@rounds", comp.rounds ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Competition created successfully");
        }

        // ✅ GET ALL COMPETITIONS
        [HttpGet]
        public IActionResult GetAllCompetitions()
        {
            List<Competition> list = new List<Competition>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Competition", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Competition
                    {
                        competition_id = (int)reader["competition_id"],
                        title = reader["title"].ToString(),
                        password = reader["password"]?.ToString(),
                        start_date = reader["start_date"] == DBNull.Value ? null : (DateTime?)reader["start_date"],
                        end_date = reader["end_date"] == DBNull.Value ? null : (DateTime?)reader["end_date"],
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"],
                        rounds = reader["rounds"] == DBNull.Value ? null : (int?)reader["rounds"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ GET COMPETITION BY ID
        [HttpGet("{id}")]
        public IActionResult GetCompetitionById(int id)
        {
            Competition comp = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Competition WHERE competition_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    comp = new Competition
                    {
                        competition_id = (int)reader["competition_id"],
                        title = reader["title"].ToString(),
                        password = reader["password"]?.ToString(),
                        start_date = reader["start_date"] == DBNull.Value ? null : (DateTime?)reader["start_date"],
                        end_date = reader["end_date"] == DBNull.Value ? null : (DateTime?)reader["end_date"],
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"],
                        rounds = reader["rounds"] == DBNull.Value ? null : (int?)reader["rounds"]
                    };
                }
            }

            if (comp == null) return NotFound("Competition not found");
            return Ok(comp);
        }

        // ✅ UPDATE COMPETITION
        [HttpPut("{id}")]
        public IActionResult UpdateCompetition(int id, Competition comp)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Competition SET
                        title = @title,
                        password = @password,
                        start_date = @start_date,
                        end_date = @end_date,
                        created_by = @created_by,
                        rounds = @rounds
                      WHERE competition_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@title", comp.title);
                cmd.Parameters.AddWithValue("@password", comp.password ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@start_date", comp.start_date ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@end_date", comp.end_date ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@created_by", comp.created_by ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@rounds", comp.rounds ?? (object)DBNull.Value);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Competition not found");
            }

            return Ok("Competition updated successfully");
        }

        // ✅ DELETE COMPETITION
        [HttpDelete("{id}")]
        public IActionResult DeleteCompetition(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Competition WHERE competition_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Competition not found");
            }

            return Ok("Competition deleted successfully");
        }
    }
}