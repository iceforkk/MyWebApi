using Community.Common;
using System.Web.Http;
using System.Web.Mvc;

namespace Community.WebApi.Common
{
    public class BaseController : ApiController
    {

      
        #region 取得登录用户信息

        /// <summary>
        /// 取得登录用户信息
        /// </summary>
        /// <returns></returns>
        protected string LoginUserInfo()
        {
            if (System.Web.HttpContext.Current.Request.Cookies["user"] != null)
            {
                var uer = System.Web.HttpContext.Current.Request.Cookies["user"].Value;
                return uer;
            }
            else
            {
                return "";
            }
        }

        #endregion 取得登录用户信息

    }
}