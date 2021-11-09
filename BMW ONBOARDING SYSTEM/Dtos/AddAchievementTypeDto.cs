using System.ComponentModel.DataAnnotations;

namespace BMW_ONBOARDING_SYSTEM.Dtos
{
    public class AddAchievementTypeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int MinMark { get; set; }
        [Required]
        public int MaxMark { get; set; }
    }
}