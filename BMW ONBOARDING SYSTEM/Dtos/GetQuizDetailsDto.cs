﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BMW_ONBOARDING_SYSTEM.Dtos
{
    public class GetQuizDetailsDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int LessonId { get; set; }
        public string LessonName { get; set; }

        public int LessonOutcomeId { get; set; }
        public string LessonOutcomeName { get; set; }


        public string Name { get; set; }
        public string DueDate { get; set; }


        public List<GetQuizQuestionDto> Questions { get; set; }

        public GetQuizDetailsDto()
        {
            Questions = new List<GetQuizQuestionDto>();
        }
    }

    public class GetQuizQuestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<GetQuizQuestionAnswerOptionDto> AnswerOptions { get; set; }

        public GetQuizQuestionDto()
        {
            AnswerOptions = new List<GetQuizQuestionAnswerOptionDto>();
        }
    }
}

public class GetQuizQuestionAnswerOptionDto
{
    public int Id { get; set; }
    public string Option { get; set; }
    public bool Correct { get; set; }

}