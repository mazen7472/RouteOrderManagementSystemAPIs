using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Core.DTOs.Identity;
using OrderManagementSystem.Core.Entites.Identity;
using OrderManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterDTO registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var token = await _tokenService.GenerateTokenAsync(user);

                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = token,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }

            throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
        }

        public async Task<UserDTO> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                var token = await _tokenService.GenerateTokenAsync(user);

                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = token,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }

            throw new Exception("Invalid username or password");
        }
    }
}
