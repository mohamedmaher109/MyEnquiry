using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry.Helper
{
    public enum CaseEnumStatus
    {
        SentFromBank=1,
        RecivedFromCompany=2,
        WaitingForRecivers=3,
        RejectedFromBank = 4,
        AssignedToRecivers=5,
        StartFromRecivers = 6,
        ArrivedToClient =7,
        DoneFromReciver = 8,
        AcceptedFromReviewer = 9,
        AcceptedFromSupervisor =10,
        CaseDone = 11,
        AcceptedFromBank = 12,
        StartFromRepres = 13
        
       
    }
    public enum UserMediaTypes
    {
        NationalFront=1,
        NationalBack=2,
        CriminalFish=3,
        AcadimicQualification=4,
        profile=5,
        
       
    }

}
