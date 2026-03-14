namespace Programming_Community_API.Models
{
    public class Quiz
    {
        public int quiz_id { get; set; }
        public string quiz_title { get; set; }
        public string quiz_description { get; set; }

        public int expert_id { get; set; }
    }
}