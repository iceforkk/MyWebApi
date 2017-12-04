using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Community.IRepository;
using System.Net.Http;
using Community.WebApi.Models;
using System.Web.Security;
using Community.WebApi.Common;
using Community.DAL;
using Community.Model;

namespace Community.WebApi.Controllers
{
    public class UserController : BaseController
    {

        // public IUserRepository _userRepository { get; set; }

        readonly IUserRepository _userRepository;
        readonly ICourseWareRepository _courseWareRepository;
        readonly IChannelMmrRepository _channelMmrRepository;
        public UserController(IUserRepository userRepository,
            ICourseWareRepository courseWareRepository,
            IChannelMmrRepository channelMmrRepository
            )
        {
            _channelMmrRepository = channelMmrRepository;
            _courseWareRepository = courseWareRepository;
            _userRepository = userRepository;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="userLogin"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ValidateUser(AppLoginModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (model.Account == null)
            {
                result.Message = "输入用户名";
                return result;
            }
            var entity = _userRepository.GetEntity(m => m.USER_ID == model.Account);
            if (entity == null)
            {
                result.Message = "用户不存在";
                return result;

            }
            if (entity.USER_PWD != (model.Password))
            {
                result.Message = "密码错误";
                return result;

            }
            //保存身份票据
            SetUserAuth(entity.USER_ID, false);
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, entity.USER_NAME, DateTime.Now,
            //      DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", entity.USER_NAME, entity.USER_PWD),
            //      FormsAuthentication.FormsCookiePath);
       

            //保存登录名
            result.Message = "登录成功";
            return result;

        }
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
     
        [HttpPost]
        public HttpResponseMessage CheckUserIsExit(AccountModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var isuser = _userRepository.CheckUserLogin(model.Account);

            if (isuser > 0)
            {
                result.Message = "存在用户";
            }
            else
            {
                result.Message = "用户不存在";
                result.Type = ERRORCODE.UserNotExist;
            }
            return result;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage Register(PostRegisterModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var regUserModel = new USERS
            {
                USER_ID = model.Account,
                USER_PWD = model.Password,
                USER_TEL = model.Mobile,
                USER_NAME = model.Name,
                USER_GROUP_ID = model.GroupId,
                id_card = model.IdCard,
                //RegDate = DateTime.Now

            };
            var flag = _userRepository.Insert(regUserModel);
            if (flag == true)
            {
                result.Message = "注册成功";
            }
            else
            {
                result.Message = "注册失败";
            }
            return result;
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage LoginOut()
        {
            APIInvokeResult result = new APIInvokeResult();
            string UserID = string.Empty;
            if (HttpContext.Current.User != null || HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                UserID = HttpContext.Current.User.Identity.Name;
            }
            FormsAuthentication.SignOut();
            return result;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUserInfo(UpdateUserModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var usermodel = _userRepository.GetEntity(m => m.USER_ID == model.Account);
            if (usermodel != null)
            {
                usermodel.USER_NAME = model.Name;
                usermodel.id_card = model.IdCard;
                usermodel.USER_EMAIL = model.Email;
                usermodel.MobileNo = model.Mobile;
                usermodel.USER_GROUP_ID = model.GroupId;
                var flag = _userRepository.Update(usermodel);
                result.Message = "更新成功";
                return result;
            }
            else
            {
                result.Type = ERRORCODE.LoginError;
                result.Message = "用户不存在";
                return result;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetUserPassword(SetPasswordModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userid = LoginUserInfo();
            var usermodel = _userRepository.GetEntity(m => m.USER_ID == userid);
            if (usermodel.USER_PWD == model.OldPassword)
            {
                result.Message = "旧密码输入错误，请重新输入";
                result.Type = ERRORCODE.ParasError;
                return result;
            }
            usermodel.USER_PWD = model.OldPassword;
            var flag = _userRepository.Update(usermodel);
            if (flag != true)
            {
                result.Message = "修改失败";
                result.Type = ERRORCODE.Failed;
            }
            else
            {
                result.Message = "修改成功";
            }

            return result;
        }


        /// <summary>
        /// 获取用户课程列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserCourseInfoList(UserCourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            List<CourserWareModel> list;
            int count = 0;
            var userid = _userRepository.GetEntity(m => m.USER_ID == LoginUserInfo()).USER_NM;

            list = _courseWareRepository.RelatedFinishCourseList(LoginUserInfo(), model.Finish, model.Page, model.Rows, out int totalcount);
            count = totalcount;


            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = count,
                List = list.Select((s, i) =>
                {
                    var online = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    CourseProcess cpquery = new CourseProcess();
                    cpquery.CourseId = int.Parse(s.Id.ToString());
                    cpquery.UserId = LoginUserInfo();
                    var cplist = s.Learning;


                    var lastpostion = cplist;
                    var lastnodeid = "S001";
                    //var examstring = string.Empty;
                    //foreach (var exam in s.Course.Exam.Where(w=>w.PortalExam.Any(a=>a.PortalId==PortalId)))
                    //{
                    //  //  examstring+=exam.Name+""+exam
                    //}
                    var channel = s.channelName;
                    var onlineurl = "";
                    //if (s.Course.Standards == CourseStandards.Office.ToString())
                    //{
                    //    if (!string.IsNullOrWhiteSpace(onlineurl))
                    //    {
                    //        onlineurl = "/api/mobile/GetOffice?file=" + VerifyEncrypt.UrlEncode(onlineurl);
                    //        // onlineurl = "/Content/plugins/pdf/web/viewer.html?file=" + VerifyEncrypt.UrlEncode(onlineurl);
                    //    }
                    //}

                    return new CourseInfo
                    {
                        Index = (i + 1),
                        CourseId = s.Id.ToString(),
                        CourseName = s.Name,
                        Credit = s.Credit.ToString(),
                        CourseType = s.Type,
                        CreateDate = s.CreateDate,
                        FinishDate = DateTime.Now,
                        BrowseScore = s.Learning.ToString(),
                        LastLocation = lastpostion,////TODO 2015-02-28 09:24:08 添加LastNodeID
                        LastNodeId = lastnodeid ?? "S001",// lastnodeid,
                        TeacherName = s.Teacher,
                        Description = s.Description,
                        Duration = "",

                        OnlineUrl = onlineurl,// online != null ? online.Url : "",
                        CourseImg = s.Img,
                        ChannelName = s.channelName
                    };
                }).ToList()
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 注册登录票据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="createPersistentCookie"></param>
        [NonAction]
        public void SetUserAuth(string userId, bool admin)
        {
            string roles = string.Empty;

            FormsAuthentication.SetAuthCookie(userId.ToString(), true, FormsAuthentication.FormsCookiePath);

            if (admin)
            {
                roles = "Admin";
            }
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), true, userId.ToString());

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie ck = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(ck);
            HttpContext.Current.Response.AddHeader("ASPXAUTH", encTicket); 
            //this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }
    }
}