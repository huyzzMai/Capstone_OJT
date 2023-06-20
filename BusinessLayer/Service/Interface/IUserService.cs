using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IUserService
    {
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task<User> GerCurrentUser(int id);
    }
}
