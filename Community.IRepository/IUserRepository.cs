using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;

namespace Community.IRepository
{
    public interface IUserRepository :  IRepository<USERS>
    {
        /// <summary>
        /// 登录方法（测试版）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        USERS Login(string userName, string pwd);

        int CheckUserLogin(string userName);

        int Resister(USERS user);

        int UserCreModel(string UserId);

    }
}
