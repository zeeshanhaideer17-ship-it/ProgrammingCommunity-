using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly string _connectionString;

        public QuizController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE QUIZ
        [HttpPost]
        public IActionResult CreateQuiz(Quiz quiz)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Quiz (quiz_title,quiz_description, expert_id)
                      VALUES (@quiz_title,@quiz_description, @expert_id)", con);

                cmd.Parameters.AddWithValue("@quiz_title", quiz.quiz_title);
                cmd.Parameters.AddWithValue("@quiz_description", quiz.quiz_description);

                cmd.Parameters.AddWithValue("@expert_id", quiz.expert_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Quiz created successfully");
        }

        // ✅ GET ALL QUIZZES
        [HttpGet]
        public IActionResult GetAllQuizzes()
        {
            List<Quiz> quizzes = new List<Quiz>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Quiz", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quizzes.Add(new Quiz
                    {
                        quiz_id = (int)reader["quiz_id"],
                        quiz_title = reader["quiz_title"].ToString(),
                        quiz_description = reader["quiz_description"].ToString(),

                        expert_id = (int)reader["expert_id"]
                    });
                }
            }

            return Ok(quizzes);
        }

        // ✅ GET QUIZ BY ID
        [HttpGet("{id}")]
        public IActionResult GetQuizById(int id)
        {
            Quiz quiz = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Quiz WHERE quiz_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    quiz = new Quiz
                    {
                        quiz_id = (int)reader["quiz_id"],
                        quiz_title = reader["quiz_title"].ToString(),
                        quiz_description = reader["quiz_description"].ToString(),

                        expert_id = (int)reader["expert_id"]
                    };
                }
            }

            if (quiz == null)
                return NotFound("Quiz not found");

            return Ok(quiz);
        }

        // ✅ UPDATE QUIZ
        [HttpPut("{id}")]
        public IActionResult UpdateQuiz(int id, Quiz quiz)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Quiz 
                      SET quiz_title = @quiz_title, expert_id = @expert_id ,quiz_description=@quiz_description
                      WHERE quiz_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@quiz_title", quiz.quiz_title);
                cmd.Parameters.AddWithValue("@quiz_description", quiz.quiz_description);

                cmd.Parameters.AddWithValue("@expert_id", quiz.expert_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Quiz not found");
            }

            return Ok("Quiz updated successfully");
        }

        // ✅ DELETE QUIZ
        [HttpDelete("{id}")]
        public IActionResult DeleteQuiz(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Quiz WHERE quiz_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("Quiz not found");
            }

            return Ok("Quiz deleted successfully");
        }
    }
}