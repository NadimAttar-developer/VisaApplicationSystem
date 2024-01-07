using VisaApplicationAPI.ViewModels.Account;
using VisaApplicationAPI.ViewModels.VisaType;

namespace VisaApplicationAPI.ViewModels.Applicant;

public class ApplicantDetailsViewModel
{
    public ApplicantDetailsViewModel()
    {
        VisaTypesViewModel = new List<VisaTypeViewModel>();
    }
    public IEnumerable<VisaTypeViewModel> VisaTypesViewModel { get; set; }
    public ApplicantFormViewModel ApplicantDetailsInfoViewModel { get; set; }
}
