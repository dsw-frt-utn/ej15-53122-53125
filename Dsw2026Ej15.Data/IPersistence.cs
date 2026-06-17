using System;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Data
{
    public interface IPersistence
    {
        List<Doctor> GetDoctors();
        List<Speciality>GetSpecialities();

        void AddDoctor(Doctor doctor);
        Doctor ? GetDoctor(Guid id);
    }
}
