using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly string _connectionString;

        public TeamMemberController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ ADD MEMBER TO TEAM
        [HttpPost]
        public IActionResult AddMember(TeamMember member)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // 1️⃣ Check if student is already in team
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM TeamMember WHERE team_id = @team_id AND student_id = @student_id", con);
                checkCmd.Parameters.AddWithValue("@team_id", member.team_id);
                checkCmd.Parameters.AddWithValue("@student_id", member.student_id);

                int exists = (int)checkCmd.ExecuteScalar();
                if (exists > 0)
                    return BadRequest("Student is already a member of this team");

                // 2️⃣ Insert member
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO TeamMember (team_id, student_id)
              VALUES (@team_id, @student_id)", con);

                cmd.Parameters.AddWithValue("@team_id", member.team_id);
                cmd.Parameters.AddWithValue("@student_id", member.student_id);

                cmd.ExecuteNonQuery();
            }

            return Ok("Member added to team successfully");
        }

        // ✅ GET MEMBERS BY TEAM
        [HttpGet("team/{teamId}")]
        public IActionResult GetMembersByTeam(int teamId)
        {
            List<TeamMember> list = new List<TeamMember>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM TeamMember WHERE team_id = @team_id", con);

                cmd.Parameters.AddWithValue("@team_id", teamId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new TeamMember
                    {
                        team_id = (int)reader["team_id"],
                        student_id = (int)reader["student_id"]
                    });
                }
            }

            return Ok(list);
        }

        // ✅ REMOVE MEMBER FROM TEAM
        [HttpDelete]
        public IActionResult RemoveMember([FromBody] TeamMember member)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM TeamMember WHERE team_id = @team_id AND student_id = @student_id", con);

                cmd.Parameters.AddWithValue("@team_id", member.team_id);
                cmd.Parameters.AddWithValue("@student_id", member.student_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound("Member not found in team");
            }

            return Ok("Member removed from team successfully");
        }
    }
}