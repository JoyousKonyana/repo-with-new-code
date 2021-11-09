using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        public virtual LessonOutcome LessonOutcome { get; set; }
        public int LessonOutcomeID { get; set; }

        public virtual QuestionBank QuestionBank { get; set; }
        public int QuestionBankId { get; set; }

        public virtual List<Achievement> Achievements { get; set; }

        public virtual List<QuizQuestion> Questions { get; set; }

        public Quiz()
        {
            Achievements = new List<Achievement>();
            Questions = new List<QuizQuestion>();

        }

    }
}
