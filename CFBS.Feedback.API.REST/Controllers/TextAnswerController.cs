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
        private readonly AnswerRepository<AnswerDetailsDTO> _answerRepository;
        private readonly TextAnswerRepository _textAnswerRepository;
        private readonly SubmittedTextAnswerRepository _submittedTextAnswerRepository;

        public TextAnswerController(IMapper mapper, AnswerRepository<AnswerDetailsDTO> answerRepository,
            TextAnswerRepository textAnswerRepository, SubmittedTextAnswerRepository submittedTextAnswerRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
            _textAnswerRepository = textAnswerRepository;
            _submittedTextAnswerRepository = submittedTextAnswerRepository;
        }

        #region SubmittedTextAnswers

        // GET: api/TextAnswer/Submitted
        [HttpGet("Submitted")]
        public async Task<ActionResult<IEnumerable<SubmittedAnswerDTO<AnswerDetailsDTO>>>> GetSubmitted(int? locationID = null, int? questionID = null, bool? isActive = null)
        {
            SubmittedAnswerDTO<AnswerDetailsDTO>[] submittedAnswerDTOs = (await _submittedTextAnswerRepository.Get(filter: submittedAnswer => submittedAnswer.Answer.AnswerType == AnswerType.Text &&
                                                                                                                                              (!locationID.HasValue || submittedAnswer.LocationID == locationID) &&
                                                                                                                                              (!questionID.HasValue || submittedAnswer.Answer.QuestionID == questionID.Value))).ToArray();

            foreach (SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTO in submittedAnswerDTOs)
            {
                if (!submittedAnswerDTO.ID.HasValue) throw new InvalidOperationException();
                submittedAnswerDTO.Answer.AnswerDetails = _mapper.Map<AnswerDetailsDTO>(await _textAnswerRepository.GetByID(submittedAnswerDTO.ID.Value));
            }

            return Ok(submittedAnswerDTOs);
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

            submittedAnswerDTO.Answer.AnswerDetails = await _textAnswerRepository.GetByID(id);

            return Ok(submittedAnswerDTO);
        }

        /// <summary>
        /// Can be used to submit text answers.
        /// </summary>
        /// <param name="submittedAnswerDTO"></param>
        /// <returns></returns>
        // POST: api/TextAnswer/Submitted
        [HttpPost("Submitted")]
        public async Task<ActionResult<SubmittedAnswerDTO<AnswerDetailsDTO>>> PostSubmitted(SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTO)
        {
            submittedAnswerDTO.ID = null;
            submittedAnswerDTO.Location = null;
            submittedAnswerDTO.CreatedAt = DateTime.Now;

            SubmittedAnswerDTO<AnswerDetailsDTO> submittedAnswerDTOCreated = await _submittedTextAnswerRepository.Create(submittedAnswerDTO);

            if (!submittedAnswerDTOCreated.ID.HasValue) throw new InvalidOperationException();

            submittedAnswerDTOCreated.Answer.AnswerDetails.AnswerID = submittedAnswerDTOCreated.AnswerID;

            submittedAnswerDTOCreated.Answer.AnswerDetails = await _textAnswerRepository.Create(submittedAnswerDTOCreated.Answer.AnswerDetails);

            return CreatedAtAction("GetSubmitted", new { id = submittedAnswerDTO.ID }, submittedAnswerDTO);
        }

        #endregion
    }
}
