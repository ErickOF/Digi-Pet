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
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]
        public string Race { get; set; }

        public int WalksQuant { get
            {
                return Walks.Count;
            }
        }

        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public int Age { get; set; }
        public string Size { get; set; }//S,M,L,XL
        public string Description { get; set; }

        [Required]
        public int PetOwnerId { get; set; }
        public virtual Petowner Petowner { get; set; }

        public ICollection<Walk> Walks { get; set; }
    }
}
