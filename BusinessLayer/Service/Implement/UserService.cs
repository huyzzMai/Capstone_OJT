using BusinessLayer.Payload.RequestModel;
using BusinessLayer.Payload.RequestModel.AuthenticationRequest;
using BusinessLayer.Payload.RequestModel.UserRequest;
using BusinessLayer.Payload.ResponseModel;
using BusinessLayer.Payload.ResponseModel.AuthenticationResponse;
using BusinessLayer.Payload.ResponseModel.UserResponse;
using BusinessLayer.Service.Interface;
using BusinessLayer.Utilities;
using DataAccessLayer.Commons;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrelloDotNet;
using TrelloDotNet.Model;

//using static BusinessLayer.Models.ResponseModel.UserResponse.PersonalUserResponse;

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

        public string GenerateRandomCharacter()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();

            int index1 = random.Next(0, alphabet.Length);
            int index2 = random.Next(0, alphabet.Length);

            char char1 = alphabet[index1];
            char char2 = alphabet[index2];

            string randomString = $"{char1}{char2}";
            return randomString;
        }

        public async Task SendTokenResetPassword(string email)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByEmailAndStatusActive(email);
                if (u == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "User not found!");
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
                sender.Send(email, u.FirstName, r);
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
                throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Reset code is not correct!");
            }
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByResetCodeAndStatusActive(request.ResetCode);
                if (u == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Reset code is not correct! Please go back to resend verification code.");
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
            //authHeader = authHeader.Replace("Bearer ", "");
            if (authHeader.Contains("Bearer "))
            {
                authHeader = authHeader.Replace("Bearer ", "");
            }
            else if (authHeader.Contains("bearer "))
            {
                authHeader = authHeader.Replace("bearer ", "");
            }
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,   
                    Email = user.Email,
                    Address = user.Address,
                    AvatarURL = user.AvatarURL,
                    Birthday = user.Birthday ?? default(DateTime),
                    Gender = user.Gender ?? default(int),
                    PhoneNumber = user.PhoneNumber,
                    RollNumber = user.RollNumber,
                    StudentCode = user.StudentCode
                };

                if (user.Position != null)
                {
                    result.PositionName = user.Position.Name;
                }

                if (user.Trainer != null)
                {
                    result.TrainerResponse.TrainerName = user.Trainer.LastName + user.Trainer.FirstName;
                    result.TrainerResponse.TrainerEmail = user.Trainer.Email;   
                    result.TrainerResponse.AvatarURL = user.Trainer.AvatarURL;  
                    result.TrainerResponse.TrainerPhoneNumber = user.Trainer.PhoneNumber;   
                }

                var listSkill = new List<PersonalUserResponse.PersonalSkillResponse>();

                foreach (var sa in user.UserSkills)
                {
                    var s = new PersonalUserResponse.PersonalSkillResponse();
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    AvatarURL = user.AvatarURL,
                    Birthday = DateTimeService.ConvertToDateString(user.Birthday),
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    RollNumber = user.RollNumber,
                    //Position = user.Position,
                    TrelloId = user.TrelloId,
                    CreatedAt = DateTimeService.ConvertToDateString(user.CreatedAt),
                    UpdatedAt = DateTimeService.ConvertToDateString(user.UpdatedAt)
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
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Gender = user.Gender ?? default(int),
                            AvatarURL = user.AvatarURL,
                            RollNumber = user.RollNumber,
                            PositionName = user.Position.Name,
                            Status = user.Status
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
                if (trainer == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainer not found!");
                }
                TrainerResponse res = new()
                {
                    Id = trainer.Id,
                    FirstName = trainer.FirstName,
                    LastName = trainer.LastName,
                    Email = trainer.Email,
                    Gender = trainer.Gender ?? default(int),
                    AvatarURL = trainer.AvatarURL,
                    RollNumber = trainer.RollNumber,
                    PositionName = trainer.Position.Name,
                    PhoneNumber = trainer.PhoneNumber,
                    Address = trainer.Address,
                    Birthday = trainer.Birthday ?? default,
                    Status = trainer.Status
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
            var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
            List<TraineeResponse> res = new(); 
            foreach (var user in users)
            {
                TraineeResponse a = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender ?? default(int),
                    AvatarURL = user.AvatarURL,
                    RollNumber = user.RollNumber,   
                    PositionName = user.Position.Name,
                    Status = user.Status
                };
                List<Card> listTask = new();
                if (user.TrelloId != null)
                {
                    listTask = await client.GetCardsForMemberAsync(user.TrelloId);
                }
                if (listTask != null && listTask.Count != 0)
                {
                    if (listTask.Any(task => task.DueComplete == false && task.Due != null))
                    {
                        a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.WORKING;
                    }
                    else
                    {
                        a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.FREE;
                    }
                }
                else
                {
                    a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.FREE;
                }
                res.Add(a); 
            }
                
                //users.Select(
                //async user =>
                //{
                //    return new TraineeResponse()
                //    {
                //        Id = user.Id,
                //        FirstName = user.FirstName,
                //        LastName = user.LastName,
                //        Email = user.Email,
                //        Gender = user.Gender ?? default(int),
                //        AvatarURL = user.AvatarURL,
                //        PositionName = user.Position.Name,
                //        Status = user.Status,
                //        WorkStatus = nul 
                //    };
                //    var listTask = await client.GetCardsForMemberAsync(user.TrelloId);
                //    if (listTask != null && listTask.Count != 0)
                //    {
                //        if (listTask.Any(task => task.DueComplete == false))
                //        {

                //        }
                //    }
                //    else
                //    {

                //    }
                //}
                //).ToList();

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
            var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
            List<TraineeResponse> res = new();
            foreach (var user in users)
            {
                TraineeResponse a = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender ?? default(int),
                    AvatarURL = user.AvatarURL,
                    RollNumber = user.RollNumber,
                    PositionName = user.Position.Name,
                    Status = user.Status
                };
                List<Card> listTask = new();
                if (user.TrelloId != null)
                {
                    listTask = await client.GetCardsForMemberAsync(user.TrelloId);
                }
                if (listTask != null && listTask.Count != 0)
                {
                    if (listTask.Any(task => task.DueComplete == false && task.Due != null))
                    {
                        a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.WORKING;
                    }
                    else
                    {
                        a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.FREE;
                    }
                }
                else
                {
                    a.WorkStatus = CommonEnums.TRAINEE_WORKING_STATUS.FREE;
                }
                res.Add(a);
            }

            //List<TraineeResponse> res = users.Select(
            //    user =>
            //    {
            //        return new TraineeResponse()
            //        {
            //            Id = user.Id,
            //            FirstName = user.FirstName,
            //            LastName = user.LastName,
            //            Email = user.Email,
            //            Gender = user.Gender ?? default(int),
            //            AvatarURL = user.AvatarURL,
            //            PositionName = user.Position.Name,
            //            Status = user.Status
            //        };
            //    }
            //    ).ToList();

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

        public async Task<List<UnassignedTraineeResponse>> GetUnassignedTraineeList()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetUnassignedTraineeList();
                List<UnassignedTraineeResponse> res = users.Select(
                user =>
                {
                    return new UnassignedTraineeResponse()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        AvatarURL = user.AvatarURL,
                        PositionName = user.Position.Name
                    };
                }
                ).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PersonalTraineeResponse> GetTraineeDetail(int roleId, int traineeId)
        {
            try
            {
                var check = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(roleId);
                if (check == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Current Login User not found!");
                }
                if (check.Role == CommonEnums.ROLE.MANAGER)
                {
                    var trainee = await _unitOfWork.UserRepository.GetUserByIdWithSkillList(traineeId);
                    if (trainee == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainee not found!");
                    }
                    PersonalTraineeResponse res = new()
                    {
                        Id = trainee.Id,
                        FirstName = trainee.FirstName,
                        LastName = trainee.LastName,
                        Email = trainee.Email,
                        AvatarURL = trainee.AvatarURL,
                        Gender = trainee.Gender ?? default(int),
                        PositionName = trainee.Position.Name,
                        RollNumber = trainee.RollNumber,
                        PhoneNumber = trainee.PhoneNumber,
                        Address = trainee.Address,
                        Birthday = trainee.Birthday ?? default,
                        Status = trainee.Status,
                        TrainerName = trainee.Trainer.LastName + trainee.Trainer.FirstName,
                        TrainerEmail = trainee.Trainer.Email
                    };

                    var listSkill = new List<PersonalTraineeResponse.PersonalSkillResponse>();

                    foreach (var sa in trainee.UserSkills)
                    {
                        var s = new PersonalTraineeResponse.PersonalSkillResponse();
                        s.Name = sa.Skill.Name;
                        //s.Type = sa.Skill.Type;
                        s.CurrentLevel = sa.CurrentLevel ?? default(int);
                        listSkill.Add(s);
                    }
                    res.Skills = listSkill;

                    return res; 
                }
                else
                {
                    var trainee = await _unitOfWork.UserRepository.GetUserByIdWithSkillList(traineeId);
                    if(trainee == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Trainee not found!");
                    }
                    if (trainee.UserReferenceId != roleId)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Not your assigned trainee!");
                    }

                    PersonalTraineeResponse res = new()
                    {
                        Id = trainee.Id,
                        FirstName = trainee.FirstName,
                        LastName = trainee.LastName,
                        Email = trainee.Email,
                        AvatarURL = trainee.AvatarURL,
                        Gender = trainee.Gender ?? default(int),
                        PositionName = trainee.Position.Name,
                        RollNumber = trainee.RollNumber,
                        PhoneNumber = trainee.PhoneNumber,
                        Address = trainee.Address,
                        Birthday = trainee.Birthday ?? default,
                        Status = trainee.Status
                    };

                    var listSkill = new List<PersonalTraineeResponse.PersonalSkillResponse>();

                    foreach (var sa in trainee.UserSkills)
                    {
                        var s = new PersonalTraineeResponse.PersonalSkillResponse();
                        s.Name = sa.Skill.Name;
                        //s.Type = sa.Skill.Type;
                        s.CurrentLevel = sa.CurrentLevel ?? default(int);
                        listSkill.Add(s);
                    }
                    res.Skills = listSkill;

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
                if (request.Trainees == null || request.Trainees.Count == 0)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "List of trainees is not found!");
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

        public async Task CreateUserCriteria(int userid,int batchId)
        {
            var currentdate= DateTimeService.GetCurrentDateTime();
            var ojtbatch = await _unitOfWork.OJTBatchRepository.GetFirst(c=>c.Id==batchId 
            && c.StartTime.Value.Date.AddDays(10) >= currentdate.Date);
            var template = await _unitOfWork.TemplateRepository.GetFirst(c=>c.Id == ojtbatch.TemplateId 
            && c.Status==CommonEnums.TEMPLATE_STATUS.ACTIVE, "TemplateHeaders");
            if(template == null)
            {
                throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Template not found");
            }
            foreach (var item in template.TemplateHeaders)
            {
                if(item.IsCriteria==true && item.Status==CommonEnums.TEMPLATEHEADER_STATUS.ACTIVE)
                {
                    var usercriteria = new UserCriteria()
                    {
                        UserId = userid,
                        TemplateHeaderId = item.Id,
                        UpdatedDate = DateTime.UtcNow.AddHours(7),
                        CreatedDate = DateTime.UtcNow.AddHours(7),
                    };
                    await _unitOfWork.UserCriteriaRepository.Add(usercriteria);
                }            
            }      
        }
        public async Task CreateUser(CreateUserRequest request)
        {
            try
            {
                var emailCheck = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);
                if (emailCheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Email has been used!");
                }

                String r = GenerateRamdomCode();
                string pwd = GenerateRandomCharacter() + r;
                string encryptPwd = EncryptPassword(pwd);

                var rollNumberCheck = await _unitOfWork.UserRepository.GetUserByRollNumber(request.RollNumber);
                if (rollNumberCheck != null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Roll number has been used!");
                }

                var currentdate = DateTimeService.GetCurrentDateTime();
                var ojtbatch = await _unitOfWork.OJTBatchRepository.GetFirst(c => c.Id == request.BatchId
                && c.StartTime.Value.Date.AddDays(10) >= currentdate.Date);
                if (ojtbatch == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Invalid ojt bacth");
                }

                User user = new User()
                {
                    Role = request.Role,
                    FirstName = request.FirstName,  
                    LastName = request.LastName,
                    Email = request.Email,
                    Birthday = request.Birthday,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Gender = request.Gender,
                    RollNumber = request.RollNumber,
                    AvatarURL = request.AvatarUrl,
                    Password = encryptPwd,
                    PositionId = request.Position,
                    StudentCode = request.StudentCode,  
                    OJTBatchId = request.BatchId,
                    //TrelloId = request.TrelloId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    Status = CommonEnums.USER_STATUS.ACTIVE
                };

                if (user.Role == CommonEnums.ROLE.TRAINEE || user.Role == CommonEnums.ROLE.TRAINER)
                {
                    if (user.PositionId == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Trainee or Trainer must have a Position!");
                    }
                    var positionCheck = await _unitOfWork.PositionRepository.GetFirst(s => s.Id == request.Position && s.Status == CommonEnums.POSITION_STATUS.ACTIVE);
                    if (positionCheck == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Position invalid!");
                    }
                    var client = new TrelloClient(_configuration["TrelloWorkspace:ApiKey"], _configuration["TrelloWorkspace:token"]);
                    var trelloUsers = await client.GetMembersOfOrganizationAsync(_configuration["TrelloWorkspace:WorkspaceId"]); 
                    //var target = trelloUsers.FirstOrDefault(x => x.Username == user.RollNumber);
                    var target = trelloUsers.FirstOrDefault(x => string.Equals(x.Username, user.RollNumber, StringComparison.OrdinalIgnoreCase));
                    if (target == null)
                    {
                        throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "Trainee or Trainer had not been created a Trello Accout!");
                    }
                    user.TrelloId = target.Id; 
                }
               
                if (request.CreateSkills != null && request.CreateSkills.Count != 0) 
                {
                    ICollection<UserSkill> re = new List<UserSkill>();
                    foreach (var skillRequest in request.CreateSkills)
                    {
                        var check = await _unitOfWork.SkillRepository.GetFirst(s => s.Id == skillRequest.SkillId && s.Status == CommonEnums.SKILL_STATUS.ACTIVE);
                        if (check == null)
                        {
                            throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "Skill not found!");
                        }
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
                }
                  
                await _unitOfWork.UserRepository.Add(user);
                if (user.Role == CommonEnums.ROLE.TRAINEE)
                {
                    await CreateUserCriteria(user.Id, (int)user.OJTBatchId);
                }
                var sender = new MailSender();
                sender.SendMailCreateAccount(user.Email, user.FirstName, user.Email, pwd);
            }
            catch (ApiException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserPassword(int id, UpdateUserPasswordRequest model)
        {
            try
            {
                var u = await _unitOfWork.UserRepository.GetUserByIdAndStatusActive(id);    
                if (u == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.NOT_FOUND, "This user cannot be found!");
                }

                var realOldPassword = DecryptPassword(u.Password);  
                if (model.OldPassword != realOldPassword)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "The old password is not correct!");
                }
                var encryptPassword = EncryptPassword(model.NewPassord);
                u.Password = encryptPassword;
                u.UpdatedAt = DateTime.UtcNow.AddHours(7);
                await _unitOfWork.UserRepository.Update(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserInformation(int id, UpdateUserInformationRequest model)
        {
            try
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

                if (model.FirstName != null)
                {
                    u.FirstName = model.FirstName;
                }

                if (model.LastName != null)
                {
                    u.LastName = model.LastName;
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<User> SearchUsers(string searchTerm, int? role,int? filterStatus,List<User> userList)
        {
            var query = userList.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                query = query.Where(c =>
                    (c.FirstName != null && c.FirstName.ToLower().Contains(searchTerm)) ||
                    (c.LastName != null && c.LastName.ToLower().Contains(searchTerm)) ||
                    (c.Address != null && c.Address.ToLower().Contains(searchTerm)) ||
                   (c.Email != null && c.Email.ToLower().Contains(searchTerm)) ||
                   (c.RollNumber != null && c.RollNumber.ToLower().Contains(searchTerm))
               );
            }

            if (role != null)
            {
                query = query.Where(c => c.Role == role);
            }
            if (filterStatus != null)
            {
                query = query.Where(c => c.Status == filterStatus);
            }

            return query.ToList();
        }


        public async Task<BasePagingViewModel<UserListResponse>> GetUserList(PagingRequestModel paging, string searchTerm, int? role, int? filterStatus)
        {
            var users = await _unitOfWork.UserRepository.Get();
            if (!string.IsNullOrEmpty(searchTerm) || role != null || filterStatus !=null)
            {
                users = SearchUsers(searchTerm,role,filterStatus,users.ToList());
            }
            List<UserListResponse> res = users.Select(
                user =>
                {
                    return new UserListResponse()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        AvatarURL= user.AvatarURL,
                        Birthday = DateTimeService.ConvertToDateString(user.Birthday),
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
                var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==id);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET,"User not found");
                }
                var userdetail = new UserDetailResponse()
                {
                    Id = id,
                    Email = user.Email,
                    Address = user.Address,
                    Birthday = DateTimeService.ConvertToDateString(user.Birthday),
                    FirstName=user.FirstName,
                    LastName =user.LastName,    
                    Gender= user.Gender,
                    PhoneNumber= user.PhoneNumber,
                    AvatarUrl= user.AvatarURL,
                    CreatedAt= DateTimeService.ConvertToDateString(user.CreatedAt),
                    UpdatedAt= DateTimeService.ConvertToDateString(user.UpdatedAt),
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

        public async Task ActiveUser(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==id);
                if(user==null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User not found");
                }
                user.Status = CommonEnums.USER_STATUS.ACTIVE;
                await _unitOfWork.UserRepository.Update(user);
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

        public async Task DisableUser(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetFirst(c => c.Id == id);
                if (user == null)
                {
                    throw new ApiException(CommonEnums.CLIENT_ERROR.BAD_REQUET, "User not found");
                }
                user.Status = CommonEnums.USER_STATUS.INACTIVE;
                await _unitOfWork.UserRepository.Update(user);
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
