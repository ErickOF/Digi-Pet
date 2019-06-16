using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Services;

namespace WebApi.CustomAttributes
{
    public class ValidateDate : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _configSrvc = (IConfigurationService)validationContext.GetService(typeof(IConfigurationService));

            if (value == null) return new ValidationResult("object null");
            var MinutesBeforeAskingService = _configSrvc.GetValue<int>(ConfigurationValues.MinimumMinutesBeforeAskingService);

            var requestedDate = value as DateTime?;

            if (requestedDate >= DateTime.UtcNow.AddMinutes(MinutesBeforeAskingService))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"need to ask service at least {MinutesBeforeAskingService} before Time");
            
        }
    }
}
