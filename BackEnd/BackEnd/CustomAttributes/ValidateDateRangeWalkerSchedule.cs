using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.CustomAttributes
{
    public class ValidateDateRangeWalkerSchedule : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage($"no se puede actualizar estas fechas, ver politicas");
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _configSrvc = (IConfigurationService)validationContext.GetService(typeof(IConfigurationService));

            var minimumUpdateSheduleHours = _configSrvc.GetValue<int>(ConfigurationValues.MinimumUpdateSheduleHours);
            var maximumUpdateSheduleHours = _configSrvc.GetValue<int>(ConfigurationValues.MaximumUpdateSheduleHours);
            var schedules = (List<ScheduleDto>) value;

            if (schedules.Count == 0) return new ValidationResult("list is empty");
            schedules = schedules.OrderBy(s => s.Date).ToList();
            //validate minimum policy
            var closest = schedules.First();
            var minHour = closest.HoursAvailable.Min();
            if (closest.Date.Date.AddHours(minHour) < DateTime.UtcNow.AddHours(minimumUpdateSheduleHours))
                return new ValidationResult($"need at least {minimumUpdateSheduleHours} hours to update day");

            //validate maximum policy
            var furthest = schedules.Last();
            var maxHour = furthest.HoursAvailable.Max();
            if (furthest.Date.Date.AddHours(maxHour) > DateTime.UtcNow.AddHours(maximumUpdateSheduleHours))
                return new ValidationResult($"cant update beyond {maximumUpdateSheduleHours} hours");

            return ValidationResult.Success;
        }
    }
}
