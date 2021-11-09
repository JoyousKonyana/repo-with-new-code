using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMW_ONBOARDING_SYSTEM.Models
{
    public class AchievementType
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int MinMark { get; set; }
        public int MaxMark { get; set; }

        public virtual List<Achievement> Achievements { get; set; }

        public AchievementType()
        {
            Achievements = new List<Achievement>();
        }
    }
}
