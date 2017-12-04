using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;

namespace Community.Repository
{
    public class BookTitleRepository : RepositoryBase<BookTitle>, IBookTitleRepository
    {

        public List<BookTitle> GetBookTitleList(int BookId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var db = new MobileAPIEntities())
            {
                IQueryable<BookTitle> list = db.BookTitle.Where(m => m.BookNameID == BookId);
                totalCount = list.Count();
                 var date =list.OrderByDescending(m => m.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return date;
            }
        }
    }
}
