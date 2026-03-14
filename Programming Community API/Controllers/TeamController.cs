using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using Programming_Community_API.Models;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly string _connectionString;

        public TeamController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ CREATE TEAM
        [HttpPost]
        public IActionResult CreateTeam(Team team)
        {
            Team createdTeam = new Team();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Team (team_name, created_by)
                  VALUES (@team_name, @created_by)", con);

                cmd.Parameters.AddWithValue("@team_name", team.team_name);
                cmd.Parameters.AddWithValue("@created_by", team.created_by ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();

                // Get last inserted team
                SqlCommand cmd2 = new SqlCommand("SELECT TOP 1 * FROM Team ORDER BY team_id DESC", con);
                SqlDataReader reader = cmd2.ExecuteReader();

                if (reader.Read())
                {
                    createdTeam.team_id = (int)reader["team_id"];
                    createdTeam.team_name = reader["team_name"].ToString();
                    createdTeam.password = reader["password"].ToString();
                    createdTeam.created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"];
                }
            }

            return Ok(createdTeam);
        }

        // ✅ GET ALL TEAMS
        [HttpGet]
        public IActionResult GetAllTeams()
        {
            List<Team> teams = new List<Team>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Team", con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    teams.Add(new Team
                    {
                        team_id = (int)reader["team_id"],
                        team_name = reader["team_name"].ToString(),
                        password = reader["password"].ToString(),
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"]
                    });
                }
            }

            return Ok(teams);
        }

        // ✅ GET TEAM BY ID
        [HttpGet("id/{id}")]
        public IActionResult GetTeamById(int id)
        {
            Team team = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Team WHERE team_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    team = new Team
                    {
                        team_id = (int)reader["team_id"],
                        team_name = reader["team_name"].ToString(),
                        password = reader["password"].ToString(),
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"]
                    };
                }
            }

            if (team == null)
                return NotFound("Team not found");

            return Ok(team);
        }



        [HttpGet("userid/{userid}")]
        public IActionResult GetTeamByuserid(int userid)
        {
            Team team = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Team WHERE created_by = @userid order by team_id desc", con);
                cmd.Parameters.AddWithValue("@userid", userid);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    team = new Team
                    {
                        team_id = (int)reader["team_id"],
                        team_name = reader["team_name"].ToString(),
                        password = reader["password"].ToString(),
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"]
                    };
                }
            }

            if (team == null)
                return NotFound("Team not found");

            return Ok(team);
        }


        // ✅ GET TEAM BY PASSWORD
        [HttpGet("password/{password}")]
        public IActionResult GetTeamByPassword(string password)
        {
            Team team = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Team WHERE password = @password", con);
                cmd.Parameters.AddWithValue("@password", password);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    team = new Team
                    {
                        team_id = (int)reader["team_id"],
                        team_name = reader["team_name"].ToString(),
                        password = reader["password"].ToString(),
                        created_by = reader["created_by"] == DBNull.Value ? null : (int?)reader["created_by"]
                    };
                }
            }

            if (team == null)
                return NotFound("Team not found");

            return Ok(team);
        }

        // ✅ UPDATE TEAM
        [HttpPut("{id}")]
        public IActionResult UpdateTeam(int id, Team team)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                @"UPDATE Team 
                  SET team_name = @team_name, created_by = @created_by
                  WHERE team_id = @id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@team_name", team.team_name);
                cmd.Parameters.AddWithValue("@created_by", team.created_by ?? (object)DBNull.Value);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Team not found");
            }

            return Ok("Team updated successfully");
        }

        // ✅ DELETE TEAM
        [HttpDelete("{id}")]
        public IActionResult DeleteTeam(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Team WHERE team_id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("Team not found");
            }

            return Ok("Team deleted successfully");
        }
    }
}