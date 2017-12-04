using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;

namespace Community.IRepository
{
    public interface IBookNameRepository:IRepository<bookName>
    {
        List<bookName> GetBookList(string articltName, int pageIndex, int pageSize, out int totalCount,  int type);
    }
}
