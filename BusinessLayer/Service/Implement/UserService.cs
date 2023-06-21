using BusinessLayer.Models.RequestModel.AuthenticationRequest;
using BusinessLayer.Models.ResponseModel.AuthenticationResponse;
using BusinessLayer.Service.Interface;
using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
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
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
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

        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            User u = await _unitOfWork.UserRepository.GetUserByEmailAndDeleteIsFalse(request.Email);
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

            if (u.Role == 1)
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
            return null;
            //else if (u.Role == CommonEnums.ROLE.FIXER)
            //{
            //    var claims = new[]
            //        {
            //        new Claim(ClaimTypes.Role, "Fixer"),
            //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //        new Claim(JwtRegisteredClaimNames.NameId, userId)
            //        };
            //    var result = CreateToken(claims);
            //    return result;
            //}
            //else
            //{
            //    var claims = new[]
            //    {
            //        new Claim(ClaimTypes.Role, "User"),
            //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //        new Claim(JwtRegisteredClaimNames.NameId, userId)
            //        };
            //    var result = CreateToken(claims);
            //    return result;
            //}
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetFirst(c=>c.Id==id);
            return user;
        }
    }
}
