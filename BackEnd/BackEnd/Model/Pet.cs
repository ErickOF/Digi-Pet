﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public int PetOwnerId { get; set; }
        public virtual Petowner Petowner { get; set; }
    }
}
