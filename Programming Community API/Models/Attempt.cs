namespace Programming_Community_API.Models
{
    public class Attempt
    {
        public int attempt_id { get; set; }
        public int student_id { get; set; }
        public int quiz_id { get; set; }
        public DateTime attempt_date { get; set; }
    }
}