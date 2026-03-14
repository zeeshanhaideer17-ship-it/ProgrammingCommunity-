using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertsController : ControllerBase
    {
        private readonly string _connectionString;

        public ExpertsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE EXPERT
        [HttpPost]
        public IActionResult CreateExpert(Expert expert)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Expert (expert_id, expertise)
                      VALUES (@expert_id, @expertise)", con);

                cmd.Parameters.AddWithValue("@expert_id", expert.expert_id);
                cmd.Parameters.AddWithValue("@expertise", expert.expertise);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Expert created successfully");
        }

        // ✅ GET EXPERT BY ID
        [HttpGet("{id}")]
        public IActionResult GetExpertById(int id)
        {
            Expert expert = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Expert WHERE expert_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    expert = new Expert
                    {
                        expert_id = (int)reader["expert_id"],
                        expertise = reader["expertise"].ToString()
                    };
                }
            }

            if (expert == null)
                return NotFound("Expert not found");

            return Ok(expert);
        }
    }
}