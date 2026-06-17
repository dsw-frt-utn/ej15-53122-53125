
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data; 
using Dsw2026Ej15.Domain;
using Microsoft.AspNetCore.DataProtection;


[ApiController]
[Route("api/[controller]")]

namespace Dsw2026Ej15.Api.Controllers
{
    public class DoctorsController:ControllerBase
    {
        private readonly IPersistence _persistence;
        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }
        public ActionResult<List<Doctor>> Get()
        {
            var doctors = _persistence.GetDoctor();
            if (doctors== null)
            {
                return NotFound();
            }
            return Ok(doctors);
        }
}
}
