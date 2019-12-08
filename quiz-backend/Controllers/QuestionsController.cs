using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quiz_backend.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_backend.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizContext context;

        public QuestionsController(QuizContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Question> GetQuestions()
        {
            return context.Questions;
        }

        [HttpGet("{quizId}")]
        public IEnumerable<Question> GetQuestions([FromRoute] int quizId)
        {
            return context.Questions.Where(q => q.QuizId == quizId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Question question)
        {
            var quiz = context.Quiz.SingleOrDefault(q => q.Id == question.QuizId);
            if (quiz == null)
                return NotFound();
            context.Questions.Add(question);
            await context.SaveChangesAsync();
            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Question question)
        {
            if (id != question.Id)
                return BadRequest();

            context.Entry(question).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(question);
        }
    }
}