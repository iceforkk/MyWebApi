using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;

namespace Community.Repository
{
    public class BookNameRepository : RepositoryBase<bookName>, IBookNameRepository
    {
        public List<bookName> GetBookList(string articltName, int pageIndex, int pageSize, out int totalCount,int type )
        {
            using (var db = new MobileAPIEntities())
            {
                IQueryable<bookName> list = db.bookName;
                if (type != 0)
                {
                    list = list.Where(m => m.BookTypeID == type);
                }
                if (!string.IsNullOrEmpty(articltName))
                {
                    list = list.Where(m => m.Name.Contains(articltName));
                }

                totalCount = list.Count();
                var date= list.OrderByDescending(m => m.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return date;
            }
        }
    }
}
