using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisaApplication.Service.Repositories.Security.IData;
using VisaApplication.Service.Services.Applicant.Dto;
using VisaApplication.Service.Services.Applicant.IData;
using VisaApplication.Service.Services.Content.Dto;
using VisaApplication.Service.Services.Content.IData;
using VisaApplication.Service.Services.Security.IData;
using VisaApplication.Service.Services.VisaType.IData;
using VisaApplicationAPI.Constant;
using VisaApplicationAPI.ViewModels.Account;
using VisaApplicationAPI.ViewModels.Applicant;
using VisaApplicationAPI.ViewModels.VisaType;
using VisaApplicationBase.OperationResult.Enum;

namespace VisaApplicationAPI.Controllers;
public class ApplicantController : Controller
{
    #region Constructor && Properties
    private readonly IApplicantService _applicantService;
    private readonly IFileService _fileService;
    private readonly IAccountService _accountService;
    private readonly IVisaTypeService _visaTypeService;

    public ApplicantController(
        IApplicantService applicantService,
        IFileService fileService,
        IAccountService accountService,
        IVisaTypeService visaTypeService)
    {
        _applicantService = applicantService;
        _fileService = fileService;
        _accountService = accountService;
        _visaTypeService = visaTypeService;
    }
    #endregion

    public async Task<IActionResult> Applicants()
    {
        var result = await _applicantService.GetApplicants();

        var visaTypeResults = await _visaTypeService.GetAvailableVisaTypesAsync();

        var userResults = await _accountService.GetUsers();

        if (result.EnumResult == GenericOperationResult.Success)
        {
            var resultDto = result.Result;
            var visaTypeDto = visaTypeResults.Result;
            var userDtos = userResults.Result;
            var applicantsViewModel = new ApplicantViewModel();

            applicantsViewModel.VisaTypesViewModel = visaTypeDto
                .Select(visaType => new VisaTypeViewModel
                {
                    Id = visaType.Id,
                    Name = visaType.Name,
                }).ToList();

            applicantsViewModel.UsersViewModel = userDtos
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName
                }).ToList();

            applicantsViewModel.ApplicantsInfoViewModel = resultDto
                .Select(data => new ApplicantInfoViewModel
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Age = data.Age,
                    Country = data.Country,
                    City = data.City,
                    Active = data.Active,
                    VisaType = data.VisaType
                }).ToList();

            return View(applicantsViewModel);
        }

        return RedirectToAction("Applicants", "Applicant");
    }

    [HttpPost]
    [Authorize]
    [Route("[action]")]
    public async Task<IActionResult> Action(
        ApplicantFormViewModel applicantFormViewModel)
    {
        var applicantFormDto = new ApplicantFormDto
        {
            Id = applicantFormViewModel.Id,
            FirstName = applicantFormViewModel.FirstName,
            LastName = applicantFormViewModel.LastName,
            Age = applicantFormViewModel.Age,
            Country = applicantFormViewModel.Country,
            City = applicantFormViewModel.City,
            VisaTypeId = applicantFormViewModel.VisaTypeId,
            ApplicationStatus = applicantFormViewModel.ApplicantStatus
        };
        var result = await _applicantService.ActionAsync(
            applicantFormDto, applicantFormViewModel.UserId);

        return RedirectToAction("Applicants", "Applicant");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ApplicantDetails(string id)
    {
        var result = await _applicantService.GetApplicantDetails(id);
        var visaTypeResult = await _visaTypeService.GetAvailableVisaTypesAsync();

        if (result.EnumResult == GenericOperationResult.Success)
        {
            var applicantDetailsViewModel = new ApplicantDetailsViewModel();
            var applicantDetailsDto = result.Result;
            var visaTypeDtos = visaTypeResult.Result;

            applicantDetailsViewModel.VisaTypesViewModel = visaTypeDtos
                .Select(visaType => new VisaTypeViewModel
                {
                    Id = visaType.Id,
                    Name = visaType.Name
                }).ToList();

            applicantDetailsViewModel.ApplicantDetailsInfoViewModel = new ApplicantFormViewModel
            {
                Id = applicantDetailsDto.Id,
                FirstName = applicantDetailsDto.FirstName,
                LastName = applicantDetailsDto.LastName,
                Age = applicantDetailsDto.Age,
                Country = applicantDetailsDto.Country,
                City = applicantDetailsDto.City,
                VisaTypeId = applicantDetailsDto.VisaTypeId,
                ApplicantStatus = applicantDetailsDto.ApplicationStatus
            };

            return View(applicantDetailsViewModel);
        }

        return RedirectToAction("ApplicantDetails", "Applicant");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadDocuments(
       [FromForm] List<IFormFile> documents,
       Guid applicantId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //var applicantId = await _applicantService.FetchApplicantIdByUserID(Guid.Parse(userId));
        List<FileFormDto> filesDto = new();

        foreach (var document in documents)
        {
            var documentFile = await _fileService.SaveFile(
                DataFilesPathes.DocumentFile, document);

            var documentFileDto = documentFile.Result;

            filesDto.Add(new FileFormDto
            {
                Name = documentFileDto.Name,
                Extension = documentFileDto.Extension,
                Url = documentFileDto.Url,
            });
        }

        var result = await _fileService.AddCollectionFileAsync(
                filesDto, applicantId);

        return Json(result.EnumResult == GenericOperationResult.Success);
    }
}
