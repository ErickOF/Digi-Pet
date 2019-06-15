using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class WalkInfoDto
    {

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
