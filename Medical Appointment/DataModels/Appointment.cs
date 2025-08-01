namespace Medical_Appointment.DataModels
{
    public class Appointment
    {
        
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

    }
}
