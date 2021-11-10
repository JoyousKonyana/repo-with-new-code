using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class Achievement
    {

        public int Id { get; set; }
        public double MarkAchieved { get; set; }
        public DateTime DateAchieved { get; set; }

        public virtual Onboarder Onboarder { get; set; }
        public int OnboarderId { get; set; }
     
        public virtual Quiz Quiz { get; set; }
        public int QuizId { get; set; }

        public virtual AchievementType AchievementType { get; set; }
        public int AchievementTypeId { get; set; }

        public virtual Course Course { get; set; }
        public int CourseId { get; set; }

    }
}
