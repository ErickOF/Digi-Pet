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
        Task<Tuple<Petowner, string>> CreateOwner(OwnerDto walkerDto);
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
                            Username = walkerDto.SchoolId,
                            Password = walkerDto.Password,
                            Role = Role.Walker,
                            FirstName = walkerDto.Name,
                            LastName = walkerDto.LastName,
                            Province = walkerDto.Province,
                            Canton = walkerDto.Canton,
                            Email = walkerDto.Email,
                            Email2 = walkerDto.Email2,
                            Mobile = walkerDto.Mobile,
                            Description = walkerDto.Description,
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

        public async Task<Tuple<Petowner, string>> CreateOwner(OwnerDto ownerDto)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var pets = new List<Pet>();
                    foreach (var pet in ownerDto.Pets)
                    {
                        pets.Add(new Pet { Name = pet.Name, Race=pet.Race,DateCreated=DateTime.UtcNow,Age=pet.Age, Size=pet.Size, Description=pet.Description});
                    }
                    var owner = new Petowner
                    {
                        User = new Entities.User
                        {
                            Username = ownerDto.Email,
                            Password = ownerDto.Password,
                            Role = Role.Petowner,
                            FirstName = ownerDto.Name,
                            LastName = ownerDto.LastName,
                            Province = ownerDto.Province,
                            Canton = ownerDto.Canton,
                            Email = ownerDto.Email,
                            Email2 = ownerDto.Email2,
                            Mobile = ownerDto.Mobile,
                            DateCreated = DateTime.UtcNow,
                            Description = ownerDto.Description
                        },
                        Pets = pets

                    };
                    _dbContext.Owners.Add(owner);
                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return new Tuple<Petowner, string>(owner, "success");
                }
                catch (DbUpdateException e)
                {
                    trans.Rollback();
                    return new Tuple<Petowner, string>(null, e.InnerException.Message);
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
