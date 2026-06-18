using System;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain;
using System.Linq;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private readonly List<Doctor> _doctors = new();
        private readonly List<Speciality> _specialities = new();


        public List<Doctor> GetDoctors() => _doctors;
        public List<Speciality> GetSpecialities() => _specialities;
        public void AddDoctor(Doctor doctor)
        {
            _doctors.Add(doctor);
        }
        public Doctor? GetDoctor(Guid id) => _doctors.FirstOrDefault(d => d.Id == id);

        public bool DeactivateDoctor(Guid id)
        {
            var doctor = GetDoctor(id);
            if (doctor == null) return false;
            doctor.IsActive = false;
            return true;
        }

    }
}