using Microsoft.AspNetCore.Mvc;
using VisaApplication.Service.Services.VisaType.IData;
using VisaApplicationAPI.ViewModels.VisaType;

namespace VisaApplicationAPI.Controllers;
public class VisaTypeController : Controller
{
    #region Properties && Constructor
    private readonly IVisaTypeService _visaTypeService;

    public VisaTypeController(IVisaTypeService visaTypeService)
    {
        _visaTypeService = visaTypeService;
    }
    #endregion
    public async Task<IActionResult> AvailableVisaTypeApplication()
    {
        var result = await _visaTypeService.GetAvailableVisaTypesAsync();

        var visaTypeViewModel = new List<VisaTypeViewModel>();

        visaTypeViewModel = result.Result.Select(data => new VisaTypeViewModel
        {
            Id = data.Id,
            Name = data.Name,
        }).ToList();

        return View(visaTypeViewModel);
    }
}
