using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class CaseFormAnswers
    {

        public CaseFormAnswers()
        {
            this.FormAnswersFiles = new HashSet<FormAnswersFiles>();
        }
       
        public int Id { get; set; }


        public int CaseId { get; set; }
        public string Answer { get; set; }
        public int AnswerId { get; set; }




      
        [ForeignKey("AnswerId")]
        public Answers Answers { get; set; }
        [ForeignKey("CaseId")]
        public Cases Cases { get; set; }


        public HashSet<FormAnswersFiles> FormAnswersFiles { get; set; }

    }
}
