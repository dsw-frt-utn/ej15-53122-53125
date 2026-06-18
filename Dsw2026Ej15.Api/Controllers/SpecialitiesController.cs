using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class SpecialitiesController : ControllerBase
{
    private readonly IPersistence _persistence;

    public SpecialitiesController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpGet]
    public ActionResult<List<Speciality>> Get()
    {
        var specialities = _persistence.GetSpecialities();
        return Ok(specialities);
    }
}