using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModels
{
    public class UploadfileAnswer
    {
        public int id{ get; set; }
        public IFormFile file{ get; set; }
    }
}
