using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.CustomAttributes;

namespace WebApi.Model
{
    public class WalkerSchedule
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int WalkerId { get; set; }
        public virtual Walker Walker { get; set; }

        [Required,DataType(DataType.Date),Column(TypeName ="DATE")]
        public DateTime Date { get; set; }
        [Required,ValidateHoursAvailable]
        public int[] HoursAvailable { get; set; }//{}

    }
}
