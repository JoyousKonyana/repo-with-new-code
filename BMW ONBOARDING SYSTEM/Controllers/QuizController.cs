using System;
using System.Collections.Generic;
using System.Linq;
using BMW_ONBOARDING_SYSTEM.Dtos;
using BMW_ONBOARDING_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BMW_ONBOARDING_SYSTEM.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private readonly INF370DBContext _context;
        private static readonly Random rng = new Random();


        public QuizController(
            INF370DBContext context
            )
        {

            _context = context;
        }

        [HttpPost("Add")]
        public IActionResult AddLessonOutcomeQuiz([FromBody] AddLessonOutcomeQuizDto model)
        {
            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Something went wrong on your side.";
                return BadRequest(new { message });
            }

            var isBankInDb = _context.QuestionBank
                .FirstOrDefault(item => item.Id == model.QuestionBankId);

            if (isBankInDb == null)
            {
                message = "Question bank not found";
                return BadRequest(new { message });
            }


            var isOutcomeInDb = _context.LessonOutcome
                .Include(item => item.Lesson)
                .ThenInclude(item => item.Course)
                .FirstOrDefault(item => item.LessonOutcomeID == Convert.ToInt32(model.OutcomeId));

            if (isOutcomeInDb == null)
            {
                message = "Lesson Outcome not found";
                return BadRequest(new { message });
            }

            var courseDueDate = isOutcomeInDb.Lesson.Course.EndDate;

            if (model.DueDate > courseDueDate)
            {
                message = "Due date cannot be after: " + courseDueDate.ToString("dd/MM/yyyy");
                return BadRequest(new { message });
            }

            var newQuiz = new Quiz()
            {
                Name = model.Name,
                LessonOutcomeID = isOutcomeInDb.LessonOutcomeID,
                QuestionBankId = isBankInDb.Id,
                DueDate = model.DueDate
            };
            _context.Quizzes.Add(newQuiz);
            _context.SaveChanges();

            foreach (var question in model.Questions)
            {
                var newQuizQuestion = new QuizQuestion()
                {
                    QuizId = newQuiz.Id,
                    Name = question.name
                };

                _context.QuizQuestions.Add(newQuizQuestion);
                _context.SaveChanges();

                foreach (var option in question.options)
                {
                    var newOption = new QuizQuestionOption()
                    {
                        QuizQuestionId = newQuizQuestion.Id,
                        Option = option.option,
                        IsOptionAnswer = option.isOptionAnswer
                    };

                    _context.QuizQuestionOptions.Add(newOption);
                    _context.SaveChanges();
                }

            }

            return Ok();

        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<GetLessonOutcomeQuizDto>> GetAllLessonOutcomeQuizzes()
        {
            var quizzesInDb = _context.Quizzes
                .Include(item => item.Questions)
                .ThenInclude(item => item.Options)
                .Include(item => item.LessonOutcome)
                .Include(item => item.QuestionBank)
                .Select(item => new GetLessonOutcomeQuizDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    DueDate = item.DueDate.ToString("dd/MM/yyyy"),
                    QuestionBankId = item.QuestionBank.Id,
                    QuestionBankName = item.QuestionBank.Name,
                    LessonOutcomeId = item.LessonOutcome.LessonOutcomeID,
                    LessonOutcomeName = item.LessonOutcome.LessonOutcomeName,
                    NumberOfQuestions = item.Questions.Count()
                }).ToList();

            return quizzesInDb;
        }

        [HttpGet("GetAll/LessonOutcome/{lessonOutcomeId}")]
        public ActionResult<IEnumerable<GetLessonOutcomeQuizDto>> GetAllLessonOutcomeQuizzes(int lessonOutcomeId)
        {
            var isOutcomeInDb = _context.LessonOutcome
                .FirstOrDefault(item => item.LessonOutcomeID == lessonOutcomeId);

            if (isOutcomeInDb == null)
            {
                var message = "Lesson Outcome not found";
                return BadRequest(new { message });
            }

            var quizzesInDb = _context.Quizzes
                .Where(item => item.LessonOutcomeID == lessonOutcomeId)
                .Include(item => item.LessonOutcome)
                .ThenInclude(item=>item.Lesson)
                .ThenInclude(item=>item.Course)
                .Include(item => item.QuestionBank)
                .Select(item => new GetLessonOutcomeQuizDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    DueDate = item.DueDate.ToString("dd/MM/yyyy"),
                    QuestionBankId = item.QuestionBank.Id,
                    QuestionBankName = item.QuestionBank.Name,
                    LessonOutcomeId = item.LessonOutcome.LessonOutcomeID,
                    LessonOutcomeName = item.LessonOutcome.LessonOutcomeName,
                    NumberOfQuestions = item.Questions.Count(),

                    CourseId = item.LessonOutcome.Lesson.Course.CourseID,
                    CourseName = item.LessonOutcome.Lesson.Course.CourseName,
                    LessonId = item.LessonOutcome.Lesson.LessonID,
                    LessonName = item.LessonOutcome.Lesson.LessonName,

                }).ToList();

            return quizzesInDb;
        }



        [HttpGet("Get/{quizId}")]
        public ActionResult<GetQuizDetailsDto> GetQuizDetails(int quizId)
        {
            var quiz = _context.Quizzes
                .Where(item => item.Id == quizId)
                .Include(item => item.LessonOutcome)
                .ThenInclude(item => item.Lesson)
                .ThenInclude(item => item.Course)
                .Include(item => item.Questions)
                .ThenInclude(item => item.Options)
                .Select(item => new GetQuizDetailsDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    DueDate = item.DueDate.ToString("dd/MM/yyyy"),
                    LessonOutcomeId = item.LessonOutcome.LessonOutcomeID,
                    LessonOutcomeName = item.LessonOutcome.LessonOutcomeName,
                    LessonId = item.LessonOutcome.Lesson.LessonID,
                    LessonName = item.LessonOutcome.Lesson.LessonName,
                    CourseId = item.LessonOutcome.Lesson.Course.CourseID,
                    CourseName = item.LessonOutcome.Lesson.Course.CourseName,
                    Questions = item.Questions
                        .Select(question => new GetQuizQuestionDto
                        {
                            Id = question.Id,
                            Name = question.Name,
                            AnswerOptions = question.Options
                                .Select(answer => new GetQuizQuestionAnswerOptionDto
                                {
                                    Id = answer.Id,
                                    Correct = answer.IsOptionAnswer,
                                    Option = answer.Option
                                }).ToList()
                        }).ToList()
                }).First();

            return quiz;
        }




        [HttpPost("SubmitQuiz/{onboarderid}")]
        public ActionResult<GetQuizDetailsDto> SubmitQuiz([FromBody] SubmitQuizDto model, int onboarderid)
        {

            var message = "";
            if (!ModelState.IsValid)
            {
                message = "Something went wrong on your side.";
                return BadRequest(new { message });
            }


            var quizInDb = _context.Quizzes
                .Include(item => item.Questions)
                .ThenInclude(item => item.Options)
                .Include(item => item.LessonOutcome)
                .ThenInclude(item => item.Lesson)
                .ThenInclude(item => item.Course)
                .FirstOrDefault(item => item.Id == model.QuizId);

            if (quizInDb == null)
            {
                message = "Quiz not found";
                return BadRequest(new { message });
            }

            if (quizInDb.Questions.Count != model.QuestionsAndOptions.Count)
            {
                message = "Reload page and answer all questions";
                return BadRequest(new { message });
            }

            var quizAlreadyPassed = _context.Achievements
                .Where(item => item.OnboarderId == onboarderid)
                .FirstOrDefault(item => item.QuizId == quizInDb.Id);

            if (quizAlreadyPassed != null)
            {
                message = "Quiz already and achieved recorded on: " + quizAlreadyPassed.DateAchieved.ToString("dd/MM/yyyy hh:mm");
                return BadRequest(new { message });
            }

            var currentCount = 0;
            foreach (var question in quizInDb.Questions)
            {

                foreach (var questionQuiz in model.QuestionsAndOptions)
                {

                    if (question.Id == questionQuiz.QuestionId)
                    {
                        var correctOptions = _context.QuizQuestionOptions
                            .Where(item => item.QuizQuestionId == questionQuiz.QuestionId)
                            .ToList();

                        foreach (var optionFromDb in correctOptions)
                        {
                            if (questionQuiz.OptionId == optionFromDb.Id && optionFromDb.IsOptionAnswer)
                            {
                                currentCount += 1;
                            }

                        }

                    }
                }
            }

            var onBorderPercentage = Convert.ToDecimal(currentCount) / Convert.ToDecimal(quizInDb.Questions.Count) * 100;

            if (onBorderPercentage < 50)
            {
                message = "You failed. Your score: " + onBorderPercentage + "% is lower than 50%";
                return BadRequest(new { message });
            }
            if (onBorderPercentage >= 50)
            {
                var type = _context.AchievementTypes
                    .FirstOrDefault(item => onBorderPercentage >= Convert.ToDecimal(item.MinMark) && onBorderPercentage <= Convert.ToDecimal(item.MaxMark));

                if (type ==null)
                {
                    message = "Badge not found";
                    return BadRequest(new { message });
                }

                var newAchievement = new Achievement()
                {
                    AchievementTypeId = type.Id,
                    OnboarderId = onboarderid,
                    CourseId = quizInDb.LessonOutcome.Lesson.Course.CourseID,
                    MarkAchieved = Convert.ToDouble(onBorderPercentage),
                    DateAchieved = DateTime.Now,
                    QuizId = quizInDb.Id,
                };
                _context.Achievements.Add(newAchievement);
                _context.SaveChanges();
            }

            return Ok();

        }

    }
}
