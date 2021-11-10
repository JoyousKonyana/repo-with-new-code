using System;
using System.Collections.Generic;
using BMW_ONBOARDING_SYSTEM.Models;

namespace BMW_ONBOARDING_SYSTEM.Dtos
{

    public class GetLessonOutcomeQuizDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int LessonId { get; set; }
        public string LessonName { get; set; }

   

        public int Id { get; set; }

        public string Name { get; set; }

        public string DueDate { get; set; }

        public int NumberOfQuestions { get; set; }
        public int LessonOutcomeId { get; set; }
        public string LessonOutcomeName { get; set; }

        public int QuestionBankId { get; set; }
        public string QuestionBankName { get; set; }
    }
}