using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class CasesTypeForms
    {
        public CasesTypeForms()
        {
            this.CaseFormAnswers = new HashSet<CaseFormAnswers>();
        }
        public int Id { get; set; }

        public string Question { get; set; }

        public bool HasFile { get; set; }
        public int CaseTypeId { get; set; }

        [ForeignKey("CaseTypeId")]
        public CaseTypes CaseType { get; set; }

        public HashSet<CaseFormAnswers> CaseFormAnswers { get; set; }
    }
}
