using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface.InterfaceApi
{
    public interface Iform
    {
        dynamic GetSurvey(string UserId, int CompanyId, string formId, int CaseId); 
    }
}
