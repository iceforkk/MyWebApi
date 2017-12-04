using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.DAL;
using Community.IRepository;
using System.Data.SqlClient;

namespace Community.Repository
{
    public class UserRepository : RepositoryBase<USERS>, IUserRepository
    {
        public USERS Login(string userName, string pwd)
        {
            using (var db = new MobileAPIEntities())
            {
         
                var date = db.USERS.Where(d => d.USER_ID == userName && d.USER_PWD == pwd).FirstOrDefault<USERS>();

                return date;
            }
        }

        public int CheckUserLogin(string userName)
        {
            var flag = 0;
            using (var db = new MobileAPIEntities())
            {
                var date = db.USERS.Where(m => m.USER_ID == userName).FirstOrDefault();

                if (date != null)
                {
                    flag = 1;
                }
            }
            return flag;
        }

        public int Resister(USERS user)
        {
            using (var db = new MobileAPIEntities())
            {

               return db.InsertMobileUSERS(
                    (short)user.user_state.Value,
                    user.LAST_LOGIN_TIME.Value,
                    user.USER_GROUP_ID.Value,
                    user.USER_ID,
                    user.USER_TYPE.Value,
                    user.USER_NAME,
                    user.USER_PWD,
                     user.USER_TEL,
                     user.LOGIN_TIMES.Value,
                     user.id_card
                    );

                //string strsql = " exec  InsertMobileUSERS @USER_STATUS,@LAST_LOGIN_TIME,@USER_GROUP_ID,@USER_ID,@USER_TYPE,@USER_NAME,@USER_PWD,@USER_TEL,@LOGIN_TIMES,@id_card";

                //SqlParameter[] parms = new SqlParameter[11];

                //parms[0] = new SqlParameter("@USER_STATUS",(short)user.user_state.Value);
                //parms[1] = new SqlParameter("@LAST_LOGIN_TIME", user.LAST_LOGIN_TIME.Value);
                //parms[2] = new SqlParameter("@USER_GROUP_ID", user.USER_GROUP_ID.Value);
                //parms[3] = new SqlParameter("@USER_ID", user.USER_ID);
                //parms[4] = new SqlParameter("@USER_TYPE", user.USER_TYPE.Value);
                //parms[5] = new SqlParameter("@USER_NAME", user.USER_NAME);
                //parms[6] = new SqlParameter("@USER_PWD", user.USER_PWD);
                //parms[7] = new SqlParameter("@USER_TEL", user.USER_TEL);
                //parms[8] = new SqlParameter("@LOGIN_TIMES", user.LOGIN_TIMES.Value);
                //parms[9] = new SqlParameter("@id_card", user.id_card); 

                //return db.Database.ExecuteSqlCommand(strsql, parms);

            }
        }
        public int UserCreModel(string UserId)
        {
            using (var db = new MobileAPIEntities())
            {
                var userCre = db.UserCredit.Where(m => m.User_ID == UserId).FirstOrDefault();
                if (userCre != null)
                {
                    return Convert.ToInt32(userCre.CourseCredit + userCre.virCredit + userCre.ExamCredit);
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
