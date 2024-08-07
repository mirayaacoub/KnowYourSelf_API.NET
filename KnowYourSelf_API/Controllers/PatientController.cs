using KnowYourSelf_API.Data;
using KnowYourSelf_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowYourSelf_API.Controllers
{
    [ApiController]
    public class PatientController : BaseController
    {
        public PatientController(ApplicationDbContext db) : base(db) { }

        [HttpGet("{userId:int}", Name = "GetPatient")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Patient> GetPatient(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            var patient = _db.Patients
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        // endpoint to update therapist 
        [HttpPut("{userId:int}", Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Therapist> UpdatePatient(int userId, [FromBody] UpdatePatientDTO updatePatientDTO)
        {
            if (updatePatientDTO == null || userId != updatePatientDTO.UserId)
            {
                return BadRequest();
            }

            var patient = _db.Patients
                .Include(p => p.User)
                .FirstOrDefault(th => th.UserId == userId);

            if (patient == null)
            {
                return NotFound();
            }

            patient.DiagnosisHistory = updatePatientDTO.DiagnosisHistory;

            _db.Patients.Update(patient);
            _db.SaveChanges();
            return Ok(patient);
        }
    }
}
