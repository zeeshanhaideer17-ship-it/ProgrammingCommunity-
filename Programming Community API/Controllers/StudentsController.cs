using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString;

        public StudentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE STUDENT
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Student (student_id, level)
                      VALUES (@student_id, @level)", con);

                cmd.Parameters.AddWithValue("@student_id", student.student_id);
                cmd.Parameters.AddWithValue("@level", student.level);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Student created successfully");
        }

        // ✅ GET STUDENT BY ID
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            Student student = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Student WHERE student_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    student = new Student
                    {
                        student_id = (int)reader["student_id"],
                        level = reader["level"].ToString()
                    };
                }
            }

            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }

        // ✅ UPDATE STUDENT
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Student 
                      SET level = @level 
                      WHERE student_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@level", student.level);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Student not found");
            }

            return Ok("Student updated successfully");
        }

        // ✅ DELETE STUDENT
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Student WHERE student_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Student not found");
            }

            return Ok("Student deleted successfully");
        }
    }
}