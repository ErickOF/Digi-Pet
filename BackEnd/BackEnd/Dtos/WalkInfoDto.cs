using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class WalkInfoDto
    {
        public WalkInfoDto(Walk walk)
        {
            Id = walk.Id;
            Begin = walk.Begin;
            Duration = walk.Duration;
            Province = walk.Province;
            Canton = walk.Canton;
            Description = walk.Description;
            PetId = walk.PetId;
            WalkerId = walk.WalkerId;
        }
        public int Id { get; set; }
        public int PetId { get; set; }

        public int WalkerId { get; set; }

        public DateTime Begin { get; set; }

        public decimal Duration { get; set; }
        
        public string Province { get; set; }

        public string Canton { get; set; }
  
        public string Description { get; set; }
    }
}
