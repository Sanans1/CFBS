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
    public class ImageAnswerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AnswerRepository<ImageAnswerDetailsDTO> _AnswerRepository;
        private readonly ImageAnswerRepository _imageAnswerRepository;
        private readonly SubmittedImageAnswerRepository _submittedImageAnswerRepository;

        public ImageAnswerController(IMapper mapper, AnswerRepository<ImageAnswerDetailsDTO> answerRepository, 
            ImageAnswerRepository imageAnswerRepository, SubmittedImageAnswerRepository submittedImageAnswerRepository)
        {
            _mapper = mapper;
            _AnswerRepository = answerRepository;
            _imageAnswerRepository = imageAnswerRepository;
            _submittedImageAnswerRepository = submittedImageAnswerRepository;
        }

        // GET: api/ImageAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>>> Get()
        {
            return Ok(await _AnswerRepository.Get());
        }

        // GET: api/submitted/ImageAnswer
        [HttpGet]
        [Route("Submitted")]
        public async Task<ActionResult<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>>> GetSubmitted(int? locationID = null)
        {
            return Ok((await _submittedImageAnswerRepository.Get(filter: submittedAnswer => locationID.HasValue || submittedAnswer.LocationID == locationID)).Select(submittedAnswer => submittedAnswer.Answer));
        }

        // GET: api/ImageAnswer/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> Get(int id)
        {
            AnswerDTO<ImageAnswerDetailsDTO> answerDTO = await _AnswerRepository.GetByID(id);

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // GET: api/submitted/ImageAnswer/5
        [HttpGet("{id}", Name = "Get")]
        [Route("Submitted")]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> GetSubmitted(int id)
        {
            AnswerDTO<ImageAnswerDetailsDTO> answerDTO = (await _submittedImageAnswerRepository.GetByID(id)).Answer;

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
            AnswerDTO<ImageAnswerDetailsDTO> answerDTOCreated = await _AnswerRepository.Create(answerDTO);

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }


        // POST: api/ImageAnswer
        [HttpPost]
        [Route("Submitted")]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            answerDTO = await _submittedImageAnswerRepository.Create(answerDTO);

            return CreatedAtAction("GetSubmitted", new { id = answerDTO.ID }, answerDTO);
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

        // PUT: api/ImageAnswer/5
        [HttpPut("{id}")]
        [Route("Submitted")]
        public async Task<IActionResult> PutSubmitted(int id, SubmittedAnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            if (id != answerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                SubmittedAnswerDTO<ImageAnswerDetailsDTO> oldAnswerDTO = await _submittedImageAnswerRepository.GetByID(id);

                await _submittedImageAnswerRepository.Update(id, answerDTO);
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
            if (!await _AnswerRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _AnswerRepository.Delete(id);

            return NoContent();
        }

        // DELETE: api/ImageAnswer/5
        [HttpDelete("{id}")]
        [Route("Submitted")]
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
