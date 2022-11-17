using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class BankCompanyVm
    {
        public int Id { get; set; }
        public string CaseTypesId { get; set; }
        public string BanksId { get; set; }
        public string CompaniesId { get; set; }
        public string CitiesId { get; set; }
        public float? PriceCase { get; set; }
    }
}
