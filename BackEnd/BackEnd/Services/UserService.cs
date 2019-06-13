using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        Task<AuthedUser> AuthenticateAsync(string username, string password);
        User GetById(int id);
        IEnumerable<User> GetAll();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        User CreateUser(User user, string password);
        bool UsernameExists(string v);
        Task<bool> PersistUser(User user);
    }

    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public async Task<AuthedUser> AuthenticateAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username );

            // return null if user not found
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var authedUser = new AuthedUser { UserName=user.Username, FirstName=user.FirstName,LastName=user.LastName,Email=user.Email, Role=user.Role };
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            authedUser.Token = tokenHandler.WriteToken(token);
            return authedUser;

        }

        public User GetById(int id) {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            // return user without password
            if (user != null) 
                user.PasswordHash = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _dbContext.Users.ToList();
            return users.Select(u => { u.PasswordHash = null; return u; });
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }


        public User CreateUser(User user, string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return user;
        }


        public bool UsernameExists(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username) != null;
        }

        public async Task<bool> PersistUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception )
            {
                return false;
            }

           
        }

    }
}