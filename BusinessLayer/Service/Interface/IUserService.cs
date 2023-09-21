using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.AuthenticationRequest;
using BusinessLayer.Payload.RequestModel.UserRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.AuthenticationResponse;
using BusinessLayer.Payload.ResponseModel.CriteriaResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
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
        Task<TokenResponse> CreateToken(string userId, string role);
        Task SaveUserRefToken(int userId, string refToken);
        Task<bool> CheckExistUserRefToken(string refToken);
        Task<User> GetUserByRefToken(string refToken);
        Task SendTokenResetPassword(string email);
        Task VerifyResetCode(string token);
        Task ResetPassword(ResetPasswordRequest request);
        Task<PersonalUserResponse> GetUserProfile(int id);
        Task<UserCommonResponse> GetCurrentUserById(int id);
        Task<BasePagingViewModel<TrainerResponse>> GetTrainerList(PagingRequestModel paging, string keyword, int? position);
        Task<BasePagingViewModel<UserListResponse>> GetUserList(PagingRequestModel paging,string searchTerm,int? role,int? filterStatus);
        Task<BasePagingViewModel<TraineeResponse>> GetTraineeList(PagingRequestModel paging, string keyword, int? position);
        Task<BasePagingViewModel<TraineeResponse>> GetTraineeListByTrainer(int id, PagingRequestModel paging);
        Task<List<UnassignedTraineeResponse>> GetUnassignedTraineeList();
        Task<TraineeResponse> GetTraineeDetail(int roleId, int traineeId);
        Task<TrainerResponse> GetTrainerDetail(int trainerId);
        Task AssignTraineeToTrainer(AssignTraineesRequest request);
        int GetCurrentLoginUserId(string authHeader);
        //Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task CreateUser(CreateUserRequest request);

        Task ActiveUser(int id);

        Task DisableUser(int id);

        Task UpdateUserPassword(int id, UpdateUserPasswordRequest model);
        Task UpdateUserInformation(int id, UpdateUserInformationRequest model);

        Task <UserDetailResponse> GetUserDetail(int id);
       
    }
}
