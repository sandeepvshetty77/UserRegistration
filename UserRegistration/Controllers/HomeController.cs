using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserRegistration.Models;

namespace UserRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;  
        private readonly IUserRepository _userRepository;
        private readonly ITokenManager _tokenManager;

        public HomeController(ILogger<HomeController> logger, 
                              IUserRepository userRepository,
                              ITokenManager tokenManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenManager = tokenManager;
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
                    token = _tokenManager.GenerateToken(userViewModel);

                    // saving the token 
                    _tokenManager.SaveToken(token);

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
