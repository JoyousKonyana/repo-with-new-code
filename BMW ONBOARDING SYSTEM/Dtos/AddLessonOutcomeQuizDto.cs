using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BMW_ONBOARDING_SYSTEM.Models;

namespace BMW_ONBOARDING_SYSTEM.Dtos
{
    public class AddLessonOutcomeQuizDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DueDate { get; set; }


        [Required]
        public string OutcomeId { get; set; }
        public int QuestionBankId { get; set; }
       

        public virtual List<AddQuizQuestionDto> Questions { get; set; }

        public AddLessonOutcomeQuizDto()
        {
            Questions = new List<AddQuizQuestionDto>();
        }
    }

    public class AddQuizQuestionDto
    {
        public int id { get; set; }
        public string name { get; set; }

        public virtual List<AddQuizQuestionOptionDto> options { get; set; }

        public AddQuizQuestionDto()
        {
            options = new List<AddQuizQuestionOptionDto>();

        }

    }

    public class AddQuizQuestionOptionDto
    {
        public int id { get; set; }
        public string option { get; set; }
        public bool isOptionAnswer { get; set; }
    }
}