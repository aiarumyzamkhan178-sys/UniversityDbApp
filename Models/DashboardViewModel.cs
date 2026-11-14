namespace UniversityDbApp.Models
{
    public class DashboardViewModel
    {
        public int DepartmentsCount { get; set; }
        public int TeachersCount { get; set; }
        public int RecentTeachersCount { get; set; }
        public List<Teacher> RecentTeachers { get; set; } = new();
        public List<DepartmentStatistic> DepartmentStatistics { get; set; } = new();
    }

    public class DepartmentStatistic
    {
        public string Name { get; set; } = string.Empty;
        public int TeachersCount { get; set; }
    }
}
