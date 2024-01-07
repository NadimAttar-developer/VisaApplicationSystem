namespace VisaApplicationAPI.ViewModels.Applicant;

public class ApplicantFormViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public Guid VisaTypeId { get; set; }
    public Guid UserId { get; set; }
    public bool ApplicantStatus { get; set; }
}
