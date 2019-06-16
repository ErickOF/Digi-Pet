using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class PetDto
    {
        public PetDto()
        {

        }
        public PetDto(Pet pet)
        {
            Id = pet.Id;
            Name = pet.Name;
            Race = pet.Race;
            Age = pet.Age;
            Size = pet.Size;
            Photos = pet.Photos;
            DateCreated = pet.DateCreated;
            Trips = pet.WalksQuant;
        }
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]
        public string Race { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public string Size { get; set; }//S,M,L,XL
        [Required]
        public string Description { get; set; }

        public string[] Photos { get; set; }

        public int Trips { get; set; }
        public DateTime DateCreated { get; internal set; }
    }
}
