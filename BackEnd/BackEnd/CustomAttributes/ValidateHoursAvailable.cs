using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.CustomAttributes
{
    public class ValidateHoursAvailable : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var hours = (int[])value;
            foreach (var hour in hours)
            {
                if (hour < 0 || hour > 23) return false;
            }
            return true;
            
        }

        public override string FormatErrorMessage(string name)
        {
            return $"the field {name} must be between 0-23";
        }
    }



}
