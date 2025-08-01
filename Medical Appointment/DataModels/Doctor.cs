using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical_Appointment.DataModels
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Unicode(false)]

        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
