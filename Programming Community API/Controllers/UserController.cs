using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Programming_Community_API.Models;
using  Programming_Community_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ GET ALL USERS
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = new List<User>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        user_id = (int)reader["user_id"],
                        name = reader["name"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["role"].ToString()
                    });
                }
            }
            return Ok(users);
        }

        // ✅ GET USER BY ID
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            User user = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Users WHERE user_id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        user_id = (int)reader["user_id"],
                        name = reader["name"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString(),
                        role = reader["role"].ToString()
                    };
                }
            }

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // ✅ CREATE USER
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Users (name, email, password, role)
                      VALUES (@name, @email, @password, @role)", con);

                cmd.Parameters.AddWithValue("@name", user.name);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@role", user.role);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("User created successfully");
        }

        // ✅ UPDATE USER
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Users 
                      SET name=@name, email=@email, password=@password, role=@role
                      WHERE user_id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", user.name);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@role", user.role);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("User not found");
            }

            return Ok("User updated successfully");
        }

        // ✅ DELETE USER
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Users WHERE user_id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("User not found");
            }

            return Ok("User deleted successfully");
        }
       public  class loginreq {
            public string email {  get; set; }
            public string password { get; set; }
        }


        // ✅ LOGIN USER
        [HttpPost("login")]
        public IActionResult Login([FromBody] loginreq request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            User user = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT * FROM Users 
              WHERE email = @email AND password = @password", con);

                cmd.Parameters.AddWithValue("@email", request.email);
                cmd.Parameters.AddWithValue("@password", request.password);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        user_id = (int)reader["user_id"],
                        name = reader["name"].ToString(),
                        email = reader["email"].ToString(),
                        role = reader["role"].ToString()
                    };
                }
            }

            if (user == null)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                message = "Login successful",
                user = user
            });
        }
    }
}