using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using BusinessLayer.Models.ResponseModel.TaskResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Implement;
using DataAccessLayer.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region CreateToken
        public LoginResponse CreateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

            var result = new LoginResponse()
            {
                Token = token
            };
            return result;
        }
        #endregion

        #region Encrypt Password
        public string EncryptPassword(string plainText)
        {
            var key = _configuration["Secret:Value"];
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        #endregion

        #region Decrypt Password
        public string DecryptPassword(string plainText)
        {
            var key = _configuration["Secret:Value"];
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

        #region Send Token Reset Password

        public async Task<string> CheckResetCode(string code)
        {
            var u = await _unitOfWork.UserRepository.GetUserByResetCode(code);
            if (u == null)
            {
                return code;
            }
            else
            {
                return await CheckResetCode(GenerateRamdomCode());
            }
        }

        public string GenerateRamdomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;   
        }

        public async Task SendTokenResetPassword(string email)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByEmailAndStatusActive(email);
                if (u == null)
                {
                    throw new Exception("User not found!");
                }
                String r = GenerateRamdomCode();

                var finalR = await CheckResetCode(r);

                //var check = await _unitOfWork.UserRepository.GetUserByResetCode(r);
                //if (check != null)
                //{
                //    throw new Exception("There is an error! Please resend verification code.");
                //}

                u.ResetPassordCode = finalR;
                await _unitOfWork.UserRepository.Update(u);

                var sender = new MailSender();
                sender.Send(email, u.Name, r);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task VerifyResetCode(string token)
        {
            var u = await _unitOfWork.UserRepository.GetUserByResetCodeAndStatusActive(token);
            if (u == null)
            {
                throw new Exception("Reset code is not correct!");
            }
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByResetCodeAndStatusActive(request.ResetCode);
                if (u == null)
                {
                    throw new Exception("Reset code is not correct! Please go back to resend verification code.");
                }
                var newPassword = EncryptPassword(request.NewPassword);
                u.Password = newPassword;
                u.ResetPassordCode = null;
                await _unitOfWork.UserRepository.Update(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            User u = await _unitOfWork.UserRepository.GetUserByEmailAndStatusActive(request.Email);
            if (u == null)
            {
                throw new Exception("Email is wrong!");
            }
            var decryptPass = DecryptPassword(u.Password);

            if (request.Password != decryptPass)
            {
                throw new Exception("Wrong password!");
            }

            String userId = u.Id.ToString();

            if (u.Role == CommonEnums.ROLE.ADMIN)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim("UserId", userId.ToString())
                    };
                var result = CreateToken(claims);
                return result;
            }
            else if (u.Role == CommonEnums.ROLE.MANAGER)
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim("UserId", userId.ToString())
                    };
                var result = CreateToken(claims);
                return result;
            }
            else if (u.Role == CommonEnums.ROLE.TRAINER)
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Trainer"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim("UserId", userId.ToString())
                    };
                var result = CreateToken(claims);
                return result;
            }
            else if (u.Role == CommonEnums.ROLE.TRAINEE)
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Role, "Trainee"),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim("UserId", userId.ToString())
                    };
                var result = CreateToken(claims);
                return result;
            }
            return null;
        }

        public int GetCurrentLoginUserId(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            int userId = int.Parse(id);
            return userId;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==id && c.Status == CommonEnums.USER_STATUS.ACTIVE);
            if (user == null)
            {
                throw new Exception("This user cannot be found!");
            }
            return user;
        }

        public async Task<BasePagingViewModel<TrainerResponse>> GetTrainerList(PagingRequestModel paging)
        {
            var users = await _unitOfWork.UserRepository.GetTrainerList();
            List<TrainerResponse> res = users.Select(
                user =>
                {
                    return new TrainerResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Email = user.Email
                    };
                }
                ).ToList();

           int totalItem = res.Count;

            var result = new BasePagingViewModel<TrainerResponse>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = res
            };
            return result;
        }

        public async Task<BasePagingViewModel<TraineeResponse>> GetTraineeList(PagingRequestModel paging)
        {
            var users = await _unitOfWork.UserRepository.GetTraineeList();
            List<TraineeResponse> res = users.Select(
                user =>
                {
                    return new TraineeResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Email = user.Email
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            var result = new BasePagingViewModel<TraineeResponse>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = res
            };
            return result;
        }

        public async Task<BasePagingViewModel<TraineeResponse>> GetTraineeListByTrainer(int id, PagingRequestModel paging)
        {
            var users = await _unitOfWork.UserRepository.GetTraineeListByTrainerId(id);
            List<TraineeResponse> res = users.Select(
                user =>
                {
                    return new TraineeResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Email = user.Email
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            var result = new BasePagingViewModel<TraineeResponse>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = res
            };
            return result;
        }

        public async Task AssignTraineeToTrainer (int trainerId, int traineeId)
        {
            try
            {
                var trainer = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(trainerId);  
                if (trainer.Role != CommonEnums.ROLE.TRAINER)
                {
                    throw new Exception("Trainer not found!");
                }
                var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                if (trainee.Role != CommonEnums.ROLE.TRAINEE)
                {
                    throw new Exception("Trainee not found!");
                }

                trainee.UserReferenceId = trainerId;
                trainee.UpdatedAt = DateTime.Now;
                await _unitOfWork.UserRepository.Update(trainee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var emailCheck = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);
            if (emailCheck != null)
            {
                throw new Exception("Email has been used!");
            }

            String r = GenerateRamdomCode();
            string pwd = "tn" + r;
            string encryptPwd = EncryptPassword(pwd);

            User user = new User()
            {
                Name = request.FullName,
                Email = request.Email,
                Birthday = request.Birthday,    
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Gender = request.Gender,    
                RollNumber = request.RollNumber,
                AvatarURL = request.AvatarUrl,
                Role = request.Role,
                Password = encryptPwd,
                CreatedAt = DateTime.Now,
                Status = CommonEnums.USER_STATUS.ACTIVE
            };

            await _unitOfWork.UserRepository.Add(user);

            var rs = new CreateUserResponse
            {
                Email = request.Email,
                Password = pwd
            };
            return rs;
        }

        public async Task UpdateUserInformation(int id, UpdateUserInformationRequest model)
        {
            var u = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(id);

            if (u == null)
            {
                throw new Exception("This user cannot be updated!");
            }

            #region Old method
            //var check = await _unitOfWork.UserRepository.GetUserByEmailAndDeleteIsFalse(model.Email);
            //if (check != null)
            //{
            //    throw new Exception("This email existed!");
            //}

            //if (model.FullName == null)
            //{
            //    u.Name = u.Name;
            //}
            //else
            //{
            //    u.Name = model.FullName;
            //}
            //if (model.Birthday == null)
            //{
            //    u.Birthday = u.Birthday;
            //}
            //else
            //{
            //    u.Birthday = model.Birthday;
            //}

            //if (model.PhoneNumber == null)
            //{
            //    u.PhoneNumber = u.PhoneNumber;
            //}
            //else
            //{
            //    u.PhoneNumber = model.PhoneNumber;
            //}
            #endregion

            if (model.FullName != null)
            {
                u.Name = model.FullName;
            }

            if (model.Birthday != null)
            {
                u.Birthday = model.Birthday;
            }

            if (model.PhoneNumber != null)
            {
                u.PhoneNumber = model.PhoneNumber;
            }

            if (model.Gender != null)
            {
                u.Gender = model.Gender;
            }

            if (model.Address != null)
            {
                u.Address = model.Address;
            }

            if (model.AvatarURL != null)
            {
                u.AvatarURL = model.AvatarURL;
            }

            u.UpdatedAt = DateTime.Now;

            await _unitOfWork.UserRepository.Update(u);
        }

        public async Task<BasePagingViewModel<UserListResponse>> GetUserList(PagingRequestModel paging)
        {
            var users = await _unitOfWork.UserRepository.Get(c=>c.Status == CommonEnums.USER_STATUS.ACTIVE && c.Role!= CommonEnums.ROLE.ADMIN);
            List<UserListResponse> res = users.Select(
                user =>
                {
                    return new UserListResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Role = user.Role
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            var result = new BasePagingViewModel<UserListResponse>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalItem = totalItem,
                TotalPage = (int)Math.Ceiling((decimal)totalItem / (decimal)paging.PageSize),
                Data = res
            };
            return result;
        }
    }
}
