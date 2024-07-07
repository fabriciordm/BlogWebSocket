using ClientBLog.Models;
using ClientBLog.Services;


namespace ClientBLog.Controller
{
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Register(string username, string password)
        {
            return await _authService.RegisterAsync(username, password);
        }

        public async Task<User> Login(string username, string password)
        {
            return await _authService.LoginAsync(username, password);
        }

    }
}
