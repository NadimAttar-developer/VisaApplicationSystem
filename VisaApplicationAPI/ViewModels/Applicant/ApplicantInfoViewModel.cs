namespace VisaApplicationAPI.ViewModels.Applicant;

public class ApplicantInfoViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string VisaType { get; set; }
    public string Active { get; set; }
}
