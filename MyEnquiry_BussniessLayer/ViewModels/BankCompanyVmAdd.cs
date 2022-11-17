using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class BankCompanyVmAdd
    {
        public int Id { get; set; }
        public int CaseTypesId { get; set; }
        public int BanksId { get; set; }
        public int CompaniesId { get; set; }
        public int CitiesId { get; set; }
        public float? PriceCase { get; set; }
    }
}
