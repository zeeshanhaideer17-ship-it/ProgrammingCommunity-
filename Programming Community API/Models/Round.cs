namespace Programming_Community_API.Models
{
    public class Round
    {
        public int round_id { get; set; }
        public int competition_id { get; set; }
        public string round_type { get; set; }
        public string title { get; set; }
        public int? quiz_id { get; set; }
    }
}
