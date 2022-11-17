using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class FormAnswersFiles
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int CaseFormAnsweId { get; set; }
        [ForeignKey("CaseFormAnsweId")]
        public CaseFormAnswers CaseFormAnswers { get; set; }
    }
}
