using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.ViewModel.ResponseModels
{
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<DetailsResponse> details { get; set; }
    }
    public class DetailsResponse
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
