using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class WalkInfoDto
    {
        public WalkInfoDto()
        {

        }
        public WalkInfoDto(Walk walk)
        {
            Id = walk.Id;
            Begin = walk.Begin;
            Duration = walk.Duration;
            Province = walk.Province;
            Canton = walk.Canton;
            Description = walk.Description;
            Pet = new PetDto( walk.Pet);
            WalkerId = walk.WalkerId;
            ExactAddress = walk.ExactAddress;
            if(walk.ReportWalks!=null && walk.ReportWalks.Count != 0)
            {
                Report = walk.ReportWalks.Select(rw => { rw.Walk = null; return rw; }).ToList();
            }
            Total = walk.Total;
            
        }
        public int Id { get; set; }
        public PetDto Pet { get; set; }

        public int WalkerId { get; set; }
        public string ExactAddress { get; private set; }
        public ICollection<ReportWalk> Report { get; }
        public DateTime Begin { get; set; }

        public decimal Duration { get; set; }
        
        public string Province { get; set; }

        public string Canton { get; set; }
  
        public string Description { get; set; }
        public decimal Total { get; set; }
    }
}
