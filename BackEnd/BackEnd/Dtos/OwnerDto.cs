﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class OwnerDto
    {
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Canton { get; set; }

        [Required]
        public string Description { get; set; }
        public string Mobile { get; set; }
        [Required]
        public ICollection<PetDto> Pets { get; set; }
    }

    public class PetDto
    {
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
    }
}
