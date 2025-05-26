namespace Jobify.Services.Features.Profile.Commands.UpdateProfile
{
    public class EducationDto
    {
        public int? Id { get; set; }
        public string SchoolName { get; set; }
        public string Major { get; set; }
        public string DegreeType { get; set; }
        public decimal? Gpa { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
