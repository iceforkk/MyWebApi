using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;
using Community.Model;
using Community.Common;
using System.Data.SqlClient;
using System.Data;

namespace Community.Repository
{
    public class CourseWareRepository : RepositoryBase<COURSEWARE>, ICourseWareRepository
    {
        public List<COURSEWARE> CourseClickRank(int count)
        {
            using (var db = new MobileAPIEntities())
            {
                var list = db.COURSEWARE.OrderByDescending(m => m.topnum).Take(count).ToList();

                return list;
            }

        }

        public List<UserCreditModel> UserCreDitModel(int count)
        { 
            using (var db = new MobileAPIEntities())
            {

                var userlist = (from a in db.UserCredit
                                join b in db.USERS on a.User_ID equals b.USER_ID
                                where a.CourseCredit >= 0
                                select new UserCreditModel
                                {
                                    userName = db.USERS.Where(m => m.USER_ID == a.User_ID).Select(pp => pp.USER_NAME).FirstOrDefault(),
                                    Value = a.CourseCredit + a.ExamCredit + a.OtherCredit + a.virCredit
                                }
                               ).OrderByDescending(m => m.Value).Take(count).ToList();

                return userlist;
            }
        }

        public List<CourserWareModel> GetCourseWareList(string name, string teacher, int channel, int pageIndex, int pageSize, out int totalCount)
        {

            //由于数据库字段不一样 此方法导致 类型转换导致加载很慢
            //using (var db = new MobileAPIEntities())
            //{
            //    var list = (from a in db.COURSEWARE
            //                join b in db.JY_Channellesson on a.COURSE_ID equals (decimal)b.lessionid
            //                join c in db.JY_Channel on b.Channelid equals (short)c.ChannelID
            //                join d in db.JY_Lession on a.COURSE_NUMBER equals d.Course_number
            //                join e in db.jy_lessionimage on d.id equals e.LID
            //                where (channel != 0 ? b.Channelid == channel : b.Channelid == 58)
            //                && (!string.IsNullOrEmpty(teacher) ? a.teachername == teacher : true)
            //                && (!string.IsNullOrEmpty(name) ? a.COURSE_NAME.Contains(name) : true)
            //                select new CourserWareModel
            //                {
            //                    channelName = c.ChannelName,
            //                    ClickCount = a.topnum != null ? a.topnum.Value : 0,
            //                    CourseSize = a.LIMIT_TIME.ToString(),
            //                    CreateDate = a.COURSE_CREATEDATE.Value,
            //                    Credit = a.credit_hour != null ? a.credit_hour.Value : 0,
            //                    Exam = a.exam_id.ToString(),
            //                    Id = a.COURSE_ID,
            //                    Img = e.Path,
            //                    Description = a.DESCRIPTION,
            //                    Name = a.COURSE_NAME,
            //                    Learning = 0,
            //                    Teacher = a.teachername,
            //                    Standards = a.TYPE_ID != null ? a.TYPE_ID.Value : 0,
            //                    Duration = a.LIMIT_TIME != null ? a.LIMIT_TIME.Value : 0,
            //                    CommentCredit = a.Recommend,
            //                    Type = a.COURSE_TYPE
            //                }).OrderByDescending(m => m.CreateDate);

            //    totalCount = list.Count();
            //    return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //}

            //采用存储过程
            using (var db = new MobileAPIEntities())
            {
                int start = (pageIndex - 1) * pageSize;
                int end = pageIndex * pageSize;

                var data = db.GetCourseWareList(
                    string.IsNullOrWhiteSpace(name) ? "无" : name,
                    string.IsNullOrWhiteSpace(teacher) ? "无" : teacher,
                    channel.ToString()
                    ).OrderByDescending(m => m.COURSE_CREATEDATE).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                List<CourserWareModel> list = new List<CourserWareModel>();
                data.ForEach(s=> {
                    CourserWareModel model = new CourserWareModel();
                    model.channelName = s.ChannelName;
                    model.ClickCount = s.topnum != null ? s.topnum.Value : 0;
                    model.CourseSize = s.LIMIT_TIME.ToString();
                    model.CreateDate = s.COURSE_CREATEDATE.Value;
                    model.Credit = s.credit_hour != null ? s.credit_hour.Value : 0;
                    model.Exam = s.exam_id.ToString();
                    model.Id = s.COURSE_ID;
                    model.Img = s.Path;
                    model.Description = s.DESCRIPTION;
                    model.Name = s.COURSE_NAME;
                    model.Learning = 0;
                    model.Teacher = s.teachername;
                    model.Standards = s.TYPE_ID != null ? s.TYPE_ID.Value : 0;
                    model.Duration = s.LIMIT_TIME != null ? s.LIMIT_TIME.Value : 0;
                    model.CommentCredit = s.Recommend;
                    model.Type = s.COURSE_TYPE;
                    list.Add(model);
                });
                totalCount = 0;
                return list;
            }
        }


        public List<CourserWareModel> RelatedCourseList(string userid, int Courser, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new MobileAPIEntities())
            {
                var list = (from a in db.COURSEWARE
                            join b in db.JY_Channellesson on a.COURSE_ID equals (decimal)b.lessionid
                            join c in db.JY_Channel on b.Channelid equals (short)c.ChannelID
                            where b.Channelid == 58
                            orderby a.topnum descending
                            select new CourserWareModel
                            {
                                channelName = c.ChannelName,
                                ClickCount = a.topnum != null ? a.topnum.Value : 0,
                                CourseSize = a.LIMIT_TIME.ToString(),
                                CreateDate = a.COURSE_CREATEDATE.Value,
                                Credit = a.credit_hour != null ? a.credit_hour.Value : 0,
                                Exam = a.exam_id.ToString(),
                                Id = a.COURSE_ID,
                                Img = "",
                                Description = a.DESCRIPTION,
                                Name = a.COURSE_NAME,
                                Learning = 0,
                                Teacher = a.teachername,
                                Standards = a.TYPE_ID != null ? a.TYPE_ID.Value : 0,
                                Duration = a.LIMIT_TIME != null ? a.LIMIT_TIME.Value : 0,
                                CommentCredit = a.Recommend,
                                Type = a.COURSE_TYPE
                            }).OrderByDescending(m => m.CreateDate);

                totalCount = list.Count();
                return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<CourserWareModel> RelatedFinishCourseList(string userid, int isFinish, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new MobileAPIEntities())
            {
                DateTime apptime = Convert.ToDateTime("2017-1-1");
                var list = (from a in db.AICC_J_HIGH_SCORE
                            join b in db.COURSEWARE on a.COURSE_ID equals b.COURSE_NUMBER
                            join d in db.JY_Lession on a.COURSE_ID equals d.Course_number
                            join e in db.jy_lessionimage on d.id equals e.LID 
                            where a.STUDENT_ID == userid && a.OBJ_FIRST_DATE> apptime
                            && (isFinish == 1 ? a.length <= a.timems : a.length > a.timems)
                            orderby a.OBJ_FIRST_DATE descending
                            select new CourserWareModel
                            {
                                channelName = "",
                                ClickCount = b.topnum.Value,
                                CourseSize = b.LIMIT_TIME.ToString(),
                                CreateDate = b.COURSE_CREATEDATE.Value,
                                Credit = b.credit_hour.Value,
                                Exam = b.exam_id.ToString(),
                                Id = b.COURSE_ID,
                                Img = e.Path,
                                Description = b.DESCRIPTION,
                                Name = b.COURSE_NAME,
                                Teacher = b.teachername,
                                Standards = b.TYPE_ID.Value,
                                Duration = b.LIMIT_TIME.Value,
                                CommentCredit = b.Recommend,
                                Times = a.timems.Value,
                                Length = a.length.Value,
                                Type = b.COURSE_TYPE
                            });
                totalCount = list.Count();
                return list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public CourserWareModel GetCourserModel(decimal courserId, string userid)
        {
            using (var db = new MobileAPIEntities())
            {
                var model = (from a in db.COURSEWARE
                             join b in db.JY_Channellesson on a.COURSE_ID equals  (decimal)b.lessionid
                             join c in db.JY_Channel on b.Channelid equals (short) c.ChannelID
                             join d in db.JY_Lession on a.COURSE_NUMBER equals d.Course_number
                             join e in db.jy_lessionimage on d.id equals e.LID
                             where a.COURSE_ID == courserId
                             select new CourserWareModel
                             {
                                 channelName = c.ChannelName,
                                 ClickCount = a.topnum.Value,
                                 CourseSize = a.LIMIT_TIME.ToString(),
                                 CreateDate = a.COURSE_CREATEDATE.Value,
                                 Credit = a.credit_hour.Value,
                                 Exam = a.exam_id.ToString(),
                                 Id = a.COURSE_ID,
                                 Img = e.Path,
                                 Description = a.DESCRIPTION,
                                 Name = a.COURSE_NAME,
                                 Learning = db.AICC_J_HIGH_SCORE.Where(m => m.COURSE_ID == a.COURSE_NUMBER && m.STUDENT_ID == userid).FirstOrDefault() == null ? 0 : db.AICC_J_HIGH_SCORE.Where(m => m.COURSE_ID == a.COURSE_NUMBER && m.STUDENT_ID == userid).FirstOrDefault().timems.Value,
                                 Teacher = a.teachername,
                                 Standards = a.TYPE_ID.Value,
                                 Duration = a.LIMIT_TIME.Value,
                                 CommentCredit = a.Recommend,
                                 Type = a.COURSE_TYPE,
                                 Score = a.Recommend.ToString()
                             }).FirstOrDefault();

                return model;

            }
        }
    }
}
