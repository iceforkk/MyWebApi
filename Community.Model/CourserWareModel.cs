using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Model
{
    public class CourserWareModel
    {
        public string channelName;
        public decimal Id { get; set; }
        public string Name { get; set; }
        public decimal Standards { get; set; }
        public bool RequiredFlag { get; set; }
        public double Credit { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Teacher { get; set; }
        public string Img { get; set; }
        public string Exam { get; set; }
        public int CommentCredit { get; set; }
        public int Learning { get; set; }
        public string CourseSize { get; set; }
        public int ClickCount { get; set; }
        public string Type { get; set; }
        public string Score { get; set; }
        public int Times { get; set; }
        public int Length { get; set; }
    }
}
