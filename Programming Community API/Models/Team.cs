using System.Diagnostics.CodeAnalysis;

namespace Programming_Community_API.Models
{
    public class Team
    {
        public int team_id { get; set; }
         public string team_name { get; set; }



        public string? password { get; set; } = null;


        public int? created_by { get; set; }
    }
}