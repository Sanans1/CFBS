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

        public ImageAnswerController(IMapper mapper, AnswerRepository<ImageAnswerDetailsDTO> answerRepository, 
            ImageAnswerRepository imageAnswerRepository, SubmittedImageAnswerRepository submittedImageAnswerRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
            _imageAnswerRepository = imageAnswerRepository;
            _submittedImageAnswerRepository = submittedImageAnswerRepository;
        }

        #region ImageAnswers

        // GET: api/ImageAnswer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO<ImageAnswerDetailsDTO>>>> Get(int? questionID = null, bool? isActive = true)
        {
            AnswerDTO<ImageAnswerDetailsDTO>[] answerDTOs = (await _answerRepository.Get(filter: answer => answer.AnswerType == AnswerType.Image && 
                                                                                                           (!questionID.HasValue || questionID.Value == answer.QuestionID)))
                                                                                                           .ToArray();

            foreach (AnswerDTO<ImageAnswerDetailsDTO> answerDTO in answerDTOs)
            {
                if (!answerDTO.ID.HasValue) throw new InvalidOperationException();
                answerDTO.AnswerDetails = _mapper.Map<ImageAnswerDetailsDTO>(await _imageAnswerRepository.GetByID(answerDTO.ID.Value));
            }

            return Ok(answerDTOs.Where(answerDTO => !isActive.HasValue || isActive == answerDTO.AnswerDetails.IsActive));
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

        /// <summary>
        /// Will add a image answer to the database.
        /// </summary>
        /// <param name="answerDTO"></param>
        /// <returns></returns>
        // POST: api/ImageAnswer
        [HttpPost]
        public async Task<ActionResult<AnswerDTO<ImageAnswerDetailsDTO>>> Post(AnswerDTO<ImageAnswerDetailsDTO> answerDTO)
        {
            //Remove the ID so the DB sets an ID.
            answerDTO.ID = null;
            answerDTO.AnswerType = AnswerType.Image;
            answerDTO.CreatedAt = DateTime.Now;
            //Remove the ImageDTO so we don't get any duplicates.
            answerDTO.AnswerDetails.Image = null;
            //Set this to true so we know the answer is active.
            answerDTO.AnswerDetails.IsActive = true;

            AnswerDTO<ImageAnswerDetailsDTO> answerDTOCreated = await _answerRepository.Create(answerDTO);

            if (!answerDTOCreated.ID.HasValue) throw new InvalidOperationException();

            //Make sure we have the right AnswerID.
            answerDTO.AnswerDetails.AnswerID = answerDTOCreated.ID.Value;

            answerDTOCreated.AnswerDetails = await _imageAnswerRepository.Create(answerDTO.AnswerDetails);

            return CreatedAtAction("Get", new { id = answerDTOCreated.ID }, answerDTOCreated);
        }

        // DELETE: api/ImageAnswer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            AnswerDTO<ImageAnswerDetailsDTO> answerDTO = await _answerRepository.GetByID(id);

            answerDTO.AnswerDetails.IsActive = false;

            try
            {
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

        #endregion

        #region SubmittedImageAnswers

        // GET: api/ImageAnswer/Submitted
        [HttpGet("Submitted")]
        public async Task<ActionResult<IEnumerable<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>>> GetSubmitted(int? locationID = null, int? questionID = null, bool? isActive = null)
        {
            SubmittedAnswerDTO<ImageAnswerDetailsDTO>[] submittedAnswerDTOs = (await _submittedImageAnswerRepository.Get(filter: submittedAnswer => submittedAnswer.Answer.AnswerType == AnswerType.Image && 
                                                                                                                                                    (!locationID.HasValue || submittedAnswer.LocationID == locationID) &&
                                                                                                                                                    (!questionID.HasValue || submittedAnswer.Answer.QuestionID == questionID.Value))).ToArray();

            foreach (SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO in submittedAnswerDTOs)
            {
                if (!submittedAnswerDTO.ID.HasValue) throw new InvalidOperationException();
                submittedAnswerDTO.Answer.AnswerDetails = _mapper.Map<ImageAnswerDetailsDTO>(await _imageAnswerRepository.GetByID(submittedAnswerDTO.AnswerID));
            }

            return Ok(submittedAnswerDTOs.Where(x => !isActive.HasValue || isActive == x.Answer.AnswerDetails.IsActive));
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

        /// <summary>
        /// Will add a user picked answer to the database.
        /// </summary>
        /// <param name="submittedAnswerDTO"></param>
        /// <returns></returns>
        // POST: api/ImageAnswer/Submitted
        [HttpPost("Submitted")]
        public async Task<ActionResult<SubmittedAnswerDTO<ImageAnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTO)
        {
            //Sets the DTO ID to null so the DB can set it
            submittedAnswerDTO.ID = null;
            submittedAnswerDTO.Location = null;
            submittedAnswerDTO.Answer = null;
            submittedAnswerDTO.CreatedAt = DateTime.Now;

            SubmittedAnswerDTO<ImageAnswerDetailsDTO> submittedAnswerDTOCreated = await _submittedImageAnswerRepository.Create(submittedAnswerDTO);

            if (!submittedAnswerDTOCreated.ID.HasValue) throw new InvalidOperationException();

            submittedAnswerDTOCreated.Answer.AnswerDetails = await _imageAnswerRepository.GetByID(submittedAnswerDTOCreated.ID.Value);

            return CreatedAtAction("GetSubmitted", new { id = submittedAnswerDTOCreated.ID }, submittedAnswerDTOCreated);
        }

        #endregion
    }
}
