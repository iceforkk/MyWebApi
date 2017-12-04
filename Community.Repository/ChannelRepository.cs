using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;

namespace Community.Repository
{
    public class ChannelRepository : RepositoryBase<JY_Channel>,IChannelRepository
    {
        public List<JY_Channel> GetChannelList( out int totalCount)
        {
            using (var db = new MobileAPIEntities())
            {
                IQueryable<JY_Channel> list = db.JY_Channel.Where(p=>p.parentid!=-1);
                totalCount = list.Count();
                return list.OrderByDescending(m => m.OrderID).ToList();

            }

        }

    }
}
