﻿using AutoMapper;
using BMW_ONBOARDING_SYSTEM.Helpers;
using BMW_ONBOARDING_SYSTEM.Interfaces;
using BMW_ONBOARDING_SYSTEM.Models;
using BMW_ONBOARDING_SYSTEM.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMW_ONBOARDING_SYSTEM.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BMW_ONBOARDING_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        private readonly INF370DBContext _context;

        // functionality not implemented yet
        // create a quiz together with a question
        public CourseController(ICourseRepository courseRepository, IMapper mapper, INF370DBContext context)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _context = context;
        }
        //[Authorize(Roles = Role.Admin)]
        [Route("[action]")]
        [HttpGet("name")]
        public async Task<ActionResult<CourseViewModel>> GetCourseByName([FromBody] string name)
        {
            try
            {
                var result = await _courseRepository.GetCourseByNameAsync(name);

                if (result == null) return NotFound();

                return _mapper.Map<CourseViewModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
        //[Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("[action]/{userid}")]
        public async Task<ActionResult<CourseViewModel>> CreateCourse(int userid, [FromBody] CourseViewModel model)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Something went wrong";
                return BadRequest(new { message });
            }

            var isCourseInDb = _context.Course
                .FirstOrDefault(item => item.CourseName.ToLower() == model.CourseName);

            if (isCourseInDb != null)
            {
                message = "Course name already exists";
                return BadRequest(new { message });
            }

            var newCourse = new Course()
            {
                CourseName = model.CourseName,
                CourseDescription = model.CourseDescription,
                EndDate = model.CourseDueDate
            };

            _context.Course.Add(newCourse);
            await _context.SaveChangesAsync();

            var auditLog = new AuditLog
            {
                AuditLogDescription = "Created Course with name" + ' ' + model.CourseName,
                AuditLogDatestamp = DateTime.Now,
                UserId = userid
            };

            _context.AuditLog.Add(auditLog);
            await _context.SaveChangesAsync();

            return Ok();

        }
        //[Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("[action]/{userid}")]
        public async Task<ActionResult<OnboarderCourseEnrollmentViewModel>> AssignCourse(int userid, [FromBody] OnboarderCourseEnrollmentViewModel model)
        {
            try
            {

                //foreach (OnboarderCourseEnrollmentViewModel enroll in model)
                //{


                var enrollment = _mapper.Map<OnboarderCourseEnrollment>(model);
                _courseRepository.Add(enrollment);

                if (!await _courseRepository.SaveChangesAsync())
                {
                    return BadRequest("We could not success fully save all enrollments");
                }
                //}

                //    if (await _courseRepository.SaveChangesAsync())
                //{
                //    return Ok("Onboarder Course Enrollment successfull");
                //}

                AuditLog auditLog = new AuditLog();
                auditLog.AuditLogDescription = "Assigned Course to onboarder with id " + ' ' + model.OnboarderId;
                auditLog.AuditLogDatestamp = DateTime.Now;
                auditLog.UserId = userid;

                return Ok("Onboarder Course Enrollment successfull");
            }
            catch (Exception)
            {

                BadRequest();
            }
            return BadRequest();
        }

        //[Authorize(Roles = Role.Onboarder)]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<GetCourseDto>>> GetAllCourses()
        {
            try
            {
                var courses = await _context.Course
                    .Select(item => new GetCourseDto
                    {
                        CourseID = item.CourseID,
                        CourseName = item.CourseName,
                        CourseDescription = item.CourseDescription,
                        CourseDueDate = item.EndDate.ToString("dd/MM/yyyy")
                    }).ToListAsync();
                return courses;
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await _courseRepository.GetCourseByIdAsync(id);
                return Ok(course);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("Get/{courseId}")]
        public ActionResult<GetCourseDetailsDto> GetCourseDetails(int courseId)
        {
            var courseInDb = _context.Course
                    .FirstOrDefault(item => item.CourseID == courseId)
                ;
            if (courseInDb == null)
            {
                var message = "Course not found";
                return BadRequest(new { message });
            }

            var courseDetails = _context.Course
                .Where(item => item.CourseID == courseId)
                .Select(item => new GetCourseDetailsDto
                {
                    Id = item.CourseID,
                    Name = item.CourseName
                }).First();


            return courseDetails;

        }

        //[Authorize(Roles = Role.Admin)]
        [HttpPut("{id}")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<CourseViewModel>> UpdateCourse(int id, CourseViewModel updatedCourseModel)
        {
            try
            {
                var existingCourse = await _courseRepository.GetCourseByIdAsync(id);

                if (existingCourse == null) return NotFound($"Could Not find course ");

                _mapper.Map(updatedCourseModel, existingCourse);

                if (await _courseRepository.SaveChangesAsync())
                {
                    AuditLog auditLog = new AuditLog();
                    auditLog.AuditLogDescription = "Updated Course to " + ' ' + updatedCourseModel.CourseName;
                    auditLog.AuditLogDatestamp = DateTime.Now;
                    auditLog.UserId = id;
                    return _mapper.Map<CourseViewModel>(existingCourse);
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        //[Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        [Route("[action]/{id}/{userid}")]
        public async Task<IActionResult> DeleteCourse(int id, int userid)
        {
            try
            {
                var existingCourse = await _courseRepository.GetCourseByIdAsync(id);

                if (existingCourse == null) return NotFound();

                _courseRepository.Delete(existingCourse);

                if (await _courseRepository.SaveChangesAsync())
                {

                    AuditLog auditLog = new AuditLog();
                    auditLog.AuditLogDescription = "Updated Course to " + ' ' + existingCourse.CourseName;
                    auditLog.AuditLogDatestamp = DateTime.Now;
                    auditLog.UserId = userid;
                    return Ok();
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, $"We could not delete the course");
            }

            return BadRequest();
        }
    }
}
