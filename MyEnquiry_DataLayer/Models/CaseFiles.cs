using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class CaseFiles
    {
        public int Id { get; set; }

        public string ExcelSheet { get; set; }

        public int CaseId { get; set; }
        public int Type { get; set; } //1 from bank 2 from company
       [ForeignKey("CaseId")]
        public Cases Case { get; set; }
    }
}
