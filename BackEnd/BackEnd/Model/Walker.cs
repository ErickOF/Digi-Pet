using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Model
{
    public class Walker
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        public string University { get; set; }


        public decimal Score { get {
                try
                {
                    return Walks.Select(w => w.ReportWalks).Sum(rw => rw.FirstOrDefault().Stars)/Walks.Select(w=>w.ReportWalks).Where(rw=>rw.Count>0).Count();
                }
                catch
                {
                    return 0;
                }
            } }

        public bool DoesOtherProvinces { get; set; }
        public string[] OtherProvinces { get; set; }
        public virtual ICollection<Walk> Walks { get; set; }


    }
}
