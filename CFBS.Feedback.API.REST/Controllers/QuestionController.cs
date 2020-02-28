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

        public QuestionController(IMapper mapper, QuestionRepository questionRepository, 
            ActiveQuestionRepository activeQuestionRepository)
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _activeQuestionRepository = activeQuestionRepository;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> Get()
        {
            return Ok(await _questionRepository.Get());
        }

        // GET: api/Image
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<ActiveQuestionDTO>>> GetActive(int? locationID = null, string locationName = null)
        {
            return Ok(await _activeQuestionRepository.Get(filter: activeQuestion => (locationID.HasValue || activeQuestion.LocationID == locationID) && 
                                                                                    (string.IsNullOrWhiteSpace(locationName) || activeQuestion.Location.Name == locationName)));
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

        // GET: api/Image/5
        [HttpGet("Active/{id}")]
        public async Task<ActionResult<ActiveQuestionDTO>> GetActive(int locationID, int questionID)
        {
            ActiveQuestionDTO activeQuestionDTO = await _activeQuestionRepository.GetByID(int.Parse($"{locationID}{questionID}"));

            if (activeQuestionDTO == null)
            {
                return NotFound();
            }

            return Ok(activeQuestionDTO);
        }

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> Post(QuestionDTO questionDTO)
        {
            questionDTO.ID = null;

            QuestionDTO questionDTOCreated = await _questionRepository.Create(questionDTO);

            return CreatedAtAction("Get", new { id = questionDTOCreated.ID }, questionDTOCreated);
        }

        // POST: api/Image
        [HttpPost("Active")]
        public async Task<ActionResult<ActiveQuestionDTO>> PostActive(ActiveQuestionDTO activeQuestionDTO)
        {
            activeQuestionDTO.Question = null;
            activeQuestionDTO.Location = null;

            ActiveQuestionDTO activeQuestionDTOCreated = await _activeQuestionRepository.Create(activeQuestionDTO);

            return CreatedAtAction("GetActive", new { id = int.Parse($"{activeQuestionDTOCreated.LocationID}{activeQuestionDTOCreated.QuestionID}") }, activeQuestionDTOCreated);
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, QuestionDTO questionDTO)
        {
            if (id != questionDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _questionRepository.Update(id, questionDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _questionRepository.EntityExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // PUT: api/Image/5
        [HttpPut("Active/{id}")]
        public async Task<IActionResult> PutActive(int locationID, int questionID, ActiveQuestionDTO activeQuestionDTO)
        {
            if (locationID != activeQuestionDTO.LocationID)
            {
                return BadRequest();
            }

            if (questionID != activeQuestionDTO.QuestionID)
            {
                return BadRequest();
            }

            try
            {
                await _activeQuestionRepository.Update(int.Parse($"{locationID}{questionID}"), activeQuestionDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _activeQuestionRepository.EntityExists(int.Parse($"{locationID}{questionID}")))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _questionRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _questionRepository.Delete(id);

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("Active/{id}")]
        public async Task<IActionResult> DeleteActive(int locationID, int questionID)
        {
            if (!await _activeQuestionRepository.EntityExists(int.Parse($"{locationID}{questionID}")))
            {
                return NotFound();
            }

            await _activeQuestionRepository.Delete(int.Parse($"{locationID}{questionID}"));

            return NoContent();
        }
    }
}
