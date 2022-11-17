using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
   
        public class BankCompany
        {
            /*    public BankCompany()
                {
                    this.Us = new HashSet<ApplicationUser>();
                }*/
            public int Id { get; set; }
            public bool Deleted { get; set; }
            public int CitiesId { get; set; }
            [ForeignKey("CitiesId")]
            public Cities Cities { get; set; }
            public float? PriceCase { get; set; }
            public int CaseTypesId { get; set; }
            [ForeignKey("CaseTypesId")]
            public CaseTypes CaseTypes { get; set; }
            public int? BanksId { get; set; }

            [ForeignKey("BanksId")]
            public Banks Banks { get; set; }
            public int CompaniesId { get; set; }
            [ForeignKey("CompaniesId")]
            public Companies Companies { get; set; }

        }
    }

