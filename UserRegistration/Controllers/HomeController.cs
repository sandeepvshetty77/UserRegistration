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
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, 
                                IConfiguration config,
                                IHttpContextAccessor httpContextAccessor,
                                IUserRepository userRepository)
        {
            _config = config;           
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
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
            if (checkIfUserAlreadyExists(userViewModel))
            {
                ModelState.AddModelError(String.Empty, "Error: User already exists");
                return View(userViewModel);
            }
            
            if (ModelState.IsValid)
            {
                User newUser = saveUser(userViewModel);

                if (newUser != null)
                {
                    token = generateJSONWebToken(userViewModel);

                    // saving the token in a httpOnly cookie.
                    persistTokenInCookies(token);

                    // TO BE REMOVED .. THIS HAS BEEN EXPOSED ON THE PAGE ONLY FOR QUICK VERIFICATION PURPOSE
                    ViewBag.Data = token;
                }                
            }

            return View();
        }

        private User saveUser(UserViewModel userViewModel)
        {
            User user = new User();
            user.Username = userViewModel.Username;
            user.Password = userViewModel.Password;

            User newUser = _userRepository.CreateUser(user);
            return newUser;
        }

        private bool checkIfUserAlreadyExists(UserViewModel userViewModel)
        {
            return (_userRepository.GetUserByUserName(userViewModel.Username) != null);
        }

        private void persistTokenInCookies(string token)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(5);
            options.HttpOnly = true;
            _httpContextAccessor.HttpContext.Response.Cookies.Append(StaticKeys.JwtTokenCookie, token, options);
        }

        private string generateJSONWebToken(UserViewModel userViewModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[StaticKeys.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(_config[StaticKeys.JwtIssuer],
              _config[StaticKeys.JwtIssuer],
              new Claim[] {
                    new Claim(ClaimTypes.Name, userViewModel.Username)       // This username can be verified when the token is verified using a JWT debugger at https://jwt.io/ 
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
