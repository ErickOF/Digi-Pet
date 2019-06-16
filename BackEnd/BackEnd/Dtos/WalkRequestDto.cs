using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.CustomAttributes;

namespace WebApi.Dtos
{
    public class WalkRequestDto
    {
        [Required]
        public int PetId { get; set; }

        [Required, DataType(DataType.DateTime), ValidateDate]
        public DateTime Begin { get; set; }

        [Required, Range(0.5, 8.0)]
        public decimal Duration { get; set; }

        [Required]
        public string Province { get; set; }
        [Required]
        public string Canton { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string ExactAddress { get; set; }

        public virtual DateTime End { get
            {
                return Begin.AddHours((double)Duration);
            }

        }

    }
}
