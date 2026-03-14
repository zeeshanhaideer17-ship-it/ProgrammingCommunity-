using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizQuestionController : ControllerBase
    {
        private readonly string _connectionString;

        public QuizQuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ ADD QUESTION TO QUIZ
        [HttpPost]
        public IActionResult AddQuestionToQuiz(QuizQuestion qq)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO QuizQuestion (quiz_id, question_id)
                      VALUES (@quiz_id, @question_id)", con);

                cmd.Parameters.AddWithValue("@quiz_id", qq.quiz_id);
                cmd.Parameters.AddWithValue("@question_id", qq.question_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Question added to quiz successfully");
        }

        // ✅ GET ALL QUESTIONS FOR A QUIZ
        [HttpGet("quiz/{quizId}")]
        public IActionResult GetQuestionsByQuiz(int quizId)
        {
            List<QuizQuestion> list = new List<QuizQuestion>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM QuizQuestion WHERE quiz_id = @quiz_id", con);

                cmd.Parameters.AddWithValue("@quiz_id", quizId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new QuizQuestion
                    {
                        quiz_id = (int)reader["quiz_id"],
                        question_id = (int)reader["question_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ GET ALL QUIZZES FOR A QUESTION
        [HttpGet("question/{questionId}")]
        public IActionResult GetQuizzesByQuestion(int questionId)
        {
            List<QuizQuestion> list = new List<QuizQuestion>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM QuizQuestion WHERE question_id = @question_id", con);

                cmd.Parameters.AddWithValue("@question_id", questionId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new QuizQuestion
                    {
                        quiz_id = (int)reader["quiz_id"],
                        question_id = (int)reader["question_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ REMOVE QUESTION FROM QUIZ
        [HttpDelete]
        public IActionResult RemoveQuestionFromQuiz([FromBody] QuizQuestion qq)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM QuizQuestion WHERE quiz_id = @quiz_id AND question_id = @question_id", con);

                cmd.Parameters.AddWithValue("@quiz_id", qq.quiz_id);
                cmd.Parameters.AddWithValue("@question_id", qq.question_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Mapping not found");
            }

            return Ok("Question removed from quiz successfully");
        }
    }
}