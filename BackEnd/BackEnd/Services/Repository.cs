using Microsoft.AspNetCore.Mvc;
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
        Task<Tuple<Pet, string>> CreatePet(PetDto petDto, int ownerId);
        Task<Petowner> GetOwnerByUserName(string username);
        Task<IEnumerable<Petowner>> GetOwners();
        Task<Petowner> GetOwner(int id);
        Task<Walker> GetWalkerByUserName(string username);
    }
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public Repository(ApplicationDbContext dbContext,IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        public async Task<Tuple<Walker,string>> CreateWalker(WalkerDto walkerDto)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    var walker = new Walker
                    {
                        User = _userService.CreateUser( new Entities.User
                        {
                            Username = walkerDto.SchoolId,
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
                        }, walkerDto.Password),
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
                        pets.Add(new Pet { Name = pet.Name, Race=pet.Race,DateCreated=DateTime.UtcNow,Age=pet.Age, Size=pet.Size, Description=pet.Description,Photos=pet.Photos});
                    }

                    var owner = new Petowner
                    {
                        User = _userService.CreateUser(new Entities.User
                        {
                            Username = ownerDto.Email,
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
                        },ownerDto.Password),
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
        public async Task<Petowner> GetOwnerByUserName(string username)
        {
            return await _dbContext.Owners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .ThenInclude(p=>p.Walks)
                .FirstOrDefaultAsync(u => u.User.Username == username);
        }

        public async Task<Tuple<Pet, string>> CreatePet(PetDto petDto, int ownerId)
        {
            var pet = new Pet
            {
                Name = petDto.Name,
                Race = petDto.Race,
                Age = petDto.Age,
                Size = petDto.Size,
                Description = petDto.Description,
                Photos = petDto.Photos,
                PetOwnerId = ownerId,
                DateCreated= DateTime.UtcNow
            };
            try
            {
                _dbContext.Add(pet);
                await _dbContext.SaveChangesAsync();
                return new Tuple<Pet, string>(pet, "success");
            }
            catch (DbUpdateException e)
            {
                return new Tuple<Pet, string>(null, $"error: {e.InnerException.Message}");
            }
            
        }


        public async Task<IEnumerable<Petowner>> GetOwners()
        {
            var owners= await _dbContext.Owners.Include(o=>o.User).ToListAsync();

            return owners.Select(u => { u.User.PasswordHash = null; return u; });
        }

        public async Task<Petowner> GetOwner(int id)
        {
            return await _dbContext.Owners.Include(o => o.Pets).Include(o=>o.User).FirstOrDefaultAsync(u => u.Id==id);
        }



        public async Task<Walker> GetWalkerByUserName(string username)
        {
            return await _dbContext.Walker
                .Include(o => o.User)
                .Include(w=>w.Walks).ThenInclude(w=>w.ReportWalks)
                .FirstOrDefaultAsync(u => u.User.Username == username);
        }
    }
}
