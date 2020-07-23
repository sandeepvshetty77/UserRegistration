using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UserRegistration.Models;

namespace UserRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, 
                                IConfiguration config,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _config = config;           
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserViewModel userViewModel)
        {
            string token = String.Empty;
            if (ModelState.IsValid)
            {
                token = GenerateJSONWebToken(userViewModel);

                persistTokenInCookies(token);
                ViewBag.Data = token;
            }

            return View();
        }

        private void persistTokenInCookies(string token)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(5);
            options.HttpOnly = true;
            _httpContextAccessor.HttpContext.Response.Cookies.Append("JWTtoken", token, options);
        }

        private string GenerateJSONWebToken(UserViewModel userViewModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, userViewModel.Id.ToString())
                 },
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
