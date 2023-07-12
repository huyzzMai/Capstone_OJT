using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
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
        Task SendTokenResetPassword(string email);
        Task VerifyResetCode(string token);
        Task ResetPassword(ResetPasswordRequest request);
        Task<User> GetUserById(int id);
        Task<BasePagingViewModel<TrainerResponse>> GetTrainerList(PagingRequestModel paging);
        Task<BasePagingViewModel<UserListResponse>> GetUserList(PagingRequestModel paging);
        Task<BasePagingViewModel<TraineeResponse>> GetTraineeList(PagingRequestModel paging);
        Task<BasePagingViewModel<TraineeResponse>> GetTraineeListByTrainer(int id, PagingRequestModel paging);
        Task AssignTraineeToTrainer(int trainerId, int traineeId);
        int GetCurrentLoginUserId(string authHeader);
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task UpdateUserInformation(int id, UpdateUserInformationRequest model);
    }
}
