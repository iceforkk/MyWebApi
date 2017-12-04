using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;

namespace Community.IRepository
{
    public interface IBookTitleRepository:IRepository<BookTitle>
    {
        List<BookTitle> GetBookTitleList(int BookId, int pageIndex, int pageSize, out int totalCount);
    }
}
