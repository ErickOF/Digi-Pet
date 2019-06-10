using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Pet
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(25)]
        public string Name { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }

        [Required]
        public int PetOwnerId { get; set; }
        public virtual Petowner Petowner { get; set; }
    }
}
