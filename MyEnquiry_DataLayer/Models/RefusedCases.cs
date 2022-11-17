using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class RefusedCases
    {
        public int Id { get; set; }

        public int CaseNumber { get; set; }

        public string Resoan { get; set; }

        public bool Solved { get; set; }

        public int BankId { get; set; }
    }
}
