using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.CustomAttributes;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class WeekScheduleDto
    {
        [Required,ValidateDateRangeWalkerSchedule]
        public ICollection<ScheduleDto> Week { get; set; }
    }

    public class ScheduleDto
    {

        [Required,DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required,ValidateHoursAvailable]
        public int[] HoursAvailable { get; set; }
    }
}
