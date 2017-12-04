using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;

namespace Community.Repository
{
    public class ChannelMmrRepository : RepositoryBase<JY_UrlMms>, IChannelMmrRepository
    {
        public JY_UrlMms Getfirst(decimal id)
        {
            using (var db = new MobileAPIEntities())
            {
                var date = db.JY_UrlMms.Where(m => m.CourseID == id).FirstOrDefault();
                return date;
            }
        }
    }
}
