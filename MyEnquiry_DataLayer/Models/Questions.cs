using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Questions
    {

        public Questions()
        {
            this.Answers = new HashSet<Answers>();
        }
        public int Id { get; set; }
        public int CaseTypeId { get; set; }

        public string Question { get; set; }
        public bool HasFile { get; set; }
        public bool choice { get; set; }

        public  HashSet<Answers> Answers { get; set; }
        [ForeignKey("CaseTypeId")]
        public CaseTypes CaseTypes { get; set; }
    }
}
