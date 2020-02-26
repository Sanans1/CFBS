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
    public class ImageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ImageRepository _imageRepository;

        public ImageController(IMapper mapper, ImageRepository imageRepository)
        {
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> Get()
        {
            return Ok(await _imageRepository.Get());
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> Get(int id)
        {
            ImageDTO imageDTO = await _imageRepository.GetByID(id);

            if (imageDTO == null)
            {
                return NotFound();
            }

            return Ok(imageDTO);
        }

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<ImageDTO>> Post(ImageDTO imageDTO)
        {
            ImageDTO imageDTOCreated = await _imageRepository.Create(imageDTO);

            return CreatedAtAction("Get", new { id = imageDTOCreated.ID }, imageDTOCreated);
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ImageDTO imageDTO)
        {
            if (id != imageDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _imageRepository.Update(id, imageDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _imageRepository.EntityExists(id))
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
            if (!await _imageRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _imageRepository.Delete(id);

            return NoContent();
        }
    }
}
