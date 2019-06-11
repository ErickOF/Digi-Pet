using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Model;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IRepository
    {
        Task<Tuple<Walker, string>> CreateWalker( WalkerDto walkerDto);
        Task<User> CreateUser(User user);
        bool UsernameExists(string username);
    }
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Tuple<Walker,string>> CreateWalker(WalkerDto walkerDto)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var walker = new Walker
                    {
                        User = new Entities.User
                        {
                            Username = walkerDto.SchoolId, Password = walkerDto.Password,Role=Role.Walker, FirstName = walkerDto.Name, LastName= walkerDto.LastName,
                            Province=walkerDto.Province, Canton = walkerDto.Canton, Email = walkerDto.Email, Email2 = walkerDto.Email2, Mobile = walkerDto.Mobile,
                            DateCreated = DateTime.UtcNow
                        },
                        University = walkerDto.University,
                        DoesOtherProvinces = walkerDto.DoesOtherProvinces,
                        OtherProvinces = walkerDto.OtherProvinces
                        
                    };
                    _dbContext.Walker.Add(walker);
                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return new Tuple<Walker,string>(walker,"success");
                }
                catch (DbUpdateException e)
                {
                    trans.Rollback();
                    return new Tuple<Walker,string>(null,e.InnerException.Message);
                }
            }
        }
        public async Task<User> CreateUser(User user)
        {
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public bool UsernameExists(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username) != null;
        }
    }
}
