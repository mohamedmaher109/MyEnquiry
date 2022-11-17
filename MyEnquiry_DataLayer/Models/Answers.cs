using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_DataLayer.Models
{
    public class Answers
    {
        public Answers()
        {
        }
        public int Id { get; set; }

        public int? QuestionId { get; set; }
        public int? NumberofPhoto { get; set; }
        public int? NumberofWords { get; set; }
        public int? SelectBox { get; set; }

        public string Answer { get; set; }
        [ForeignKey("QuestionId")]
        public Questions Questions { get; set; }
    }
}
