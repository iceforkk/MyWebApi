using Community.IRepository;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security; 


namespace Community.WebApi.Common
{
    public class UserAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        /// <summary>
        /// 重写认证过滤
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // 验证token
            //var token = actionContext.Request.Headers.Authorization;
            var ts = actionContext.Request.Headers.Where(c => c.Key == "ASPXAUTH").FirstOrDefault().Value;
        
            if (ts != null && ts.Count() > 0)
            {
                var token = ts.First<string>();
                // 验证token
                if (ValidateTicket(token, actionContext))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                    bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                    if (isAnonymous) base.OnAuthorization(actionContext);
                    else HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }
        /// <summary>
        /// 重写错误返回
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = new
            {
                success = false,
                errs = new[] { "服务端拒绝访问：你没有权限，或者掉线了" },
                Type = 401
            };
            response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");
        }
        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket, HttpActionContext actionContext)
        {
            if (!encryptTicket.Equals("null") && !string.IsNullOrEmpty(encryptTicket))
            {
                //依赖注入
                var provider = actionContext.ControllerContext.Configuration
                 .DependencyResolver.GetService(typeof(IUserRepository)) as IUserRepository;
                //解密Ticket
                var strTicket = FormsAuthentication.Decrypt(encryptTicket);
                if (strTicket != null)
                {
                    var user = provider.GetEntity(m => m.USER_ID == strTicket.UserData);
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("user", strTicket.UserData));
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}