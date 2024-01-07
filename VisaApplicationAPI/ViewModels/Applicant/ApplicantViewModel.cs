using VisaApplicationAPI.ViewModels.Account;
using VisaApplicationAPI.ViewModels.VisaType;

namespace VisaApplicationAPI.ViewModels.Applicant;

public class ApplicantViewModel
{
    public ApplicantViewModel()
    {
        VisaTypesViewModel = new List<VisaTypeViewModel>();
        ApplicantsInfoViewModel = new List<ApplicantInfoViewModel>();
        UsersViewModel = new List<UserViewModel>();
    }
    public IEnumerable<VisaTypeViewModel> VisaTypesViewModel { get; set; }
    public IEnumerable<ApplicantInfoViewModel> ApplicantsInfoViewModel { get; set; }
    public IEnumerable<UserViewModel> UsersViewModel { get; set; }
}
