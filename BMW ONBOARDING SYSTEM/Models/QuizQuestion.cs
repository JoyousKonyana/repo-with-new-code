using System.Collections.Generic;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Quiz Quiz { get; set; }
        public int QuizId { get; set; }

        public virtual List<QuizQuestionOption> Options { get; set; }

        public QuizQuestion()
        {
            Options = new List<QuizQuestionOption>();
        }
    }
}