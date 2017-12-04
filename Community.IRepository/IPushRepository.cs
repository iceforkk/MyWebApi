using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;

namespace Community.IRepository
{
    public interface IPushRepository:IRepository<Push>
    {
        List<Push> GetListByPage(string Keyword,int Page, int Rows, out int total);
    }
}
