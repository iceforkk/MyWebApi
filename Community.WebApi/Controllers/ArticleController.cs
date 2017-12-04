using Community.IRepository;
using Community.WebApi.Common;
using Community.WebApi.Models;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Community.WebApi.Controllers
{
    
    public class ArticleController : BaseController
    {
        readonly IArticleRepository _articleRepository;
        readonly IArticleTypeRepository _articleTypeRepository;
        readonly ICourseWareRepository _courseWareRepository;
        public ArticleController(IArticleRepository articleRepository,
            IArticleTypeRepository articleTypeRepository,
            ICourseWareRepository courseWareRepository
            )
        {
            _articleRepository = articleRepository;
            _articleTypeRepository = articleTypeRepository;
            _courseWareRepository = courseWareRepository;
        }
        /// <summary>
        /// 获取文章频道列表信息 parentId
        /// </summary>
        /// <param name="parentId">文章分类父级编号</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage GetArticleChannelInfoList(ParentIdModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (!string.IsNullOrWhiteSpace(model.ParentCode))
            {
                var category = _articleTypeRepository.GetEntity(m => m.ID == int.Parse(model.ParentCode));
                if (category == null)
                {
                    result.Message = "不存在该分类";
                    result.Type = ERRORCODE.Failed;
                    return result;
                }
                model.ParentId = category.ID;
            }
            string parentId = "1";
            if (model.ParentId > 0)
            {
                parentId = model.ParentId.ToString();
            }
            var nodes = _articleTypeRepository.GetEntities(m => m.ClassId == parentId);
            var date = new ResultInfo<CourseChannel>
            {
                TotalCount = nodes.Count(),
                List = nodes.Select(s => new CourseChannel
                {
                    Id = s.ID,
                    Name = s.TypeName,
                    ParentId = s.ClassId != null ? int.Parse(s.ClassId) : 1
                }).ToList()
            };
            result.Data = date;
            return result;
        }

        /// <summary>
        /// 获取文章列表| ArticleInfo
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// ArticleInfo
        /// </returns> 
        [HttpPost]
        public HttpResponseMessage GetArticleInfoList(ArticleModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (!string.IsNullOrWhiteSpace(model.CategoryCode))
            {
                var category = _articleTypeRepository.GetEntity(m => m.ID == model.CategoryId);
                if (category != null)
                {
                    model.CategoryId = category.ID;
                }
                else
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "文章分类不存在";
                    return result;
                }
            }

            var DD = _articleRepository.GetArtList(model.Keyword, model.Page, model.Rows, out int totalCount, model.CategoryId);
            var json = new ResultInfo<ArticleInfo>
            {
                TotalCount = totalCount,
                List = DD.Select(s => new ArticleInfo
                {
                    ArticleId = s.id,
                    ArticleTitle = s.title,
                    ArticleContent = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + "/api/Article/GetArticleDetail?id=" + s.id, //s.Content.Substring(0, 20) + "...",
                    ArticleChannel = s.Type.ToString(),
                    ArticleCreateDate = s.time,
                    //ArticleImg = Core.SourcesCore.Build(UpLoadType.ImageArticle, s.Img),
                    ArticleAuthor = s.Author,
                    ClickCount = s.ClickRate.ToString(),
                    Description = s.title,
                    Rcount = "0",
                    Tag = ""
                })
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 文章内容
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns> 
        [HttpPost, HttpGet]
        public virtual HttpResponseMessage GetArticleDetail(int id)
        {
            //var id = model.Id;
            APIInvokeResult rankge = new APIInvokeResult();
            var article = _articleRepository.GetEntity(m => m.id == id);
            if (article == null)
            {
                return null;
            }
            //更新点击次数
            article.ClickRate += 1;
            _articleRepository.Update(article);
            var json = article != null ? article.content : "";
            var html = string.Format(@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                            <meta name='viewport' content='maximum-scale=2.0,minimum-scale=0.5,user-scalable=yes,width=device-width,initial-scale=1.0'></meta>
                            <title>xx</title>
                           <style> </style>
                        </head>
                        <body>
                            <div style='word-wrap:break-word; text-align:center; font-size:larger; font-weight:blod;'>
                                <span id='ArticleDetail1_lblTitle1' title='标题'>{0}</span>
                                <br>
                                <br>
                            </div>
                            <div>
                                <span class='ArticleDetail_Ma_Ac_12_info'>发布时间：{2}</span><br/><span><span style='float:left;'>作者：{3}</span><span style='float:right;'>来源：{4}</span>
                            </div>
                            </br>
                            <div style='position:relative;padding-top:10px;word-wrap:break-word;'>
                               {5}
                            </div>
                        </body>
                        </html>
                   ", article.title, article.ClickRate, article.time.Value.ToString("yyyy-MM-dd"), article.Author, article.source, json);
            rankge.Data = html;
            return rankge;
        }

        /// <summary>
        /// 获取排行榜
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        public HttpResponseMessage GetRankInfoList(RankModelP model)
        {
            APIInvokeResult rankge = null;
            var portal = 1;
            switch (model.RankType)
            {
                //学时
                case 1:
                    rankge = GetCreditRange(model.TotalCount, model.RankType, portal);
                    break;
                //课程
                case 2:
                    rankge = GetCourseRange(model.TotalCount, model.RankType, portal);
                    break;
                //单位
                case 3:
                    break;
            }
            return rankge;
        }

        /// <summary>
        /// 课程点击排行
        /// </summary>
        /// <param name="count">获取条数</param>
        /// <param name="type">type</param>
        /// <param name="portal">平台编号</param>
        /// <returns></returns>
        [NonAction]
        public APIInvokeResult GetCourseRange(int count, int type, int portal)
        {
            APIInvokeResult result = new APIInvokeResult();
            var data = _courseWareRepository.CourseClickRank(count);
            var json = new ResultInfo<RankInfo>
            {
                TotalCount = count,
                List = data.Select((s, i) => new RankInfo
                {
                    index = i + 1,
                    name = s.COURSE_NAME,
                    value = s.topnum.ToString()
                })
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 获取学分排行
        /// </summary>
        /// <param name="count">获取条数</param>
        /// <param name="type">type</param>
        /// <param name="portal">平台编号</param>
        /// <returns></returns>
        [NonAction]
        public APIInvokeResult GetCreditRange(int count, int type, int portal)
        {
            APIInvokeResult result = new APIInvokeResult();
            var data = _courseWareRepository.UserCreDitModel(count);
            var json = new ResultInfo<RankInfo>
            {
                TotalCount = count,
                List = data.Select((s, i) => new RankInfo
                {
                    index = i + 1,
                    name = s.userName,
                    value = s.Value.Value.ToString("0.0")
                })
            };
            result.Data = json;
            return result;
        }

    }
}
