namespace Proje.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        public string Notes { get; set; }
    }
}