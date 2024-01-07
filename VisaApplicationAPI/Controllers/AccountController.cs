using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisaApplication.Service.Services.Security.Dto;
using VisaApplication.Service.Services.Security.IData;
using VisaApplicationAPI.ViewModels.Account;
using VisaApplicationBase.OperationResult.Enum;

namespace VisaApplicationAPI.Controllers;
public class AccountController : Controller
{
    #region Properties && Constructor
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    #endregion

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        LoginDto loginDto = new LoginDto
        {
            UserName = loginViewModel.UserName,
            Password = loginViewModel.Password
        };

        var result = await _accountService.LoginUser(loginDto);

        if (result.EnumResult == GenericOperationResult.Success)
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> CreateUser(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        RegisterDto registerDto = new RegisterDto
        {
            UserName = registerViewModel.UserName,
            Password = registerViewModel.Password,
            ConfirmPassword = registerViewModel.ConfirmPassword
        };

        var result = await _accountService.CreateUser(registerDto);

        if (result.EnumResult == GenericOperationResult.Success)
        {
            return RedirectToAction("Login", "Account");
        }

        return RedirectToAction("SignUp", "Account");
    }

    [HttpGet]
    public async Task<IActionResult> SignUp()
    {
        return View();
    }
}
