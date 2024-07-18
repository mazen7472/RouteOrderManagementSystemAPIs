using OrderManagementSystem.Core.DTOs.Identity;
using OrderManagementSystem.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUserAsync(RegisterDTO registerDTO);
        Task<UserDTO> LoginUserAsync(LoginDTO loginDTO);
    }
}
