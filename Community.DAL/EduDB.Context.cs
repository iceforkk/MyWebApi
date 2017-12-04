﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MobileAPIEntities : DbContext
    {
        public MobileAPIEntities()
            : base("name=MobileAPIEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AICC_J_HIGH_SCORE> AICC_J_HIGH_SCORE { get; set; }
        public virtual DbSet<BookContent> BookContent { get; set; }
        public virtual DbSet<bookName> bookName { get; set; }
        public virtual DbSet<BookTitle> BookTitle { get; set; }
        public virtual DbSet<BookType> BookType { get; set; }
        public virtual DbSet<CHW_EXAM_PREF> CHW_EXAM_PREF { get; set; }
        public virtual DbSet<COURSEWARE> COURSEWARE { get; set; }
        public virtual DbSet<Jy_Article> Jy_Article { get; set; }
        public virtual DbSet<Jy_Article_Fabulous> Jy_Article_Fabulous { get; set; }
        public virtual DbSet<Jy_Article_Fujian> Jy_Article_Fujian { get; set; }
        public virtual DbSet<JY_Atype> JY_Atype { get; set; }
        public virtual DbSet<JY_Channel> JY_Channel { get; set; }
        public virtual DbSet<JY_Channellesson> JY_Channellesson { get; set; }
        public virtual DbSet<JY_Lession> JY_Lession { get; set; }
        public virtual DbSet<jy_lessionimage> jy_lessionimage { get; set; }
        public virtual DbSet<JY_UrlMms> JY_UrlMms { get; set; }
        public virtual DbSet<Push> Push { get; set; }
        public virtual DbSet<tblArticleComment> tblArticleComment { get; set; }
        public virtual DbSet<USER_COURSE_REG> USER_COURSE_REG { get; set; }
        public virtual DbSet<USER_GROUP> USER_GROUP { get; set; }
        public virtual DbSet<user_xueli> user_xueli { get; set; }
        public virtual DbSet<user_zj> user_zj { get; set; }
        public virtual DbSet<UserAppeal> UserAppeal { get; set; }
        public virtual DbSet<UserCredit> UserCredit { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }
        public virtual DbSet<CHW_CONTENT_TYPE_BASE> CHW_CONTENT_TYPE_BASE { get; set; }
    
        public virtual int GetCourseWareListCount(string name, string teacher, Nullable<int> channel, string start, string end)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var teacherParameter = teacher != null ?
                new ObjectParameter("teacher", teacher) :
                new ObjectParameter("teacher", typeof(string));
    
            var channelParameter = channel.HasValue ?
                new ObjectParameter("channel", channel) :
                new ObjectParameter("channel", typeof(int));
    
            var startParameter = start != null ?
                new ObjectParameter("start", start) :
                new ObjectParameter("start", typeof(string));
    
            var endParameter = end != null ?
                new ObjectParameter("end", end) :
                new ObjectParameter("end", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetCourseWareListCount", nameParameter, teacherParameter, channelParameter, startParameter, endParameter);
        }
    
        public virtual int InsertMobileUSERS(Nullable<short> uSER_STATUS, Nullable<System.DateTime> lAST_LOGIN_TIME, Nullable<decimal> uSER_GROUP_ID, string uSER_ID, Nullable<short> uSER_TYPE, string uSER_NAME, string uSER_PWD, string uSER_TEL, Nullable<int> lOGIN_TIMES, string id_card)
        {
            var uSER_STATUSParameter = uSER_STATUS.HasValue ?
                new ObjectParameter("USER_STATUS", uSER_STATUS) :
                new ObjectParameter("USER_STATUS", typeof(short));
    
            var lAST_LOGIN_TIMEParameter = lAST_LOGIN_TIME.HasValue ?
                new ObjectParameter("LAST_LOGIN_TIME", lAST_LOGIN_TIME) :
                new ObjectParameter("LAST_LOGIN_TIME", typeof(System.DateTime));
    
            var uSER_GROUP_IDParameter = uSER_GROUP_ID.HasValue ?
                new ObjectParameter("USER_GROUP_ID", uSER_GROUP_ID) :
                new ObjectParameter("USER_GROUP_ID", typeof(decimal));
    
            var uSER_IDParameter = uSER_ID != null ?
                new ObjectParameter("USER_ID", uSER_ID) :
                new ObjectParameter("USER_ID", typeof(string));
    
            var uSER_TYPEParameter = uSER_TYPE.HasValue ?
                new ObjectParameter("USER_TYPE", uSER_TYPE) :
                new ObjectParameter("USER_TYPE", typeof(short));
    
            var uSER_NAMEParameter = uSER_NAME != null ?
                new ObjectParameter("USER_NAME", uSER_NAME) :
                new ObjectParameter("USER_NAME", typeof(string));
    
            var uSER_PWDParameter = uSER_PWD != null ?
                new ObjectParameter("USER_PWD", uSER_PWD) :
                new ObjectParameter("USER_PWD", typeof(string));
    
            var uSER_TELParameter = uSER_TEL != null ?
                new ObjectParameter("USER_TEL", uSER_TEL) :
                new ObjectParameter("USER_TEL", typeof(string));
    
            var lOGIN_TIMESParameter = lOGIN_TIMES.HasValue ?
                new ObjectParameter("LOGIN_TIMES", lOGIN_TIMES) :
                new ObjectParameter("LOGIN_TIMES", typeof(int));
    
            var id_cardParameter = id_card != null ?
                new ObjectParameter("id_card", id_card) :
                new ObjectParameter("id_card", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertMobileUSERS", uSER_STATUSParameter, lAST_LOGIN_TIMEParameter, uSER_GROUP_IDParameter, uSER_IDParameter, uSER_TYPEParameter, uSER_NAMEParameter, uSER_PWDParameter, uSER_TELParameter, lOGIN_TIMESParameter, id_cardParameter);
        }
    
        public virtual ObjectResult<GetCourseWareList_Result> GetCourseWareList(string name, string teacher, string channel)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var teacherParameter = teacher != null ?
                new ObjectParameter("teacher", teacher) :
                new ObjectParameter("teacher", typeof(string));
    
            var channelParameter = channel != null ?
                new ObjectParameter("channel", channel) :
                new ObjectParameter("channel", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCourseWareList_Result>("GetCourseWareList", nameParameter, teacherParameter, channelParameter);
        }
    }
}
