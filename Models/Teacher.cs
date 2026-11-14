using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ФИО преподавателя.")]
        [StringLength(160, ErrorMessage = "ФИО не должно превышать 160 символов.")]
        [Display(Name = "ФИО преподавателя")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Кафедра")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите кафедру.")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;
    }
}
