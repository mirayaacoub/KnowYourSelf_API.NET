using KnowYourSelf_API.Data;
using KnowYourSelf_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowYourSelf_API.Controllers
{
    [ApiController]
    public class TherapistController : BaseController
    {
        public TherapistController(ApplicationDbContext db) : base(db) { }

        // endpoint to get all therapists
        [HttpGet(Name = "GetAllTherapists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Therapist>> GetAllTherapists()
        {
            var therapists = _db.Therapists
            .Include(t => t.User)
            .ToList();

            if (therapists == null)
            {
                return NotFound();
            }


            return Ok(therapists);
        }


        [HttpGet("{userId:int}", Name = "GetTherapist")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Therapist> GetTherapist(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            var therapist = _db.Therapists
                 .Include(t => t.User)
                 .FirstOrDefault(th => th.UserId == userId);

            if (therapist == null)
            {
                return NotFound();
            }
            return Ok(therapist);
        }

        // endpoint to update therapist 
        [HttpPut("{userId:int}", Name = "UpdateTherapist")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Therapist> UpdateTherapist(int userId, [FromBody] UpdateTherapistDTO updateTherapistDTO)
        {
            if (updateTherapistDTO == null || userId != updateTherapistDTO.UserId)
            {
                return BadRequest();
            }

            var therapist = _db.Therapists.FirstOrDefault(th => th.UserId == userId);
            if (therapist == null)
            {
                return NotFound();
            }

            therapist.ExperienceYears = updateTherapistDTO.ExperienceYears;
            therapist.Specialty = updateTherapistDTO.Specialty;

            _db.Therapists.Update(therapist);
            _db.SaveChanges();
            return Ok(therapist);
        }
    }
}
