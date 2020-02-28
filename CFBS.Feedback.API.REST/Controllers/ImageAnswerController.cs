using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.API.REST.Services.Implementations;
using CFBS.Feedback.DAL.Entities;
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
        private readonly ImageAnswerRepository _imageAnswerRepository;
        private readonly SubmittedImageAnswerRepository _submittedImageAnswerRepository;
        private readonly LocationRepository _locationRepository;

        public ImageAnswerController(IMapper mapper, AnswerRepository<ImageAnswerDetailsDTO> answerRepository, 
            ImageAnswerRepository imageAnswerRepository, SubmittedImageAnswerRepository submittedImageAnswerRepository,
            LocationRepository locationRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
            _imageAnswerRepository = imageAnswerRepository;
            _submittedImageAnswerRepository = submittedImageAnswerRepository;
            _locationRepository = locationRepository;
        }

        // GET: api/ImageAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>>> Get()
        {
            AnswerDTO<ImageAnswerDetailsDTO>[] answerDTOs = (await _answerRepository.Get(filter: answer => answer.AnswerType == AnswerType.Image)).ToArray();

            return Ok(await ImageAnswerToImageAnswerDetailsDTO(answerDTOs));
        }

        // GET: api/ImageAnswer/Submitted
        [HttpGet("Submitted")]
        public async Task<ActionResult<IEnumerable<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>>> GetSubmitted(int? locationID = null)
        {
            SubmittedAnswerDTO<ImageAnswerDetailsDTO>[] answerDTOs = (await _submittedImageAnswerRepository.Get(filter: submittedAnswer => (!locationID.HasValue || submittedAnswer.LocationID == locationID) &&
                                                                                                                                           submittedAnswer.Answer.AnswerType == AnswerType.Image)).ToArray();

            return Ok(await SubmittedImageAnswerToSubmittedImageAnswerDetailsDTO(answerDTOs));
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

            answerDTO.AnswerDetails = await _imageAnswerRepository.GetByID(id);

            return Ok(answerDTO);
        }

        // GET: api/ImageAnswer/Submitted/5
        [HttpGet("Submitted/{id}")]
        public async Task<ActionResult<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> GetSubmitted(int id)
        {
            SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO = await _submittedImageAnswerRepository.GetByID(id);

            if (submittedAnswerDTO == null)
            {
                return NotFound();
            }

            submittedAnswerDTO.Answer.AnswerDetails = await _imageAnswerRepository.GetByID(id);

            return Ok(submittedAnswerDTO);
        }

        // POST: api/ImageAnswer
        [HttpPost]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> Post(AnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            answerDTO.ID = null;
            answerDTO.AnswerType = AnswerType.Image;

            AnswerDTO<ImageAnswerDetailsDTO> answerDTOCreated = await _answerRepository.Create(answerDTO);

            if (!answerDTOCreated.ID.HasValue) throw new InvalidOperationException();

            answerDTO.AnswerDetails.AnswerID = answerDTOCreated.ID.Value;

            ImageAnswerDetailsDTO imageAnswerDetailsDTOCreated = await _imageAnswerRepository.Create(answerDTO.AnswerDetails);

            answerDTOCreated.AnswerDetails = imageAnswerDetailsDTOCreated;

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }

        /// <summary>
        /// Will add a new ImageAnswer to the database.
        /// </summary>
        /// <param name="submittedAnswerDTO"></param>
        /// <returns></returns>
        // POST: api/ImageAnswer/Submitted
        [HttpPost("Submitted")]
        public async Task<ActionResult<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO)
        {
            //Sets the DTO ID to null so the DB can set it
            submittedAnswerDTO.ID = null;

            //Checks if the DTO has a location ID
            if (!(await _locationRepository.EntityExists(submittedAnswerDTO.LocationID))) 
                throw new InvalidOperationException();
            //If the DTO has a location ID, we check if the location within it is null or has the same ID.
            else if (submittedAnswerDTO.Location == null || submittedAnswerDTO.Location.ID.GetValueOrDefault() != submittedAnswerDTO.LocationID)
                submittedAnswerDTO.Location = await _locationRepository.GetByID(submittedAnswerDTO.LocationID);


            SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTOCreated = await _submittedImageAnswerRepository.Create(submittedAnswerDTO);

            if (!submittedAnswerDTOCreated.ID.HasValue) throw new InvalidOperationException();

            submittedAnswerDTOCreated.Answer.AnswerDetails = await _imageAnswerRepository.GetByID(submittedAnswerDTOCreated.ID.Value);

            return CreatedAtAction("GetSubmitted", new { id = submittedAnswerDTOCreated.ID }, submittedAnswerDTOCreated);
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

                //TODO check if we need to call the other repo to update the details.
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
                //TODO make sure to wipe values to stop duplicates.

                await _submittedImageAnswerRepository.Update(id, submittedAnswerDTO);

                //TODO check if we need to call the other repo to update the details.
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

            //TODO check if anything needs to be done with the other repo.

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

            //TODO check if anything needs to be done with the other repo.

            return NoContent();
        }

        private async Task<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>> ImageAnswerToImageAnswerDetailsDTO(params AnswerDTO<ImageAnswerDetailsDTO>[] answerDTOs)
        {
            foreach (AnswerDTO<ImageAnswerDetailsDTO> answerDTO in answerDTOs)
            {
                if (!answerDTO.ID.HasValue) throw new InvalidOperationException();
                answerDTO.AnswerDetails = _mapper.Map<ImageAnswerDetailsDTO>(await _imageAnswerRepository.GetByID(answerDTO.ID.Value));
            }

            return answerDTOs;
        }

        private async Task<IEnumerable<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> SubmittedImageAnswerToSubmittedImageAnswerDetailsDTO(params SubmittedAnswerDTO<ImageAnswerDetailsDTO>[] submittedAnswerDTOs)
        {
            foreach (SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO in submittedAnswerDTOs)
            {
                if (!submittedAnswerDTO.ID.HasValue) throw new InvalidOperationException();
                submittedAnswerDTO.Answer.AnswerDetails = _mapper.Map<ImageAnswerDetailsDTO>(await _imageAnswerRepository.GetByID(submittedAnswerDTO.ID.Value));
            }

            return submittedAnswerDTOs;
        }
    }
}
