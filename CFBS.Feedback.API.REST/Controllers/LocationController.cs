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
    public class LocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LocationRepository _locationRepository;

        public LocationController(IMapper mapper, LocationRepository locationRepository)
        {
            _mapper = mapper;
            _locationRepository = locationRepository;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> Get()
        {
            return Ok(await _locationRepository.Get());
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> Get(int id)
        {
            LocationDTO location = await _locationRepository.GetByID(id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<LocationDTO>> Post(LocationDTO locationDTO)
        {
            locationDTO.ID = null;

            LocationDTO locationCreated = await _locationRepository.Create(locationDTO);

            return CreatedAtAction("Get", new { id = locationCreated.ID }, locationCreated);
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, LocationDTO locationDTO)
        {
            if (id != locationDTO.ID)
            {
                return BadRequest();
            }

            try
            {
                await _locationRepository.Update(id, locationDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _locationRepository.EntityExists(id))
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
            if (!await _locationRepository.EntityExists(id))
            {
                return NotFound();
            }

            await _locationRepository.Delete(id);

            return NoContent();
        }
    }
}
