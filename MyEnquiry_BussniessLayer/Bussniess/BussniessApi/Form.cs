using MyEnquiry_BussniessLayer.Interface.InterfaceApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Bussniess.BussniessApi
{
    public class Form : Iform
    {
        public dynamic GetSurvey(string UserId, int CompanyId, string formId,int CaseId)
        {
            var Survey = "/Surveys/Form?UserId=" + UserId + "&formId=" + formId + "&CaseId=" + CaseId + "&CompanyId="+CompanyId;

            return Survey;

        }
    }
}
