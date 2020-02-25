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
    public class TextAnswerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AnswerRepository<AnswerDetailsDTO> _AnswerRepository;
        private readonly TextAnswerRepository _textAnswerRepository;
        private readonly SubmittedTextAnswerRepository _submittedTextAnswerRepository;

        public TextAnswerController(IMapper mapper, AnswerRepository<AnswerDetailsDTO> answerRepository,
            TextAnswerRepository textAnswerRepository, SubmittedTextAnswerRepository submittedTextAnswerRepository)
        {
            _mapper = mapper;
            _AnswerRepository = answerRepository;
            _textAnswerRepository = textAnswerRepository;
            _submittedTextAnswerRepository = submittedTextAnswerRepository;
        }

        //TODO does this act different to the image one?
        // GET: api/TextAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<AnswerDetailsDTO>>>> Get()
        {
            return Ok(await _AnswerRepository.Get());
        }

        // GET: api/submitted/TextAnswer
        [HttpGet]
        [Route("Submitted")]
        public async Task<ActionResult<IEnumerable<AnswerDTO<AnswerDetailsDTO>>>> GetSubmitted(int? locationID = null)
        {
            return Ok((await _submittedTextAnswerRepository.Get(filter: submittedAnswer => locationID.HasValue || submittedAnswer.LocationID == locationID)).Select(submittedAnswer => submittedAnswer.Answer));
        }

        // GET: api/TextAnswer/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> Get(int id)
        {
            AnswerDTO<AnswerDetailsDTO> answerDTO = await _AnswerRepository.GetByID(id);

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // GET: api/submitted/TextAnswer/5
        [HttpGet("{id}", Name = "Get")]
        [Route("Submitted")]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> GetSubmitted(int id)
        {
            AnswerDTO<AnswerDetailsDTO> answerDTO = (await _submittedTextAnswerRepository.GetByID(id)).Answer;

            if (answerDTO == null)
            {
                return NotFound();
            }

            return Ok(answerDTO);
        }

        // POST: api/TextAnswer
        [HttpPost]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> Post(AnswerDTO<AnswerDetailsDTO> answerDTO)
        {
            AnswerDTO<AnswerDetailsDTO> answerDTOCreated = await _AnswerRepository.Create(answerDTO);

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }


        // POST: api/TextAnswer
        [HttpPost]
        [Route("Submitted")]
        public async Task<ActionResult<AnswerDTO<AnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<AnswerDetailsDTO> answerDTO)
        {
            answerDTO = await _submittedTextAnswerRepository.Create(answerDTO);

            return CreatedAtAction("GetSubmitted", new { id = answerDTO.ID }, answerDTO);
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
                AnswerDTO<AnswerDetailsDTO> oldAnswerDTO = await _AnswerRepository.GetByID(id);

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

        // PUT: api/TextAnswer/5
        [HttpPut("{id}")]
        [Route("Submitted")]
        public async Task<IActionResult> PutSubmitted(int id, SubmittedAnswerDTO<AnswerDetailsDTO> answerDTO)
        {
            if (id != answerDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _submittedTextAnswerRepository.Update(id, answerDTO);
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

        // DELETE: api/TextAnswer/5
        [HttpDelete("{id}")]
        [Route("Submitted")]
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
