//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Community.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER_COURSE_REG
    {
        public decimal USER_NM { get; set; }
        public decimal COURSE_ID { get; set; }
        public Nullable<short> QUALIFY { get; set; }
        public Nullable<System.DateTime> REG_DATE { get; set; }
        public string REMARK { get; set; }
        public decimal ASSIGN_ID { get; set; }
        public Nullable<short> COMPLETE { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public Nullable<int> STUDY_LIMIT { get; set; }
        public Nullable<int> from_flag { get; set; }
        public Nullable<int> active_flag { get; set; }
        public Nullable<System.DateTime> active_date { get; set; }
        public Nullable<int> active_time { get; set; }
        public Nullable<double> credit_hour { get; set; }
        public Nullable<double> extra_credit { get; set; }
    }
}
