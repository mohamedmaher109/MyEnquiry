using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class PDF
    {
        public int CaseNumber { get; set; }
        public string CaseType { get; set; }
        public string BankName { get; set; }

        public string ClienName { get; set; }
        public string ClienNumber { get; set; }

        public string WorkGov { get; set; }

        public string WorkAddre { get; set; }
        public string HomeGov { get; set; }
        public string HomeAddre { get; set; }
        public ICollection<QuestionAndAnswers> QuestionAndAnswers { get; set; }

    }


    public class QuestionAndAnswers
    {
        public int AnswerId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public bool HasFile { get; set; }

        public ICollection<AnswerFiles> AnswerFiles { get; set; }
    }
    public class AnswerFiles
    {
        public int AnswerId { get; set; }

        public byte[] Url { get; set; }

        

    }
}
