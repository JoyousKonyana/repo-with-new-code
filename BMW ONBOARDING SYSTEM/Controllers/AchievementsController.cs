using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMW_ONBOARDING_SYSTEM.Dtos;
using BMW_ONBOARDING_SYSTEM.Models;

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


    }
}
