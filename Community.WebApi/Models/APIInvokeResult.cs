using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace Community.WebApi.Models
{
    public class APIInvokeResult
    {
        public APIInvokeResult(ERRORCODE pError = ERRORCODE.OK, String pMessage = "success")
        {
            Type = pError;
            Message = pMessage;
            t = DateTime.Now.Ticks;
            Data = null;
        }
        #region Member 
        private ERRORCODE _type;
        public ERRORCODE Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                string error_message = "";
                ERRORS.ERRORCODES.TryGetValue((int)_type, out error_message);
                Message = error_message;
            }
        }
        public String Message { get; set; }
        public Int64 t { get; set; }
        public Object Data { get; set; }




        #endregion

        #region Method
        public String ToJsonpResult(String pCallBack)
        {
            return String.Format("{0}({1})", pCallBack, JsonConvert.SerializeObject(Data));
        }
        public String ToJsonResult()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static implicit operator String(APIInvokeResult result)
        {

            return result == null ? String.Empty : result.ToJsonResult();
        }
        public static implicit operator HttpResponseMessage(APIInvokeResult result)
        {
            //HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Origin", "*");//CROS            
            //HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With,Authorization,ASPXAUTH");//AJAX
            //HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
            //HttpContext.Current.Response.Headers.Add("ContentType", "application/json");
            return new HttpResponseMessage { Content = new StringContent(result, Encoding.UTF8, "application/json") };
        }
        #endregion
    }

    /// <summary>
    /// date结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultInfo<T>
    {
        public int TotalCount { get; set; }

        public IEnumerable<T> List { get; set; }
    }
    #region 错误代码
    public enum ERRORCODE
    {
        OK = 1,
        Failed = 0,
        LoginError = 3,
        DataError = 98,
        ParaNull = 99,
        NoAuthority = 100,
        ParasError = 101,
        SMSCodeError = 102,
        NoPermission = 103,
        AuthExpire = 104,

        UserExist = 200,
        PhoneExist = 201,
        PassWordError = 202,
        UserNotExist = 203,
        SamePassWordError = 204,
        TokenError = 205,






        WXPayError = 701,
        WXCodeError = 702,

        NotSupport = 801,

        ServerError = 501,
        SendSmsFail = 502,
    }
    public static class ERRORS
    {
        public static readonly Dictionary<Int32, String> ERRORCODES = new Dictionary<int, string> {
            {0,"success" },
            {1,"请求失败" },
            {2,"登录名或密码为空" },
            {98,"请求数据为空" },
            {99,"请求参数为空" },
            {100,"没有登录" },
            {101,"请求参数错误" },
            {102,"短信验证码错误" },
            {103,"没有接口权限" },
            {104,"登录状态过期,请重新登录" },
            { 200,"用户名已被使用"},
            { 201,"此手机号已注册"},
            { 202,"用户名或密码错误"},
            { 203,"用户不存在"},
            { 204,"新旧密码相同"},
            { 205,"无效Token值"},
            { 501,"服务器错误"},
            { 502,"短信发送失败"},


            { 701,"微信支付下单失败"},
            { 702,"微信CODE无效"},

            { 801,"在此场景下，功能暂不支持"},


        };

    }

    #endregion
}
