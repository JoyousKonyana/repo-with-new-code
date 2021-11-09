namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class QuizQuestionOption
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public bool IsOptionAnswer { get; set; }

        public QuizQuestion QuizQuestion { get; set; }
        public int QuizQuestionId { get; set; }
    }
}