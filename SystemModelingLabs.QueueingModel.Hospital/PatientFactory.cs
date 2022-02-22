using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemModelingLabs.QueueingModel.Core;
using SystemModelingLabs.Utils;

namespace SystemModelingLabs.QueueingModel.Hospital
{
    public class PatientFactory : IItemFactory<Patient>
    {
        public List<Patient> Patients { get; } = new List<Patient>();

        public Patient Create()
        {
            var rng = new Random();
            var value = rng.NextDouble();
            PatientType type;
            if (value < 0.5)
            {
                type = PatientType.PreExaminatonCompleted;
            }
            else if (value < 0.6)
            {
                type = PatientType.PreExaminationInProgress;
            }
            else
            {
                type = PatientType.ToBeExamined;
            }

            var patient = new Patient(type)
            {
                Name = RandomDataGenerator.GetRandomPersonName(),
            };
            Patients.Add(patient);
            return patient;
        }
    }
}
