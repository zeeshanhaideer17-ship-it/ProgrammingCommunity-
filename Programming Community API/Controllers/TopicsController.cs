using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly string _connectionString;

        public TopicsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE TOPIC
        [HttpPost]
        public IActionResult CreateTopic(Topic topic)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Topics (subject_id, subject_code, title)
                      VALUES (@subject_id, @subject_code, @title)", con);

                cmd.Parameters.AddWithValue("@subject_id", topic.subject_id);
                cmd.Parameters.AddWithValue("@subject_code", topic.subject_code);
                cmd.Parameters.AddWithValue("@title", topic.title);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Topic created successfully");
        }

        // ✅ GET ALL TOPICS
        [HttpGet]
        public IActionResult GetAllTopics()
        {
            List<Topic> topics = new List<Topic>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Topics", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    topics.Add(new Topic
                    {
                        topic_id = (int)reader["topic_id"],
                        subject_id = (int)reader["subject_id"],
                        subject_code = reader["subject_code"]?.ToString(),
                        title = reader["title"].ToString()
                    });
                }
            }

            return Ok(topics);
        }

        // ✅ GET TOPIC BY ID
        [HttpGet("{id}")]
        public IActionResult GetTopicById(int id)
        {
            Topic topic = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Topics WHERE topic_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    topic = new Topic
                    {
                        topic_id = (int)reader["topic_id"],
                        subject_id = (int)reader["subject_id"],
                        subject_code = reader["subject_code"]?.ToString(),
                        title = reader["title"].ToString()
                    };
                }
            }

            if (topic == null)
                return NotFound("Topic not found");

            return Ok(topic);
        }

        // ✅ UPDATE TOPIC
        [HttpPut("{id}")]
        public IActionResult UpdateTopic(int id, Topic topic)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Topics 
                      SET subject_id = @subject_id,
                          subject_code = @subject_code,
                          title = @title
                      WHERE topic_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@subject_id", topic.subject_id);
                cmd.Parameters.AddWithValue("@subject_code", topic.subject_code);
                cmd.Parameters.AddWithValue("@title", topic.title);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Topic not found");
            }

            return Ok("Topic updated successfully");
        }

        // ✅ DELETE TOPIC
        [HttpDelete("{id}")]
        public IActionResult DeleteTopic(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Topics WHERE topic_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("Topic not found");
            }

            return Ok("Topic deleted successfully");
        }
    }
}