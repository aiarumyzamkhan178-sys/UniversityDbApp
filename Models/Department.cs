using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название кафедры.")]
        [StringLength(120, ErrorMessage = "Название не должно превышать 120 символов.")]
        [Display(Name = "Название кафедры")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
