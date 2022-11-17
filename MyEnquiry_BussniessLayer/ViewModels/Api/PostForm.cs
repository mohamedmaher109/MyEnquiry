using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels.Api
{
    public class PostForm
    {
        public int QuestionId { get; set; }
        public int CaseId { get; set; }

        public string FromAnswer { get; set; }

        public bool HasFile { get; set; }

        public IFormFile FileUpload { get; set; }

        public int? SelectedAnswerId { get; set; }
        public string SelectedAnswertext { get; set; }
    }
}
