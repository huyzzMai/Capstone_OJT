﻿using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.RequestModel.UserRequest;
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
        Task<User> GetUserById(int id);
        Task<IEnumerable<TraineeResponse>> GetTrainerList();

        Task<IEnumerable<UserListResponse>> GetUserList();
        Task<IEnumerable<TraineeResponse>> GetTraineeList();
        Task<IEnumerable<TraineeResponse>> GetTraineeListByTrainer(int id);
        int GetCurrentLoginUserId(string authHeader);
        Task UpdateUserInformation(int id, UpdateUserInformationRequest model);
    }
}
