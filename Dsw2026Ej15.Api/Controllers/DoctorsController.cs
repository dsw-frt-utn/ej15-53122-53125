using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpGet]
    public ActionResult<List<Doctor>> Get()
    {
        var doctors = _persistence.GetDoctors();
        if (doctors == null)
        {
            return NotFound();
        }
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public ActionResult<Doctor> GetById(Guid id)
    {

        var doctor = _persistence.GetDoctors().FirstOrDefault(d => d.Id == id);

        if (doctor == null)
        {
            return NotFound();
        }
        return Ok(doctor);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var deactivated = _persistence.DeactivateDoctor(id);

        if (!deactivated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Doctor> Create([FromBody] Doctor doctor)
    {
        if (string.IsNullOrWhiteSpace(doctor.Name))
        {
            return BadRequest("El nombre del médico es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(doctor.LicenseNumber))
        {
            return BadRequest("La matrícula del médico es obligatoria.");
        }

        if (doctor.Speciality == null)
        {
            return BadRequest("La especialidad del médico es obligatoria.");
        }

        var specialities = _persistence.GetSpecialities();
        var specialityExists = specialities.Any(s => s.Id == doctor.Speciality.Id);

        if (!specialityExists)
        {
            return BadRequest("La especialidad especificada no existe en el sistema.");
        }

        doctor.Id = Guid.NewGuid();
        _persistence.AddDoctor(doctor);

        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }
}