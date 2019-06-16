using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public bool Blocked { get; set; }
        public bool HasQualityStar { get; set; }

        //puntos segun reglas de negocio
        public decimal Points { get
            {
                var points =(decimal) .0;
                var walksQuant = Walks.Count();

                if (walksQuant > 50) points += 10; 
                if (walksQuant > 100) points += 10;
                if (walksQuant > 500) points += 10;

                if (HasQualityStar) points += 10;

                points += Score*50;
                //los restantes 10 dependen del canton
                return points ;
            }
        }
        [NotMapped]
        public virtual decimal TempPoints { get; set; }

        public virtual ICollection<WalkerSchedule> WalkerSchedules { get; set; }

    }
}
