using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.ApplicationCore.Dtos;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    // private readonly SignInManager<ApplicationUser> _signInManager;
    // private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RegisterModel> _logger;
    // private readonly IEmailSender _emailSender;
    private readonly IUserManagementService _userManagmentService;

    public RegisterModel(
        // UserManager<ApplicationUser> userManager,
        // SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        // IEmailSender emailSender,
        IUserManagementService userManagmentService)
    {
        // _userManager = userManager;
        // _signInManager = signInManager;
        _logger = logger;
        // _emailSender = emailSender;
        _userManagmentService = userManagmentService;
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public required string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public required string City { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public required string ZipCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public required string Country { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public required string Mobile { get; set; }

    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            var userDto = new RegisterUserDto
            {
                Email = Input?.Email,
                Password = Input?.Password,
                ConfirmPassword = Input?.ConfirmPassword,
                FirstName = Input?.FirstName,
                LastName = Input?.LastName,
                Address = Input?.Address,
                ZipCode = Input?.ZipCode,
                City = Input?.City,
                Country = Input?.Country,
                Mobile = Input?.Mobile
            };

            var result = await _userManagmentService.CreateUserAsync(userDto);

            // var user = new ApplicationUser { UserName = Input?.Email, Email = Input?.Email };
            // var result = await _userManager.CreateAsync(user, Input?.Password!);
            // if (result.Succeeded)
            // {
            //     _logger.LogInformation("User created a new account with password.");
            //
            //     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //     var callbackUrl = Url.Page(
            //         "/Account/ConfirmEmail",
            //         pageHandler: null,
            //         values: new { userId = user.Id, code = code },
            //         protocol: Request.Scheme);
            //
            //     Guard.Against.Null(callbackUrl, nameof(callbackUrl));
            //     await _emailSender.SendEmailAsync(Input!.Email!, "Confirm your email",
            //         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            //
            //     await _signInManager.SignInAsync(user, isPersistent: false);
            //     return LocalRedirect(returnUrl);
            // }
            // foreach (var error in result.Errors ?? throw new ArgumentNullException())
            // {
            //     ModelState.AddModelError(string.Empty, error);
            // }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
