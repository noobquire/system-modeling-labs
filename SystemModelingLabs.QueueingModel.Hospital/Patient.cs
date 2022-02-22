using SystemModelingLabs.QueueingModel.Core.Models;

namespace SystemModelingLabs.QueueingModel.Hospital
{
    public enum PatientType
    {
        // хворі, що пройшли попереднє обстеження і направлені на лікування
        PreExaminatonCompleted,
        // хворі, що бажають потрапити в лікарню, але не пройшли повністю попереднє обстеження
        PreExaminationInProgress,
        // хворі, які тільки що поступили на попереднє обстеження
        ToBeExamined
    }

    public class Patient : QueueItem
    {
        public string Name { get; set; }
        public double ArrivedToLabAt { get; set; }

        public PatientType Type { get; set; }

        public Patient(PatientType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name}, type {Type}";
        }
    }
}
