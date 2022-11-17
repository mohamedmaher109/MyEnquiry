using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class DuiesBank
    {
        public int? Id { get; set; }
        public float? Price { get; set; }
        public DateTime DonefromReviewr { get; set; }
        public float? Texas { get; set; }
        public float? AfterTotal  { get; set; }
        public string NameCompany { get; set; }
        public string City { get; set; }
        public float NumberOfCases { get; set; }
        public string TypeOfCases { get; set; }
        public float? Total { get; set; }
        public string TotalForCompany { get; set; }

    }
}
