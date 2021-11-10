using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMW_ONBOARDING_SYSTEM.Dtos;
using BMW_ONBOARDING_SYSTEM.Models;
using Microsoft.EntityFrameworkCore;

namespace BMW_ONBOARDING_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private readonly INF370DBContext _context;

        public AchievementsController(INF370DBContext context)
        {
            _context = context;
        }

        [HttpGet("Types/GetAll")]
        public ActionResult<IEnumerable<GetAchievementTypeDto>> GetAchievementTypes()
        {
            var typesInDb = _context.AchievementTypes
                .OrderBy(item => item.MinMark)
                .Select(item => new GetAchievementTypeDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    MinMark = item.MinMark,
                    MaxMark = item.MaxMark
                }).ToList();

            return typesInDb;
        }

        [HttpPost("Types/Add")]
        public IActionResult AddAchievementType([FromBody] AddAchievementTypeDto model)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Something went wrong on your side.";
                return BadRequest(new { message });
            }

            var isTypeInDb = _context.AchievementTypes
                .FirstOrDefault(item => item.Name.ToLower() == model.Name.ToLower());

            if (isTypeInDb != null)
            {
                message = "Achievement Type already exists.";
                return BadRequest(new { message });
            }

            var newType = new AchievementType()
            {
                Name = model.Name,
                MinMark = model.MinMark,
                MaxMark = model.MaxMark
            };
            _context.AchievementTypes.Add(newType);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("Onborder/{onBorderId}/{courseId}")]
        public ActionResult<IEnumerable<GetOnborderCourseAchievementDto>> GetOnborderAchievementsByCourseId(int onBorderId, int courseId)
        {
            var message = "";

            var isOnBorderInDb = _context.Onboarder.FirstOrDefault(item => item.OnboarderID == onBorderId);
            if (isOnBorderInDb == null)
            {
                message = "Onborder not found";
                return BadRequest(new { message });
            }

            var isCourseInDb = _context.Course.FirstOrDefault(item => item.CourseID == courseId);
            if (isCourseInDb == null)
            {
                message = "Course not found";
                return BadRequest(new { message });
            }

            var onBorderAchievementsInDb = _context.Achievements
                .Where(item => item.OnboarderId == isOnBorderInDb.OnboarderID)
                .Where(item => item.CourseId == isCourseInDb.CourseID)
                .Include(item => item.AchievementType)
                .Include(item => item.Course)
                .Include(item => item.Quiz)
                .ThenInclude(item => item.LessonOutcome)
                .ThenInclude(item => item.Lesson)
                .ThenInclude(item => item.Course)
                .OrderBy(item => item.DateAchieved)
                .Select(item => new GetOnborderCourseAchievementDto
                {
                    Id = item.Id,
                    Badge = item.AchievementType.Name,
                    Mark = item.MarkAchieved.ToString("0.00"),
                    Date = item.DateAchieved.ToString("dd/MM/yyyy"),
                    
                    QuizId = item.Quiz.Id,
                    QuizName = item.Quiz.Name,

                    LessonOutcomeId = item.Quiz.LessonOutcome.LessonOutcomeID,
                    LessonOutcomeName = item.Quiz.LessonOutcome.LessonOutcomeName,

                    LessonId = item.Quiz.LessonOutcome.Lesson.LessonID,
                    LessonName = item.Quiz.LessonOutcome.Lesson.LessonName,

                    CourseId = item.Course.CourseID,
                    CourseName = item.Course.CourseName,

                    OnborderId = item.OnboarderId
                }).ToList();

            return onBorderAchievementsInDb;
        }
    }
}
