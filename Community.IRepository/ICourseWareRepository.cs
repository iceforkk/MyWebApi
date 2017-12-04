using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.Model;

namespace Community.IRepository
{
    public interface ICourseWareRepository : IRepository<COURSEWARE>
    {
        List<COURSEWARE> CourseClickRank(int count);

        List<UserCreditModel> UserCreDitModel(int count);

        List<CourserWareModel> GetCourseWareList(string name, string teacher, int channel, int pageIndex, int pageSize, out int totalCount);

        List<CourserWareModel> RelatedCourseList(string userid, int Courser, int pageIndex, int pageSize, out int totalCount);

        CourserWareModel GetCourserModel(decimal courserId, string userid);

        List<CourserWareModel> RelatedFinishCourseList(string userid, int isFinish, int pageIndex, int pageSize, out int totalCount);
    }
}
