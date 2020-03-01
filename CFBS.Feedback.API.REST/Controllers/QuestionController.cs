using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.API.REST.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CFBS.Feedback.API.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QuestionRepository _questionRepository;
        private readonly ActiveQuestionRepository _activeQuestionRepository;
        private readonly ImageAnswerRepository _imageAnswerRepository;

        public QuestionController(IMapper mapper, QuestionRepository questionRepository, 
            ActiveQuestionRepository activeQuestionRepository, ImageAnswerRepository imageAnswerRepository)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _activeQuestionRepository = activeQuestionRepository;
            _imageAnswerRepository = imageAnswerRepository;
        }

        #region Questions

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> Get()
        {
            return Ok(await _questionRepository.Get());
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> Get(int id)
        {
            QuestionDTO questionDTO = await _questionRepository.GetByID(id);

            if (questionDTO == null)
            {
                return NotFound();
            }

            return Ok(questionDTO);
        }

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> Post(QuestionDTO questionDTO)
        {
            questionDTO.ID = null;

            QuestionDTO questionDTOCreated = await _questionRepository.Create(questionDTO);

            return CreatedAtAction("Get", new { id = questionDTOCreated.ID }, questionDTOCreated);
        }

        #endregion

        #region ActiveQuestions

        // GET: api/Image
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<ActiveQuestionDTO>>> GetActive(int? locationID = null)
        {
            return Ok(await _activeQuestionRepository.Get(filter: activeQuestion => !locationID.HasValue || activeQuestion.LocationID == locationID));
        }

        // GET: api/Image/5
        [HttpGet("Active/{id}")]
        public async Task<ActionResult<ActiveQuestionDTO>> GetActive(int id)
        {
            ActiveQuestionDTO activeQuestionDTO = await _activeQuestionRepository.GetByID(id);

            if (activeQuestionDTO == null)
            {
                return NotFound();
            }

            return Ok(activeQuestionDTO);
        }

        // POST: api/Image
        [HttpPost("Active")]
        public async Task<ActionResult<ActiveQuestionDTO>> PostActive(ActiveQuestionDTO activeQuestionDTO)
        {
            activeQuestionDTO.ID = null;
            activeQuestionDTO.Question = null;
            activeQuestionDTO.Location = null;

            ActiveQuestionDTO activeQuestionDTOCreated = await _activeQuestionRepository.Create(activeQuestionDTO);

            return CreatedAtAction("GetActive", new { id = activeQuestionDTOCreated.ID }, activeQuestionDTOCreated);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("Active/{id}")]
        public async Task<IActionResult> DeleteActive(int id)
        {
            if (!await _activeQuestionRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _activeQuestionRepository.Delete(id);

            return NoContent();
        }

        #endregion
    }
}
