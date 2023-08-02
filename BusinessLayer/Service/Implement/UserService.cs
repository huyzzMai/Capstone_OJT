using BusinessLayer.Models.RequestModel;
using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.RequestModel.UserRequest;
using BusinessLayer.Models.ResponseModel;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using BusinessLayer.Models.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Implement;
using DataAccessLayer.Repository.Interface;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Models.ResponseModel.UserResponse.PersonalUserResponse;

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

        #region CreateToken

        public async Task SaveUserRefToken(int userId, string refToken)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(userId);
                if (u == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                u.RefreshToken = refToken;
                await _unitOfWork.UserRepository.Update(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<bool> CheckExistUserRefToken(string refToken)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByRefTokenAndStatusActive(refToken);
                if (u == null)
                {
                    return false;   
                }
                return true;    

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> GetUserByRefToken(string refToken)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByRefTokenAndStatusActive(refToken);
                if (u == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
                }
                return u;   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TokenResponse> CreateToken(string userId, string role)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim("UserId", userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var refKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshKey"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var refSignIn = new SigningCredentials(refKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], 
                                 claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            var refToken = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                                 claims, expires: DateTime.UtcNow.AddDays(1.25), signingCredentials: refSignIn);

            var result = new TokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token), 
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refToken)
            };
            return result;
        }
        #endregion

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            User u = await _unitOfWork.UserRepository.GetUserByEmailAndStatusActive(request.Email);
            if (u == null)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Email is wrong!");
            }
            var decryptPass = DecryptPassword(u.Password);

            if (request.Password != decryptPass)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Wrong password!");
            }

            String userId = u.Id.ToString();

            if (u.Role == CommonEnums.ROLE.ADMIN)
            {
                string role = "Admin";
                return new LoginResponse()
                {
                    UserId = userId,
                    Role = role,    
                };
            }
            else if (u.Role == CommonEnums.ROLE.MANAGER)
            {
                string role = "Manager";
                return new LoginResponse()
                {
                    UserId = userId,
                    Role = role,
                };
            }
            else if (u.Role == CommonEnums.ROLE.TRAINER)
            {
                string role = "Trainer";
                return new LoginResponse()
                {
                    UserId = userId,
                    Role = role,
                };
            }
            else if (u.Role == CommonEnums.ROLE.TRAINEE)
            {
                string role = "Trainee";
                return new LoginResponse()
                {
                    UserId = userId,
                    Role = role,
                };
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

        public async Task<PersonalUserResponse> GetUserProfile(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdWithSkillList(id);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "This user cannot be found!");
                }

                PersonalUserResponse result = new()
                {
                    FullName = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    AvatarURL = user.AvatarURL,
                    Birthday = user.Birthday ?? default(DateTime),
                    Gender = user.Gender ?? default(int),
                    PhoneNumber = user.PhoneNumber,
                    RollNumber = user.RollNumber,
                    Position = user.Position,
                };

                var listSkill = new List<PersonalSkillResponse>();

                foreach (var sa in user.UserSkills)
                {
                    var s = new PersonalSkillResponse();
                    s.Name = sa.Skill.Name;
                    //s.Type = sa.Skill.Type;
                    s.CurrentLevel = sa.CurrentLevel ?? default(int);
                    listSkill.Add(s);
                }
                result.Skills = listSkill;  

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserCommonResponse> GetCurrentUserById(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == id && c.Status == CommonEnums.USER_STATUS.ACTIVE);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "This user cannot be found!");
                }

                UserCommonResponse usercommon = new UserCommonResponse()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    AvatarURL = user.AvatarURL,
                    Birthday = user.Birthday ?? default(DateTime),
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    RollNumber = user.RollNumber,
                    Position = user.Position,
                    TrelloId = user.TrelloId,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };
                return usercommon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BasePagingViewModel<TrainerResponse>> GetTrainerList(PagingRequestModel paging, string keyword, int? position)
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetTrainerList(keyword, position);

                List<TrainerResponse> res = users.Select(
                    user =>
                    {
                        return new TrainerResponse()
                        {
                            Id = user.Id,
                            FullName = user.Name,
                            Email = user.Email,
                            Gender = user.Gender ?? default(int),
                            AvatarURL = user.AvatarURL,
                            Position = user.Position ?? default(int)
                        };
                    }
                    ).ToList();

                int totalItem = res.Count;

                res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                        .Take(paging.PageSize).ToList();

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TrainerResponse> GetTrainerDetail(int trainerId)
        {
            try
            {
                var trainer = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(trainerId);
                TrainerResponse res = new()
                {
                    Id = trainer.Id,
                    FullName = trainer.Name,
                    Email = trainer.Email,
                    Gender = trainer.Gender ?? default(int),
                    AvatarURL = trainer.AvatarURL,
                    Position = trainer.Position ?? default(int)
                };
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BasePagingViewModel<TraineeResponse>> GetTraineeList(PagingRequestModel paging, string keyword, int? position)
        {
            var users = await _unitOfWork.UserRepository.GetTraineeList(keyword, position);
            List<TraineeResponse> res = users.Select(
                user =>
                {
                    return new TraineeResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Email = user.Email,
                        Gender = user.Gender ?? default(int),
                        AvatarURL = user.AvatarURL,
                        Position = user.Position ?? default(int)
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

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
                        Email = user.Email,
                        Gender = user.Gender ?? default(int),
                        AvatarURL = user.AvatarURL,
                        Position = user.Position ?? default(int)
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

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

        public async Task<TraineeResponse> GetTraineeDetail(int roleId, int traineeId)
        {
            try
            {
                var check = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(roleId);
                if (check.Role == CommonEnums.ROLE.MANAGER)
                {
                    var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);
                    TraineeResponse res = new()
                    {
                        Id =trainee.Id, 
                        FullName = trainee.Name,    
                        Email = trainee.Email,  
                        AvatarURL = trainee.AvatarURL,  
                        Gender = trainee.Gender ?? default(int),
                        Position = trainee.Position ?? default(int)
                    };
                    return res; 
                }
                else
                {
                    var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(traineeId);

                    if (trainee.UserReferenceId != roleId)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Not your assigned trainee!");
                    }

                    TraineeResponse res = new()
                    {
                        Id = trainee.Id,
                        FullName = trainee.Name,
                        Email = trainee.Email,
                        AvatarURL = trainee.AvatarURL,
                        Gender = trainee.Gender ?? default(int),
                        Position = trainee.Position ?? default(int)
                    };
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AssignTraineeToTrainer(AssignTraineesRequest request)
        {
            try
            {
                var trainer = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(request.TrainerId);
                if (trainer.Role != CommonEnums.ROLE.TRAINER)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainer not found!");
                }

                foreach (var item in request.Trainees)
                {
                    var trainee = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(item.TraineeId);
                    if (trainee.Role != CommonEnums.ROLE.TRAINEE || trainee.UserReferenceId != null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Trainee not available!");
                    }

                    trainee.UserReferenceId = request.TrainerId;
                    trainee.UpdatedAt = DateTime.UtcNow.AddHours(7);
                    await _unitOfWork.UserRepository.Update(trainee);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateUser(CreateUserRequest request)
        {
            try
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
                    Role = request.Role,
                    Name = request.FullName,
                    Email = request.Email,
                    Birthday = request.Birthday,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Gender = request.Gender,
                    RollNumber = request.RollNumber,
                    AvatarURL = request.AvatarUrl,
                    Password = encryptPwd,
                    Position = request.Position,
                    TrelloId = request.TrelloId,
                    OJTBatchId = request.BatchId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.USER_STATUS.ACTIVE
                };

                ICollection<UserSkill> re = new List<UserSkill>();
                foreach (var skillRequest in request.CreateSkills)
                {
                    UserSkill us = new()
                    {
                        SkillId = skillRequest.SkillId,
                        InitLevel = skillRequest.InitLevel,
                        CurrentLevel = skillRequest.InitLevel,
                        UserId = user.Id
                    };
                    re.Add(us);
                }
                user.UserSkills = re;   

                await _unitOfWork.UserRepository.Add(user);

                var sender = new MailSender();
                sender.SendMailCreateAccount(user.Email, user.Name, user.Email, pwd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserInformation(int id, UpdateUserInformationRequest model)
        {
            var u = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(id);

            if (u == null)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "This user cannot be found!");
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
            u.UpdatedAt = DateTime.UtcNow.AddHours(7);
            await _unitOfWork.UserRepository.Update(u);
        }
        public List<User> SearchUsers(string searchTerm, int? role, List<User> userList)
        {
            var query = userList.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                query = query.Where(c =>
                    (c.Name != null && c.Name.ToLower().Contains(searchTerm)) ||
                    (c.Address != null && c.Address.ToLower().Contains(searchTerm)) ||
                   (c.Email != null && c.Email.ToLower().Contains(searchTerm)) ||
                   (c.RollNumber != null && c.RollNumber.ToLower().Contains(searchTerm))
               );
            }

            if (role != null)
            {
                query = query.Where(c => c.Role == role);
            }

            return query.ToList();
        }


        public async Task<BasePagingViewModel<UserListResponse>> GetUserList(PagingRequestModel paging, string searchTerm, int? role)
        {
            var users = await _unitOfWork.UserRepository.Get(c=>c.Status != CommonEnums.USER_STATUS.DELETED);
            if (!string.IsNullOrEmpty(searchTerm) || role != null)
            {
                users = SearchUsers(searchTerm,role,users.ToList());
            }
            List<UserListResponse> res = users.Select(
                user =>
                {
                    return new UserListResponse()
                    {
                        Id = user.Id,
                        FullName = user.Name,
                        Address = user.Address,
                        AvatarURL= user.AvatarURL,
                        Birthday = user.Birthday,
                        Email = user.Email,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        Status= user.Status,
                        Role = user.Role
                    };
                }
                ).ToList();

            int totalItem = res.Count;

            res = res.Skip((paging.PageIndex - 1) * paging.PageSize)
                    .Take(paging.PageSize).ToList();

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

        public async Task<UserDetailResponse> GetUserDetail(int id)
        {
           try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Status!=CommonEnums.USER_STATUS.DELETED && c.Id==id);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"User not found");
                }
                var userdetail = new UserDetailResponse()
                {
                    Id = id,
                    Email = user.Email,
                    Address = user.Address,
                    Birthday = user.Birthday,
                    FullName=user.Name,
                    Gender= user.Gender,
                    PhoneNumber= user.PhoneNumber,
                    AvatarUrl= user.AvatarURL,
                    CreatedAt= user.CreatedAt,
                    UpdatedAt= user.UpdatedAt,
                    Status = user.Status,
                    Role = user.Role                   
                };
                return userdetail;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
