namespace Jobify.Services.Features.Profile.Commands.UpdateProfile
{
    public class ExperienceDto
    {
        public int? Id { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string JobType { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Summary { get; set; }
    }
}
