using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly string _connectionString;

        public QuestionBankController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE QUESTION BANK
        [HttpPost]
        public IActionResult CreateQuestionBank(QuestionBank qb)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO QuestionBank (subject_id)
                      VALUES (@subject_id)", con);

                cmd.Parameters.AddWithValue("@subject_id", qb.subject_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("QuestionBank created successfully");
        }

        // ✅ GET ALL QUESTION BANKS
        [HttpGet]
        public IActionResult GetAllQuestionBanks()
        {
            List<QuestionBank> banks = new List<QuestionBank>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM QuestionBank", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    banks.Add(new QuestionBank
                    {
                        questionbank_id = (int)reader["questionbank_id"],
                        subject_id = (int)reader["subject_id"]
                    });
                }
            }

            return Ok(banks);
        }

        // ✅ GET QUESTION BANK BY ID
        [HttpGet("{id}")]
        public IActionResult GetQuestionBankById(int id)
        {
            QuestionBank qb = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM QuestionBank WHERE questionbank_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    qb = new QuestionBank
                    {
                        questionbank_id = (int)reader["questionbank_id"],
                        subject_id = (int)reader["subject_id"]
                    };
                }
            }

            if (qb == null)
                return NotFound("QuestionBank not found");

            return Ok(qb);
        }

        // ✅ UPDATE QUESTION BANK
        [HttpPut("{id}")]
        public IActionResult UpdateQuestionBank(int id, QuestionBank qb)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE QuestionBank 
                      SET subject_id = @subject_id 
                      WHERE questionbank_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@subject_id", qb.subject_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("QuestionBank not found");
            }

            return Ok("QuestionBank updated successfully");
        }

        // ✅ DELETE QUESTION BANK
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestionBank(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM QuestionBank WHERE questionbank_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("QuestionBank not found");
            }

            return Ok("QuestionBank deleted successfully");
        }
    }
}