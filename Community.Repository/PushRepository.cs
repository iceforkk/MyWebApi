using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.IRepository;
using Community.DAL;


namespace Community.Repository
{
    public class PushRepository : RepositoryBase<Push>, IPushRepository
    {
        public List<Push> GetListByPage(string Keyword, int Pags, int Rows,out int total)
        {
            using (var db = new MobileAPIEntities())
            {
                IQueryable<Push> list = db.Push;

                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    list = list.Where(m => m.Name.Contains(Keyword));
                }
                total = list.Count();
                var data = list.OrderByDescending(m => m.Time).Skip((Pags - 1) * Rows).Take(Rows).ToList();
                return data;
            }
        }
    }
}
