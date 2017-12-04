using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;

namespace Community.IRepository
{
    public interface IChannelRepository:IRepository<JY_Channel>
    {
        List<JY_Channel> GetChannelList(out int totalCount);
    }
}
