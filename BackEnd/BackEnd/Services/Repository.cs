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
        Task<ReturnOwner> GetOwnerByUserName(string username);
        Task<IEnumerable<ReturnOwner>> GetOwners();
        Task<ReturnOwner> GetOwner(int id);
        Task<Walker> GetWalkerByUserName(string username);
        Task<Tuple<WalkInfoDto,string>> RequestWalk(WalkRequestDto walkRequestDto);
        Task<ActionResult<IEnumerable<ReturnWalker>>> GetAllWalkers();
        Task<string> AddSchedule(WeekScheduleDto weekScheduleDto, string username);
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
                            Photo = walkerDto.Photo,
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
                            Description = ownerDto.Description,
                            Photo=ownerDto.Photo
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
        public async Task<ReturnOwner> GetOwnerByUserName(string username)
        {
            var owner= await _dbContext.Owners
                .Include(o => o.User)
                .Include(o => o.Pets)
                .ThenInclude(p=>p.Walks)
                .FirstOrDefaultAsync(u => u.User.Username == username);

            return new ReturnOwner (owner);
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


        public async Task<IEnumerable<ReturnOwner>> GetOwners()
        {
            var owners= await _dbContext.Owners.Include(o=>o.User).Include(o=>o.Pets).ToListAsync();

            return owners.Select(u => new ReturnOwner(u));
        }

        public async Task<ReturnOwner> GetOwner(int id)
        {
            var owner= await _dbContext.Owners.Include(o => o.Pets).Include(o=>o.User).FirstOrDefaultAsync(u => u.Id==id);

            return new ReturnOwner(owner);

        }



        public async Task<Walker> GetWalkerByUserName(string username)
        {
            return await _dbContext.Walker
                .Include(o => o.User)
                .Include(w=>w.Walks).ThenInclude(w=>w.ReportWalks)
                .FirstOrDefaultAsync(u => u.User.Username == username);
        }

        public async Task<Tuple<WalkInfoDto, string>> RequestWalk(WalkRequestDto walkRequestDto)
        {
            var walkers =  _dbContext.Walker.Include(w => w.User).Include(w=>w.Walks).ThenInclude(wa=>wa.ReportWalks)
                .Where(w => w.User.Province == walkRequestDto.Province || (w.DoesOtherProvinces && w.OtherProvinces.Contains(walkRequestDto.Province)) &&
                !w.Blocked
           ).AsNoTracking();
            // se le asignan los 10 puntos adicionales si es del canton
            await walkers.ForEachAsync(w=> w.TempPoints=w.Points+((w.User.Canton==walkRequestDto.Canton && w.User.Province == walkRequestDto.Province)?10:0));
            walkers.OrderByDescending(w => w.TempPoints);


            return null;
        }

        public async Task<ActionResult<IEnumerable<ReturnWalker>>> GetAllWalkers()
        {
            return await _dbContext.Walker.Include(w => w.User).Include(w=>w.Walks)
                .Select(u => new ReturnWalker(u))
                .ToListAsync();
        }

        public async Task<string> AddSchedule(WeekScheduleDto weekScheduleDto, string username)
        {
            var walker = await GetWalkerByUserName(username);
            if (walker == null) return "walker not found";

            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var scheds = weekScheduleDto.Week.Select(s =>
                    {
                        var existing = _dbContext.WalkerSchedule.FirstOrDefault(e => e.WalkerId == walker.Id && e.Date.Date == s.Date.Date);
                        if (existing != null)
                        {
                            existing.HoursAvailable = s.HoursAvailable;
                            return existing;
                        }
                        else
                        {
                            return new WalkerSchedule
                            {
                                Id = 0,
                                WalkerId = walker.Id,
                                Date = s.Date.Date,
                                HoursAvailable = s.HoursAvailable
                            };
                        }


                    }).ToList();
                    var schedsCopy = scheds.ToList();

                    scheds.RemoveAll(sch => sch.Id==0);
                    scheds.ForEach(e => { _dbContext.Attach(e).State = EntityState.Modified; });

                    schedsCopy.RemoveAll(sch => sch.Id!=0);

                    await _dbContext.WalkerSchedule.AddRangeAsync(schedsCopy);
                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return e.Message;                   
                }
            }
        }
    }
}
