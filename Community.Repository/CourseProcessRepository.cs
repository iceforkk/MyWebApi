using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;
using Community.Model;
using Community.Common;

namespace Community.Repository
{
    public class CourseProcessRepository : RepositoryBase<AICC_J_HIGH_SCORE>, ICourseProcessRepository
    {
        public string IsAny(string userid, string courseId)
        {
            using (var db = new MobileAPIEntities())
            {
                if (string.IsNullOrEmpty(userid))
                {
                    return "未选";
                }
                else if (db.AICC_J_HIGH_SCORE.Any(m => m.COURSE_ID.Equals(courseId) && m.STUDENT_ID == userid))
                {
                    return "已选";
                }
                else
                {
                    return "未选";
                }

            }
        }
    }
}
