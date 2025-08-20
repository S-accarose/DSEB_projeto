using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DSEB_projeto.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DSEB_projeto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> UserInfo()
    {
        if (!_signInManager.IsSignedIn(User))
            return RedirectToAction("Login", "Account", new { area = "Identity" });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        ViewBag.UserName = user.UserName;
        ViewBag.Nome = user.Nome;
        ViewBag.Roles = roles;
        ViewBag.Claims = claims;
        ViewBag.IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
