namespace BMW_ONBOARDING_SYSTEM.Dtos
{
    public class GetOnborderCourseAchievementDto
    {
        public int Id { get; set; }
        public int OnborderId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int LessonId { get; set; }
        public string LessonName { get; set; }

        public int LessonOutcomeId { get; set; }
        public string LessonOutcomeName { get; set; }

        public int QuizId { get; set; }
        public string QuizName { get; set; }

        public string Mark { get; set; }
        public string Badge { get; set; }
        public string Date { get; set; }
    }
}