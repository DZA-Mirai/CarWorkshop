namespace CarWorkshop.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public string Login { get; set; }
        public string? Password { get; set; }
        public double? Salary { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public DateTime? WorkingSince { get; set; }
        public bool IsAdmin { get; set; }
    }
}
