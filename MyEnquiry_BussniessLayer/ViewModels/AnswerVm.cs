using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class AnswerVm
    {
        public int FormId { get; set; }
        public int CaseId { get; set; }

        public string Question { get; set; }
        public bool Qtype { get; set; }
        public string FromAnswer { get; set; }

        public bool HasFile { get; set; }

        public IFormFile FileUpload { get; set; }
        public IList<formanswers> formanswers { get; set; }

        public int? SelectedAnswerId { get; set; }
        public string SelectedAnswertext { get; set; }
    }

    public class formanswers
    {
        public string Answer { get; set; }
        public int  AnswerId { get; set; }
    }
}
