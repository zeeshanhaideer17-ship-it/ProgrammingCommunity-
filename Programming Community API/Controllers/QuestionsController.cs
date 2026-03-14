using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly string _connectionString;

        public QuestionsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE QUESTION
        [HttpPost]
        public IActionResult CreateQuestion(Question q)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Question
                    (questionbank_id, subject_code, user_id, title, question,
                     option1, option2, option3, option4, answer, difficulty)
                    VALUES
                    (@questionbank_id, @subject_code, @user_id, @title, @question,
                     @option1, @option2, @option3, @option4, @answer, @difficulty)", con);

                cmd.Parameters.AddWithValue("@questionbank_id", q.questionbank_id);
                cmd.Parameters.AddWithValue("@subject_code", q.subject_code ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@user_id", q.user_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@title", q.title);
                cmd.Parameters.AddWithValue("@question", q.question);
                cmd.Parameters.AddWithValue("@option1", q.option1);
                cmd.Parameters.AddWithValue("@option2", q.option2);
                cmd.Parameters.AddWithValue("@option3", q.option3);
                cmd.Parameters.AddWithValue("@option4", q.option4);
                cmd.Parameters.AddWithValue("@answer", q.answer);
                cmd.Parameters.AddWithValue("@difficulty", q.difficulty);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Question created successfully");
        }

        // ✅ GET ALL QUESTIONS
        [HttpGet]
        public IActionResult GetAllQuestions()
        {
            List<Question> questions = new List<Question>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Question", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    questions.Add(MapQuestion(reader));
                }
            }

            return Ok(questions);
        }

        // ✅ GET QUESTION BY ID
        [HttpGet("{id}")]
        public IActionResult GetQuestionById(int id)
        {
            Question q = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Question WHERE question_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    q = MapQuestion(reader);
                }
            }

            if (q == null)
                return NotFound("Question not found");

            return Ok(q);
        }

        // ✅ UPDATE QUESTION
        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(int id, Question q)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Question SET
                        questionbank_id = @questionbank_id,
                        subject_code = @subject_code,
                        user_id = @user_id,
                        title = @title,
                        question = @question,
                        option1 = @option1,
                        option2 = @option2,
                        option3 = @option3,
                        option4 = @option4,
                        answer = @answer,
                        difficulty = @difficulty
                      WHERE question_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@questionbank_id", q.questionbank_id);
                cmd.Parameters.AddWithValue("@subject_code", q.subject_code ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@user_id", q.user_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@title", q.title);
                cmd.Parameters.AddWithValue("@question", q.question);
                cmd.Parameters.AddWithValue("@option1", q.option1);
                cmd.Parameters.AddWithValue("@option2", q.option2);
                cmd.Parameters.AddWithValue("@option3", q.option3);
                cmd.Parameters.AddWithValue("@option4", q.option4);
                cmd.Parameters.AddWithValue("@answer", q.answer);
                cmd.Parameters.AddWithValue("@difficulty", q.difficulty);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Question not found");
            }

            return Ok("Question updated successfully");
        }

        // ✅ DELETE QUESTION
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Question WHERE question_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("Question not found");
            }

            return Ok("Question deleted successfully");
        }

        // 🔹 Helper Mapper
        private Question MapQuestion(SqlDataReader reader)
        {
            return new Question
            {
                question_id = (int)reader["question_id"],
                questionbank_id = (int)reader["questionbank_id"],
                subject_code = reader["subject_code"]?.ToString(),
                user_id = reader["user_id"] == DBNull.Value ? null : (int?)reader["user_id"],
                title = reader["title"]?.ToString(),
                question = reader["question"]?.ToString(),
                option1 = reader["option1"]?.ToString(),
                option2 = reader["option2"]?.ToString(),
                option3 = reader["option3"]?.ToString(),
                option4 = reader["option4"]?.ToString(),
                answer = reader["answer"]?.ToString(),
                difficulty = reader["difficulty"]?.ToString()
            };
        }
    }
}