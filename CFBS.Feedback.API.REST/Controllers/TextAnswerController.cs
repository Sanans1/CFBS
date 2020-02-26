using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.API.REST.Services.Implementations;
using CFBS.Feedback.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CFBS.Feedback.API.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextAnswerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AnswerRepository<AnswerDetailsDTO> _AnswerRepository;
        private readonly SubmittedTextAnswerRepository _submittedTextAnswerRepository;

        public TextAnswerController(IMapper mapper, AnswerRepository<AnswerDetailsDTO> answerRepository,
            SubmittedTextAnswerRepository submittedTextAnswerRepository)
        {
            _mapper = mapper;
            _AnswerRepository = answerRepository;
            _submittedTextAnswerRepository = submittedTextAnswerRepository;
        }

        //TODO does this act different to the image one?
        // GET: api/TextAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<AnswerDetailsDTO>>>> Get()
        {
            return Ok(await _AnswerRepository.Get(filter: answer => answer.AnswerType == AnswerType.Text));
        }

        // GET: api/TextAnswer/Submitted
        [HttpGet("Submitted")]
        public async Task<ActionResult<IEnumerable<SubmittedAnswerDTO<AnswerDetailsDTO>>>> GetSubmitted(int? locationID = null)
        {
            return Ok(await _submittedTextAnswerRepository.Get(filter: submittedAnswer => (locationID.HasValue || submittedAnswer.LocationID == locationID) && 
                                                                                           submittedAnswer.Answer.AnswerType == AnswerType.Text));
        }

        // GET: api/TextAnswer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> Get(int id)
        {
            AnswerDTO<AnswerDetailsDTO> answerDTO = await _AnswerRepository.GetByID(id);

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // GET: api/TextAnswer/Submitted/5
        [HttpGet("Submitted/{id}")]
        public async Task<ActionResult<SubmittedAnswerDTO<AnswerDetailsDTO>>> GetSubmitted(int id)
        {
            SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTO = await _submittedTextAnswerRepository.GetByID(id);

            if (submittedAnswerDTO == null)
            {
                return NotFound();
            }

            return Ok(submittedAnswerDTO);
        }

        // POST: api/TextAnswer
        [HttpPost]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> Post(AnswerDTO<AnswerDetailsDTO> answerDTO)
        {
            AnswerDTO<AnswerDetailsDTO> answerDTOCreated = await _AnswerRepository.Create(answerDTO);

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }


        // POST: api/TextAnswer/Submitted
        [HttpPost("Submitted")]
        public async Task<ActionResult<SubmittedAnswerDTO<AnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTO)
        {
            submittedAnswerDTO = await _submittedTextAnswerRepository.Create(submittedAnswerDTO);

            return CreatedAtAction("GetSubmitted", new { id = submittedAnswerDTO.ID }, submittedAnswerDTO);
        }

        // PUT: api/TextAnswer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AnswerDTO<AnswerDetailsDTO> answerDTO)
        {
            if (id != answerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                //TODO Check for duplicate images

                await _AnswerRepository.Update(id, answerDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _AnswerRepository.EntityExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // PUT: api/TextAnswer/Submitted/5
        [HttpPut("Submitted/{id}")]
        public async Task<IActionResult> PutSubmitted(int id, SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTO)
        {
            if (id != submittedAnswerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _submittedTextAnswerRepository.Update(id, submittedAnswerDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _submittedTextAnswerRepository.EntityExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/TextAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _AnswerRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _AnswerRepository.Delete(id);

            return NoContent();
        }

        // DELETE: api/TextAnswer/Submitted/5
        [HttpDelete("Submitted/{id}")]
        public async Task<IActionResult> DeleteSubmitted(int id)
        {
            if (!await _submittedTextAnswerRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _submittedTextAnswerRepository.Delete(id);

            return NoContent();
        }
    }
}
