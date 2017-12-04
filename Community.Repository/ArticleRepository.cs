using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;

namespace Community.Repository
{
    public class ArticleRepository : RepositoryBase<Jy_Article>, IArticleRepository
    {
        public List<Jy_Article> GetArtList(string articltName, int pageIndex, int pageSize, out int totalCount, int type = 0)
        {
            using (var db = new MobileAPIEntities())
            {
                IQueryable<Jy_Article> list = db.Jy_Article;
                if (type != 0)
                {
                    list = list.Where(m => m.Type == type);
                }
                if (!string.IsNullOrEmpty(articltName))
                {
                    list = list.Where(m => m.title.Contains(articltName));
                }

                totalCount = list.Count();
                var date = list.OrderByDescending(m => m.time).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return date;
            }
        }


    }
}
