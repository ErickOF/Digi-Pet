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
        Task<bool> CreateWalker( WalkerDto walkerDto);
    }
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateWalker(WalkerDto walkerDto)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var walker = new Walker
                    {
                        User = new Entities.User { Username = walkerDto.Id, Password = walkerDto.Password,Role=Role.Walker, FirstName = walkerDto.Name, LastName= walkerDto.LastName,
                        Province=walkerDto.Province, Canton = walkerDto.Canton, Email = walkerDto.Email, Email2 = walkerDto.Email2, Mobile = walkerDto.Mobile,
                        DateCreated = DateTime.UtcNow},
                        
                        DoesOtherProvinces = walkerDto.DoesOtherProvinces,
                        OtherProvinces = walkerDto.OtherProvinces
                        
                    };
                    _dbContext.Walker.Add(walker);
                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
    }
}
