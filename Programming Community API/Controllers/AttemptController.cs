using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly string _connectionString;

        public AttemptController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE ATTEMPT
        [HttpPost]
        public IActionResult CreateAttempt(Attempt attempt)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Attempt (student_id, quiz_id)
                      VALUES (@student_id, @quiz_id)", con);

                cmd.Parameters.AddWithValue("@student_id", attempt.student_id);
                cmd.Parameters.AddWithValue("@quiz_id", attempt.quiz_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Attempt recorded successfully");
        }

        // ✅ GET ALL ATTEMPTS
        [HttpGet]
        public IActionResult GetAllAttempts()
        {
            List<Attempt> attempts = new List<Attempt>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Attempt", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    attempts.Add(new Attempt
                    {
                        attempt_id = (int)reader["attempt_id"],
                        student_id = (int)reader["student_id"],
                        quiz_id = (int)reader["quiz_id"],
                        attempt_date = (DateTime)reader["attempt_date"]
                    });
                }
            }

            return Ok(attempts);
        }

        // ✅ GET ATTEMPTS BY STUDENT
        [HttpGet("student/{studentId}")]
        public IActionResult GetAttemptsByStudent(int studentId)
        {
            List<Attempt> attempts = new List<Attempt>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Attempt WHERE student_id = @student_id", con);

                cmd.Parameters.AddWithValue("@student_id", studentId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    attempts.Add(new Attempt
                    {
                        attempt_id = (int)reader["attempt_id"],
                        student_id = (int)reader["student_id"],
                        quiz_id = (int)reader["quiz_id"],
                        attempt_date = (DateTime)reader["attempt_date"]
                    });
                }
            }

            return Ok(attempts);
        }

        // ✅ GET ATTEMPTS BY QUIZ
        [HttpGet("quiz/{quizId}")]
        public IActionResult GetAttemptsByQuiz(int quizId)
        {
            List<Attempt> attempts = new List<Attempt>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Attempt WHERE quiz_id = @quiz_id", con);

                cmd.Parameters.AddWithValue("@quiz_id", quizId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    attempts.Add(new Attempt
                    {
                        attempt_id = (int)reader["attempt_id"],
                        student_id = (int)reader["student_id"],
                        quiz_id = (int)reader["quiz_id"],
                        attempt_date = (DateTime)reader["attempt_date"]
                    });
                }
            }

            return Ok(attempts);
        }

        // ✅ DELETE ATTEMPT
        [HttpDelete("{id}")]
        public IActionResult DeleteAttempt(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Attempt WHERE attempt_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("Attempt not found");
            }

            return Ok("Attempt deleted successfully");
        }
    }
}