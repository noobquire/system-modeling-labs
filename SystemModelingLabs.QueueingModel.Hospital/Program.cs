using SystemModelingLabs;
using SystemModelingLabs.QueueingModel.Core;
using SystemModelingLabs.QueueingModel.Core.Elements;
using SystemModelingLabs.QueueingModel.Core.Models;
using SystemModelingLabs.QueueingModel.Hospital;
using SystemModelingLabs.Utils;
namespace SystemModelingLabs.QueueingModel.Hospital;

public class Program
{
    public static void Main()
    {

        var patientFactory = new PatientFactory();
        Func<Patient, double> patientCreationRate = patient => RandomUtils.NextExponential(1d / 15);
        var patientCreation = new CreateItemsElement<Patient>(patientFactory, patientCreationRate)
        {
            Name = "new patient creation"
        };

        Func<Patient, double> dutyDoctorDelay = patient =>
        patient.Type switch
        {
            PatientType.PreExaminatonCompleted => RandomUtils.NextExponential(1d / 15),
            PatientType.PreExaminationInProgress => RandomUtils.NextExponential(1d / 40),
            PatientType.ToBeExamined => RandomUtils.NextExponential(1d / 30)
        };

        Func<Patient, int> patientPriorityFunc = patient => patient.Type switch
        {
            PatientType.PreExaminatonCompleted => 1,
            _ => 2
        };
        var dutyDoctorProcessing = new PriorityQueueProcessingElement<Patient>(dutyDoctorDelay, patientPriorityFunc, channels: 2)
        {
            Name = "doctor on duty"
        };
        patientCreation.NextElement = dutyDoctorProcessing;

        Func<Patient, double> doctorToLabDelayFunc = patient => RandomUtils.NextUniform(2, 5);
        var doctorToLabDelay = new DelayElement<Patient>(doctorToLabDelayFunc)
        {
            Name = "walking from doctor to lab"
        };
        var labToDoctorDelay = new DelayElement<Patient>(doctorToLabDelayFunc)
        {
            Name = "walking from lab to doctor"
        };
        labToDoctorDelay.NextElement = dutyDoctorProcessing;

        Func<Patient, double> waitingRoomToHospitalRoomDelayFunc = patient => RandomUtils.NextUniform(3, 8);
        var hospitalRoomProcessing = new PriorityQueueProcessingElement<Patient>(waitingRoomToHospitalRoomDelayFunc, p => 1)
        {
            Name = "being escorted to hospital room"
        };

        DecisionFunction<Patient> labOrWaitingRoomDecisionFunc = (elements, patient) => patient.Type switch
        {
            PatientType.PreExaminatonCompleted => elements.ElementAt(0),
            _ => elements.ElementAt(1)
        };
        var labOrWaitingRoomOptions = new Element<Patient>[] { hospitalRoomProcessing, doctorToLabDelay };
        var labOrWaitingRoomDecision = new ConditionElement<Patient>(labOrWaitingRoomOptions, labOrWaitingRoomDecisionFunc, p => 0)
        {
            Name = "choosing to go to lab or waiting room"
        };
        dutyDoctorProcessing.NextElement = labOrWaitingRoomDecision;

        Func<Patient, double> labRegistrationDelayFunc = patient => RandomUtils.NextErlang(4.5, 3);
        var labRegistration = new PriorityQueueProcessingElement<Patient>(labRegistrationDelayFunc, p => 1)
        {
            Name = "registration in lab"
        };
        doctorToLabDelay.NextElement = labRegistration;

        Func<Patient, double> labAnalysisDelayFunc = patient => RandomUtils.NextErlang(4, 2);
        var labAnalysis = new PriorityQueueProcessingElement<Patient>(labAnalysisDelayFunc, p => 1, channels: 2)
        {
            Name = "waiting for lab analysis"
        };
        labRegistration.NextElement = labAnalysis;

        DecisionFunction<Patient> labToDoctorOrExitDecisionFunc = (elements, patient) =>
        {
            if (patient.Type == PatientType.PreExaminationInProgress)
            {
                patient.Type = PatientType.PreExaminatonCompleted;
                return elements.ElementAt(0);
            }

            Console.WriteLine($"{patient} exiting system from lab");
            return null;
        };
        var labToDoctorOrExitOptions = new[] { labToDoctorDelay };

        var labToDoctorOrExitDecision = new ConditionElement<Patient>(labToDoctorOrExitOptions, labToDoctorOrExitDecisionFunc, p => 0)
        {
            Name = "choosing to doctor or exit lab"
        };
        labAnalysis.NextElement = labToDoctorOrExitDecision;

        var elements = new Element<Patient>[]
        {
            patientCreation,
            dutyDoctorProcessing,
            doctorToLabDelay,
            labToDoctorDelay,
            hospitalRoomProcessing,
            labOrWaitingRoomDecision,
            labRegistration,
            labToDoctorOrExitDecision,
            labAnalysis
        };

        var model = new QueueingModel<Patient>(elements);
        model.Simulate(10000);
        foreach(var element in elements)
        {
            Console.WriteLine($"{element.Name} processed {element.ProcessedItemsCount} items");
        }

        var timeInSystem = patientFactory.Patients.Where(p => p.ExitedAt != 0).Select(p => p.ExitedAt - p.CreatedAt);
        Console.WriteLine($"Average time spent in system: {timeInSystem.Average()}");

    }
}