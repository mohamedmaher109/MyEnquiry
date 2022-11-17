using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Cases
    {
        public Cases()
        {
            this.CaseFiles = new HashSet<CaseFiles>();
            this.CasesOrders = new HashSet<CasesOrders>();
            this.CaseFormAnswers = new HashSet<CaseFormAnswers>();
        }
        public int Id { get; set; }

        public string ClientName { get; set; }
        public string ClientNumbers { get; set; }

        public string NationalId { get; set; }

        public string WorkGovernorate { get; set; }
        public string WorkAddress { get; set; }

        public string HomeGovernorate { get; set; }
        public string HomeAddress { get; set; }
        public string EnquirerName { get; set; }
        public string ReviewerId { get; set; }
        public bool IsReiew { get; set; } = false;
        public string SuperVisorId { get; set; }
        public int? BankId { get; set; }


        public string RejectResion { get; set; }
        public string? FilesANswer { get; set; }
        public int CompanyId { get; set; }
        public int CaseStatusId { get; set; }
        public bool IsMain { get; set; }=false;
        public int? CasesId { get; set; }
        [ForeignKey("CasesId")]
        public Cases Casese { get; set; }
        public int CaseTypeId { get; set; }
        public string? lat { get; set; }
        public string? lng { get; set; }

        public bool Deleted { get; set; } = false;
        public DateTime DoneFromBank { get; set; } 
        public bool PaidFromCompany { get; set; } = false;
        public bool PaidFromBank { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        [ForeignKey("SuperVisorId")]
        public ApplicationUser SuperVisor { get; set; }
        [ForeignKey("ReviewerId")]
        public ApplicationUser Reviewer { get; set; }
        [ForeignKey("BankId")]
        public Banks Bank { get; set; }
        [ForeignKey("CompanyId")]
        public Companies Company { get; set; }
        [ForeignKey("CaseTypeId")]
        public CaseTypes CaseType { get; set; }
        [ForeignKey("CaseStatusId")]
        public CaseStatus CaseStatus { get; set; }

        public HashSet<CaseFiles> CaseFiles { get; set; }
        public HashSet<CasesOrders> CasesOrders { get; set; }
        public HashSet<CaseFormAnswers> CaseFormAnswers { get; set; }




        //NotMapped
        [NotMapped]
        public IFormFile File { get; set; }
        [NotMapped]
        public string FileToShow { get; set; }

    }
}
