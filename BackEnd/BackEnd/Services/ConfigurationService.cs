using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Model;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IConfigurationService
    {
        T GetValue<T>(string key);
        Task<Tuple<string, string>> PutProperty(string key, string defaultValue);
        Task<Tuple<string, string>> ModifyProperty(string key, string value);
        Task AddConfig(AppSettings value);
    }
    public class ConfigurationService: IConfigurationService
    {
        private readonly ApplicationDbContext _dbContext;

        public ConfigurationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public T GetValue<T>(string key)
        {
            var res =  _dbContext.Properties.FirstOrDefault(e => e.Property == key);
            if (res != null)
            {
                return (T)Convert.ChangeType(res.Value, typeof(T));
            }
            else
            {
                return default(T);
            }

        }



        public async Task<Tuple<string,string>> PutProperty(string key, string defaultValue)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Properties.Add(new AppConfiguration { Property = key, Value = defaultValue });
                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return new Tuple<string, string>(key,defaultValue);

                }
                catch
                {
                    trans.Rollback();
                    return null;
                }
            }
        }

        public async Task<Tuple<string, string>> ModifyProperty(string key, string value)
        {
            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var prop = await _dbContext.Properties.FirstOrDefaultAsync(p => p.Property == key);
                    if (prop == null)
                    {
                        throw new Exception($"{key} not found");
                    }
                    prop.Value = value;
                    _dbContext.Attach(prop).State = EntityState.Modified;

                    await _dbContext.SaveChangesAsync();
                    trans.Commit();
                    return new Tuple<string, string>(key, value);

                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return new Tuple<string,string>("error",e.Message);
                }
            }
        }

        public async Task AddConfig(AppSettings appSettings)
        {
            Type t = appSettings.GetType();
            var props = t.GetProperties();

            foreach (var prop in props)
            {
                var name = prop.Name;
                string value = prop.GetValue(appSettings) as string;
                if (name == "Secret") continue;
                try
                {
                    var obj= await _dbContext.Properties.FirstOrDefaultAsync(p=>p.Property==name);
                    if (obj != null) continue;
                    await PutProperty(name, value);
                }
                catch
                {

                    
                }
            }
        }
    }

    public class ConfigurationValues
    {
        public const string MinimumMinutesBeforeAskingService = "MinimumMinutesBeforeAskingService";
        public const string HourPriceWalkUSD = "HourPriceWalkUSD";
        public const string MinimumUpdateSheduleHours = "MinimumUpdateSheduleHours";
        public const string MaximumUpdateSheduleHours = "MaximumUpdateSheduleHours";
    }
}
