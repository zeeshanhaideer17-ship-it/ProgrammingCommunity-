using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Programming_Community_API.Models;
using System.Collections.Generic;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly string _connectionString;

        public SubjectsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE SUBJECT
        [HttpPost]
        public IActionResult CreateSubject(Subject subject)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Subject (code, title)
                      VALUES (@code, @title)", con);

                cmd.Parameters.AddWithValue("@code", subject.code);
                cmd.Parameters.AddWithValue("@title", subject.title);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Subject created successfully");
        }

        // ✅ GET ALL SUBJECTS
        [HttpGet]
        public IActionResult GetAllSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Subject", con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    subjects.Add(new Subject
                    {
                        subject_id = (int)reader["subject_id"],
                        code = reader["code"].ToString(),
                        title = reader["title"].ToString()
                    });
                }
            }

            return Ok(subjects);
        }

        // ✅ GET SUBJECT BY ID
        [HttpGet("{id}")]
        public IActionResult GetSubjectById(int id)
        {
            Subject subject = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Subject WHERE subject_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    subject = new Subject
                    {
                        subject_id = (int)reader["subject_id"],
                        code = reader["code"].ToString(),
                        title = reader["title"].ToString()
                    };
                }
            }

            if (subject == null)
                return NotFound("Subject not found");

            return Ok(subject);
        }

        // ✅ UPDATE SUBJECT
        [HttpPut("{id}")]
        public IActionResult UpdateSubject(int id, Subject subject)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Subject 
                      SET code = @code, title = @title 
                      WHERE subject_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@code", subject.code);
                cmd.Parameters.AddWithValue("@title", subject.title);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Subject not found");
            }

            return Ok("Subject updated successfully");
        }

        // ✅ DELETE SUBJECT
        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Subject WHERE subject_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound("Subject not found");
            }

            return Ok("Subject deleted successfully");
        }
    }
}