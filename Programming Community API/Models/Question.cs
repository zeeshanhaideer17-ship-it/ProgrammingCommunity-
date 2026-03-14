namespace Programming_Community_API.Models
{
    public class Question
    {
        public int question_id { get; set; }
        public int questionbank_id { get; set; }
        public string subject_code { get; set; }
        public int? user_id { get; set; }
        public string title { get; set; }
        public string question { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public string answer { get; set; }
        public string difficulty { get; set; }
    }
}