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
    public class ImageAnswerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AnswerRepository<ImageAnswerDetailsDTO> _answerRepository;
        private readonly SubmittedImageAnswerRepository _submittedImageAnswerRepository;

        public ImageAnswerController(IMapper mapper, AnswerRepository<ImageAnswerDetailsDTO> answerRepository, 
            SubmittedImageAnswerRepository submittedImageAnswerRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
            _submittedImageAnswerRepository = submittedImageAnswerRepository;
        }

        // GET: api/ImageAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>>> Get()
        {
            return Ok(await _answerRepository.Get(filter: answer => answer.AnswerType == AnswerType.Image));
        }

        // GET: api/ImageAnswer/Submitted
        [HttpGet("Submitted")]
        public async Task<ActionResult<IEnumerable<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>>> GetSubmitted(int? locationID = null)
        {
            return Ok(await _submittedImageAnswerRepository.Get(filter: submittedAnswer => (locationID.HasValue || submittedAnswer.LocationID == locationID) && 
                                                                                            submittedAnswer.Answer.AnswerType == AnswerType.Image));
        }

        // GET: api/ImageAnswer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> Get(int id)
        {
            AnswerDTO<ImageAnswerDetailsDTO> answerDTO = await _answerRepository.GetByID(id);

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // GET: api/ImageAnswer/Submitted/5
        [HttpGet("Submitted/{id}")]
        public async Task<ActionResult<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> GetSubmitted(int id)
        {
            SubmittedAnswerDTO<ImageAnswerDetailsDTO> answerDTO = await _submittedImageAnswerRepository.GetByID(id);

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // POST: api/ImageAnswer
        [HttpPost]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> Post(AnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            AnswerDTO<ImageAnswerDetailsDTO> answerDTOCreated = await _answerRepository.Create(answerDTO);

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }


        // POST: api/ImageAnswer/Submitted
        [HttpPost("Submitted")]
        public async Task<ActionResult<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO)
        {
            submittedAnswerDTO = await _submittedImageAnswerRepository.Create(submittedAnswerDTO);

            return CreatedAtAction("GetSubmitted", new { id = submittedAnswerDTO.ID }, submittedAnswerDTO);
        }

        // PUT: api/ImageAnswer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            if (id != answerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                //TODO Check for duplicate images
                //TODO perhaps make it so this can only have an image ID

                await _answerRepository.Update(id, answerDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _answerRepository.EntityExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // PUT: api/ImageAnswer/Submitted/5
        [HttpPut("Submitted/{id}")]
        public async Task<IActionResult> PutSubmitted(int id, SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO)
        {
            if (id != submittedAnswerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _submittedImageAnswerRepository.Update(id, submittedAnswerDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _submittedImageAnswerRepository.EntityExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ImageAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _answerRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _answerRepository.Delete(id);

            return NoContent();
        }

        // DELETE: api/ImageAnswer/Submitted/5
        [HttpDelete("Submitted/{id}")]
        public async Task<IActionResult> DeleteSubmitted(int id)
        {
            if (!await _submittedImageAnswerRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _submittedImageAnswerRepository.Delete(id);

            return NoContent();
        }
    }
}
