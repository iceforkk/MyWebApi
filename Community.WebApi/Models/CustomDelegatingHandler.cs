using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Community.WebApi.Models
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return SendOk();

            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        private Task<HttpResponseMessage> SendOk()
        {
            var response = new HttpResponseMessage();
            //response.Content = new StringContent(error);
            response.StatusCode = HttpStatusCode.OK;
            response.Headers.Add("Access-Control-Allow-Origin", "*");//CROS
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Authorization,DNT,X-Mx-ReqToken,Keep-Alive,User-Agent,X-Requested-With,If-Modified-Since,Cache-Control,Content-Type, Accept-Language, Origin, Accept-Encoding");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With,Authorization,ASPXAUTH");//AJAX
            response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }

    }
}