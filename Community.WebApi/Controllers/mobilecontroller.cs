using Community.DAL;
using Community.IRepository;
using Community.Model;
using Community.WebApi.Common;
using Community.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Community.WebApi.Controllers
{
    [UserAuthorize]
    public class mobileController : BaseController
    {
        readonly IArticleRepository _articleRepository;
        readonly IArticleTypeRepository _articleTypeRepository;
        readonly ICourseWareRepository _courseWareRepository;
        readonly IBookContentRepository _bookContentRepository;
        readonly IBookNameRepository _bookNameRepository;
        readonly IBookTitleRepository _bookTitleRepository;
        readonly IBookTypeRepository _bookTypeRepository;
        readonly IChannelRepository _channelRepository;
        readonly IChannelMmrRepository _channelMmrRepository;
        readonly IChannelLiessonRepository _channelLiessonRepository;
        readonly IUserRepository _userRepository;
        readonly ICourseProcessRepository _courseProcessRepository;
        readonly IUserGroupRepository _userGroupRepository;
        readonly IXueLiRepository _xueLiRepository;
        readonly IGradeRepository _gradeRepository;
        readonly IUserAppealRepository _userAppealRepository;
        readonly IPushRepository _pushRepository;
        public mobileController(IArticleRepository articleRepository,
            IArticleTypeRepository articleTypeRepository,
            ICourseWareRepository courseWareRepository,
            IBookContentRepository bookContentRepository,
            IBookNameRepository bookNameRepository,
            IBookTitleRepository bookTitleRepository,
            IBookTypeRepository bookTypeRepository,
            IChannelRepository channelRepository,
            IChannelMmrRepository channelMmrRepository,
            IChannelLiessonRepository channelLiessonRepository,
            IUserRepository userRepository,
            ICourseProcessRepository courseProcessRepository,
            IUserGroupRepository userGroupRepository,
            IXueLiRepository xueLiRepository,
            IGradeRepository gradeRepository,
            IUserAppealRepository userAppealRepository,
            IPushRepository pushRepository
            )
        {
            _userGroupRepository = userGroupRepository;
            _articleRepository = articleRepository;
            _articleTypeRepository = articleTypeRepository;
            _courseWareRepository = courseWareRepository;
            _bookContentRepository = bookContentRepository;
            _bookNameRepository = bookNameRepository;
            _bookTitleRepository = bookTitleRepository;
            _bookTypeRepository = bookTypeRepository;
            _channelRepository = channelRepository;
            _channelMmrRepository = channelMmrRepository;
            _channelLiessonRepository = channelLiessonRepository;
            _userRepository = userRepository;
            _courseProcessRepository = courseProcessRepository;
            _xueLiRepository = xueLiRepository;
            _gradeRepository = gradeRepository;
            _userAppealRepository = userAppealRepository;
            _pushRepository = pushRepository;
        }

        /// <summary>
        /// 获取职级列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage GetGradeList()
        {
            APIInvokeResult result = new APIInvokeResult();
            var items = _gradeRepository.GetALL();
            var json = new
            {
                totalCount = items.Count(),
                GroupInfoList = items.ToList().Select(s => new
                {
                    Id = s.user_zj_nm,
                    Name = s.user_zj_name
                }
                )
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 获取文章频道列表信息 parentId
        /// </summary>
        /// <param name="parentId">文章分类父级编号</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
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
            var nodes = _articleTypeRepository.GetALL();
            var date = new ResultInfo<CourseChannel>
            {
                TotalCount = nodes.Count(),
                List = nodes.Select(s => new CourseChannel
                {
                    Id = s.ID,
                    Code = "",
                    Name = s.TypeName,
                    NickName = "",
                    Status = "",
                    ParentId = int.Parse(s.ClassPre)==1?0: int.Parse(s.ClassPre),
                    Img = "",
                    CourseChannelList = null,
                    CourseChannel1 = null,
                    PortalChannel = new string[0],
                    PlatFormCourseChannel = new string[0]
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
        [AllowAnonymous]
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
                    ArticleContent = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + "/api/mobile/GetArticleDetail?id=" + s.id, //s.Content.Substring(0, 20) + "...",
                    ArticleChannel = s.Type.ToString(),
                    ArticleCreateDate = s.time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    //ArticleImg = Core.SourcesCore.Build(UpLoadType.ImageArticle, s.Img),
                    ArticleAuthor = s.Author,
                    ClickCount = s.ClickRate != null ? s.ClickRate.ToString() : "0",
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
        [AllowAnonymous]
        public HttpResponseMessage GetArticleDetail(int id)
        {
            //var id = model.Id;
            APIInvokeResult rankge = new APIInvokeResult();
            var article = _articleRepository.GetEntity(m => m.id == id);
            if (article == null)
            {
                return null;
            }
            //更新点击次数
            if (article.ClickRate == null)
            {
                article.ClickRate = 1;
            }
            else
            {
                article.ClickRate += 1;
            }

            _articleRepository.Update(article);
            var json = article != null ? article.content : "";
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                            <meta name='viewport' content='maximum-scale=2.0,minimum-scale=0.5,user-scalable=yes,width=device-width,initial-scale=1.0'></meta>
                            <title>xx</title>
                           <style>.source p img{width:100%}</style>
                        </head>
                        <body>
                            <div style='word-wrap:break-word; text-align:center; font-size:larger; font-weight:blod;'>
                                <span id='ArticleDetail1_lblTitle1' title='标题'>" + article.title + "</span>" +
                               " <br>" +
                               " <br>" +
                            "</div>" +
                            "<div>" +
                                "<span class='ArticleDetail_Ma_Ac_12_info'>发布时间：" + article.time.Value.ToString("yyyy-MM-dd") + "</span><br/><span><span style='float:left;'>作者：" + article.Author + "</span><span style='float:right;'>来源：" + article.source + "</span>" +
                            "</div>" +
                            "</br>" +
                            "<div class='source' style='position:relative;padding-top:10px;word-wrap:break-word;width:100%'>" +
                               "" + json + "" +
                            "</div>" +
                            "<script src='/Scripts/jquery-1.10.2.min.js'></script>" +
                            "<script>" +
                            "$(document).ready(function () {" +
                            "var length = $('.source p img').length;" +
                            "for (var i = 0; i < length; i++)" +
                            "{" +
                            "var src = $('.source p img').eq(i).attr('src');" +
                            "if (src.indexOf('http://www.cszsjy.com') < 0)" +
                              "{" +
                            "var src1 = 'http://www.cszsjy.com' + src;" +
                            "$('.source p img').eq(i).attr('src', src1);" +
                              "}" +
                            "}})" +
                            "</script>" +
                            "</body> </html> ";
            rankge.Data = html;
            return Html(html);
        }
        protected virtual HttpResponseMessage Html(string html)
        {

            HttpResponseMessage result = new HttpResponseMessage();
            result.Content = new StringContent(html, System.Text.Encoding.UTF8, "text/html");

            return result;
        }

        /// <summary>
        /// 获取排行榜
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
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

        /// <summary>
        /// 电子图书分类接口
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetBookSort()
        {
            APIInvokeResult result = new APIInvokeResult();
            var bookTypes = _bookTypeRepository.GetALL()
               .ToList()
               .Select(s => new BookTypeInfo
               {
                   BookTypeId = s.BookTypeID,
                   BookTypeName = s.BookTypeName,
                   ParentId = -1
               })
             .ToList();

            result.Data = bookTypes;
            return result;
        }
        /// <summary>
        /// 验证移动平台当前用户在线状态
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// OnlineFlag=false 离线，true: 在线
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage CheckLoginStatus(MacModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userlog = LoginUserInfo();
            if (!string.IsNullOrEmpty(userlog))
            {
                result.Data = new { OnlineFlag = true };
                return result;
            }
            result.Data = new { OnlineFlag = false };
            return result;
        }
        /// <summary>
        /// 图书列表接口
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetBookInfoList(BookSearch model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (!string.IsNullOrWhiteSpace(model.BookTypeCode))
            {
                var category = _bookTypeRepository.GetEntity(m => m.BookTypeID == model.BookTypeId);

                if (category != null)
                {
                    model.BookTypeId = category.BookTypeID;
                }
                else
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "图书分类不存在!";
                    return result;
                }
            }


            var date = _bookNameRepository.GetBookList(model.Keyword, model.Page, model.Rows, out int totcount, model.BookTypeId);
            var bookinfoLists = date
                .ToList()
                .Select(s => new BookInfo
                {
                    BookNameId = s.BookNameID,
                    BookName = s.Name,
                    BookType = _bookTypeRepository.GetEntity(m => m.BookTypeID == s.BookTypeID).BookTypeName,
                    AutoName = s.AutoName,
                    Content = s.Content,
                    CreateTime = s.CreateTime.Value,
                    BookImg = "",
                    ClickCount = s.ClickCount.Value,
                    BookUrl = "",
                })
                .ToList();

            var json = new ResultInfo<BookInfo>
            {
                TotalCount = totcount,
                List = bookinfoLists
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 图书章节
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetBookChapterInfoList(BookChapterSearch model)
        {
            APIInvokeResult result = new APIInvokeResult();
            //var chapterInfoLists = new List<ChapterInfo>(){
            //    new ChapterInfo{BookTitelID=16730,Title="一、坚持和发展中国特色社会主义",CreateTime=DateTime.Now,ParentID=-1,DownloadUrl="book_340_16730_.txt"},
            //    new ChapterInfo{BookTitelID=16731,Title="二、实现中华民族伟大复兴的中国梦",CreateTime=DateTime.Now,ParentID=-1,DownloadUrl="book_340_16731_.txt"}
            //};

            if (model.Rows == 0)
            {
                model.Rows = int.MaxValue;
            }
            var chapterInfoLists = _bookTitleRepository.GetBookTitleList(model.BookId, model.Page, model.Rows, out int totalCount)
                .ToList()
                .Select(s => new ChapterInfo
                {
                    BookTitelId = s.BookTitleID,
                    Title = s.Title,
                    CreateTime = s.CreateTime.Value,
                    DownloadUrl = "http://www.cszsjy.com/book/11.txt",
                    ParentId = -1
                })
                .ToList();
            var json = new ResultInfo<ChapterInfo>
            {
                TotalCount = totalCount,
                List = chapterInfoLists
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 图书章节内容
        /// </summary>
        /// <param name="bookTitleId">章节编号</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetBookChapterContent(IdModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var bookChapterId = model.Id;

            var date = _bookContentRepository.GetEntity(m => m.BookTitleID == model.Id);
            var json = new
            {
                BookTitleId = bookChapterId,
                BookContentId = date.BookContentID,
                CreateTime = date.CreateTime.Value,
                Content = date.content
            };
            result.Data = json;
            return result;
        }

        /// <summary>
        /// 课程频道列表
        /// </summary>
        /// <param name="parentid">父级频道编号</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetChannelInfoList(ParentIdModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (!string.IsNullOrWhiteSpace(model.ParentCode))
            {
                var channelId = int.Parse(model.ParentCode);
                var channel = _channelRepository.GetEntity(m => m.ChannelID == channelId);
                if (channel != null)
                {
                    model.ParentId = channel.ChannelID;
                }
            }
            var date = _channelRepository.GetChannelList(out int totalCount).Select(s => new CourseCategoryResult
            {
                Id = s.ChannelID,
                ParentId = s.parentid.Value,
                Img = "",
                Name = s.ChannelName
            }
            ).ToList();
            if (model.ParentId == 0)
            {
                date.Insert(0, new CourseCategoryResult
                {
                    Id = -2,
                    Name = "全部",
                    ParentId = 0,
                    Img = ""
                });
            }
            var json = new ResultInfo<CourseChannelInfo>
            {
                TotalCount = totalCount,
                List = date.Select(s => new CourseChannelInfo
                {
                    ChannelId = s.Id,
                    ChannelName = s.Name,
                    ParentId = s.ParentId != 1 ? s.ParentId.ToString() : "0",
                    ImgUrl = "",
                    CourseCount = _channelLiessonRepository.GetEntitiesCount(m => m.Channelid == s.Id).ToString()
                }).ToList()
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 获取课件列表
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetCourseInfoList(CourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (model.ChannelId < 0)
            {
                model.ChannelId = 0;
            }
            if (!string.IsNullOrWhiteSpace(model.ChannelCode))
            {
                var channelId = int.Parse(model.ChannelCode);
                var channel = _channelRepository.GetEntity(m => m.ChannelID == channelId);
                if (channel == null)
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "无此频道";
                    return result;
                }
                model.ChannelId = channel.ChannelID;
            }

            var userid = LoginUserInfo();
            var date = _courseWareRepository.GetCourseWareList(model.Keyword, model.TeacherName, model.ChannelId, model.Page, model.Rows, out int totalcount);

            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    string CourseNumber = _courseWareRepository.GetEntity(p => p.COURSE_ID == s.Id).COURSE_NUMBER;
                    var flag = _courseProcessRepository.IsAny(userid, CourseNumber.ToString());
                    var coursePlayUrl = _channelMmrRepository.Getfirst(s.Id).Mms.Replace("\\", "/");
                    //var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        //string imgtemp = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img;
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = coursePlayUrl.Contains("index.html") ? "H5" : "Mp4",
                            RequiredFlag = s.RequiredFlag ? "1" : "0",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = coursePlayUrl ?? "",
                            Description = s.Description ?? "",
                            Duration = s.Duration.ToString(),
                            TeacherName = s.Teacher,
                            //CourseImg = IsFileExists(imgtemp) ? imgtemp : "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img",
                            CourseImg = "",
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = flag,
                            BrowseScore = s.Learning + "%",
                            AvgScore = s.CommentCredit.ToString(),
                            CourseSize = s.CourseSize ?? "0",
                            ClickCount = s.ClickCount.ToString(),
                            NodeList = new List<NodeInfo>(),
                            //课程分类 
                            ChannelName = s.channelName,
                            Remainder = "",
                            LastNodeId = ""
                        };
                        return ci;
                    }
                })
            };
            result.Data = json;
            return result;
            //return Json(new { totalCount = 0, CourseInfoList = "" });
        }
        /// <summary>
        /// 每周热门
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns> 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage WeekCourse(CourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            model.Sort = "ClickCount";
            model.Order = "desc";
            var userid = LoginUserInfo();
            var date = _courseWareRepository.GetCourseWareList(model.Keyword, model.TeacherName, model.ChannelId, model.Page, model.Rows, out int totalcount);

            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    //var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = "Mp4",
                            RequiredFlag = "1",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = "",
                            Description = s.Description ?? "",
                            Duration = s.Standards.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = _courseProcessRepository.IsAny(userid, s.Id.ToString()),
                            BrowseScore = s.Learning + "%",
                            AvgScore = s.CommentCredit.ToString(),
                            CourseSize = s.CourseSize,
                            ClickCount = s.ClickCount.ToString()
                        };
                        return ci;
                    }
                })
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 获取滚动图片
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// IHttpActionResult {Id:1,Url:"",Icon:"" }
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage GetLink(PageRowsModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            List<LinkModel> link = new List<LinkModel>();
            var ll = new LinkModel
            {
                id = 0,
                Url = "",
                Icon = "http://www.hngbzx.com/Content/Upload/Image/Link/20171113011453982.png"
            };
            link.Add(ll);
            result.Data = link;
            return result;
        }

        /// <summary>
        /// 获取学历列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage GetDegreeList()
        {
            APIInvokeResult result = new APIInvokeResult();
            var items = _xueLiRepository.GetALL();
            var json = new
            {
                totalCount = items.Count(),
                GroupInfoList = items.ToList().Select(s => new
                {
                    Id = s.xueli_id,
                    Name = s.xueli_name
                }
                )
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateMobile(PostMobileModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userid = LoginUserInfo();
            //model.OldMobile = VerifyEncrypt.DesEncrypt(model.OldMobile, SiteGlobalConfig.EncryptKey);
            //model.NewMobile = VerifyEncrypt.DesEncrypt(model.NewMobile, SiteGlobalConfig.EncryptKey);
            var user = _userRepository.GetEntity(m => m.USER_ID == userid);
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.USER_TEL) || user.USER_TEL == model.OldMobile)
                {
                    user.USER_TEL = model.NewMobile;

                    var flag = _userRepository.Update(user);

                    if (flag == true)
                    {
                        result.Message = "修改成功";
                    }
                    else
                    {
                        result.Type = ERRORCODE.Failed;
                        result.Message = "修改失败";
                    }

                }
                else
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "原手机号码错误";
                    // json = JsonHandler.CreateResult(-1, "");
                }
            }
            else
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "用户不存在";
            }
            return result;

        }
        /// <summary>
        /// 相关课程
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage RelatedCourse(RelatedCourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userid = "";
            var date = _courseWareRepository.RelatedCourseList(userid, model.CourseId, model.Page, model.Rows, out int totalcount);
            var jsonr = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    var coursePlayUrl = _channelMmrRepository.Getfirst(s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = "Mp4",
                            RequiredFlag = "1",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = coursePlayUrl ?? "",
                            Description = s.Description ?? "",
                            Duration = s.Standards.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = "未选",
                            BrowseScore = s.Learning + "%",
                            AvgScore = s.CommentCredit.ToString(),
                            CourseSize = s.CourseSize,
                            ClickCount = s.ClickCount.ToString()
                        };
                        return ci;
                    }
                })
            };
            result.Data = jsonr;
            return result;
        }
        /// <summary>
        /// 提交精品课程进度
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SyncUserStudyData(SubmitCourseProcessDataModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var courseid = model.courseId;
            var Data = model.Data;
            //string playingCourseKey = string.Format(CacheKey.PlayingCourseKey, LoginUserId);

            //JYCache.OneCacheMananger().Add(playingCourseKey, courseid);
            var course = _courseWareRepository.GetEntity(m => m.COURSE_ID == courseid);
            if (course == null)
            {
                //return Fail("课程不存在");
                result.Type = ERRORCODE.Failed;
                result.Message = "课程不存在";
                return result;
            }
            //var courseUrl = course.CourseUrl.Where(w => w.Status == CourseUrlStatus.Normal.ToString() && w.Type == CourseUrlType.MobilePlay.ToString())                
            //    .OrderByDescending(o => o.Priority).Select(s => s.Url).DefaultIfEmpty("").FirstOrDefault();
            var courserUrl = _channelMmrRepository.Getfirst(courseid).Mms;
            if (Data != null)
            {
                foreach (var data in Data)
                {
                    CourseProcess entity = new CourseProcess();
                    //string result = string.Empty;
                    entity.Node = data.NodeId.Trim() ?? "S001";
                    entity.Location = (data.Time).ToString();
                    entity.Position = data.Time;
                    entity.Status = data.Status;
                    entity.UserId = LoginUserInfo();
                    //entity.PortalId = PortalId;
                    entity.PortalId = "";
                    entity.CourseId = Convert.ToInt32(courseid);
                    entity.CreateDate = DateTime.Now;
                    entity.SendDate = DateTime.Now;
                    //if (SiteGlobalConfig.CourseProcessLogFlag)
                    //{
                    //	try
                    //	{

                    //		ServicesItems.CourseProcessLogServices.AddEntity(new CourseProcessLog
                    //		{
                    //			PortalId = entity.PortalId,
                    //			UserId = entity.UserId,
                    //			CourseId = entity.CourseId,
                    //			CreateDate = entity.CreateDate,
                    //			SendDate = entity.SendDate,
                    //			Position = entity.Position,
                    //			Location = entity.Location,
                    //			Status = entity.Status,
                    //			Node = entity.Node
                    //		});
                    //	}
                    //	catch (Exception)
                    //	{
                    //	}
                    //}
                    _courseProcessRepository.Insert(new AICC_J_HIGH_SCORE
                    {
                        COURSE_ID = entity.CourseId.ToString(),
                        STUDENT_ID = LoginUserInfo(),
                        OBJ_HIGH_SCORE = "0",
                        OBJ_OBJECTIVE_ID = "1",
                        OBJ_HIGH_STATUS = "1900-01-01",
                        OBJ_FIRST_DATE = DateTime.Now,
                        SESSION_ID = "0",
                        uidcount = -1,
                        timems = entity.Location != null ? int.Parse(entity.Location) : 0,
                        length = Convert.ToInt32(_courseWareRepository.GetEntity(m => m.COURSE_ID == entity.CourseId).duration.Value)


                    });
                    //Core.CourseProcess(LoginUserBatchId, entity, CourseStandards.JYAicc);
                }
            }

            //         var userReg = ServicesItems.UserCourseRegServices.DataFirst(f => f.CourseId == courseid && f.UserId == LoginUserId && f.HistoryFlag == false && f.PortalId == PortalId);

            //if (userReg == null)
            //         {
            //             return Fail("未有选课记录");
            //         }

            //var courseProcess = ServicesItems.CourseProcessServices.DataList(f => f.UserId == LoginUserId && f.CourseId == courseid && f.PortalId == PortalId).ToList();
            var courseProcess = _courseProcessRepository.GetEntity(m => m.COURSE_ID == LoginUserInfo());


            //var cs = courseProcess.OrderByDescending(o => o.SendDate);
            UserStudyInfo usi = new UserStudyInfo
            {
                CourseId = courseProcess.COURSE_ID,
                StartTime = Convert.ToDateTime(courseProcess.OBJ_FIRST_DATE),
                CurrentProgress = courseProcess.timems.ToString(),
                LastLoation = courseProcess.SESSION_ID
            };
            //usi.LastLoation = courseProcess.Select(ss => ss.Position).DefaultIfEmpty(1).FirstOrDefault().ToString();
            //usi.LastNodeId = courseProcess.Select(ss => ss.Node).DefaultIfEmpty("S001").FirstOrDefault();


            //usi.NodeList = course.CourseNode.Select(p =>
            //{
            //    var node = cs.FirstOrDefault(f => f.Node == p.Code.Trim());
            //    var nodestate = string.Empty;
            //    if (node != null)
            //    {
            //        nodestate = CourseNodeReceiveStatus.C.ToString();
            //    }
            //    var nodeid = p.Code.Trim().Substring(1).ToInt32();

            //    return new NodeInfo
            //    {
            //        NodeId = p.Code.Trim(),
            //        Status = node != null ? node.Status : "",
            //        NodeName = p.Name,
            //        Time = node != null ? node.Location : "0",
            //        Length = p.Duration.ToString(),
            //        Date = node != null ? node.CreateDate.ToString("yyyyMMddHHmmss") : "",
            //        Mp4 = courseUrl + "/Content" + nodeid + ".mp4"
            //    };
            //}).ToList();
            result.Data = usi;
            return result;
        }

        /// <summary>
        /// 提交MP4进度
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UploadTimeNode(Mp4CourseProcessDataModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var courseid = model.CourseId;
            //string playingCourseKey = string.Format(CacheKey.PlayingCourseKey, LoginUserId);

            //JYCache.OneCacheMananger().Add(playingCourseKey, courseid);
            //var course = ServicesItems.CourseServices.GetCourse(courseid);
            var course = _courseWareRepository.GetEntity(m => m.COURSE_ID == courseid);
            string coruse_number = course.COURSE_NUMBER;
            var userid = LoginUserInfo();
            var userNm = _userRepository.GetEntity(j => j.USER_ID == userid).USER_NM;
            decimal time = 0;
            var videoLength = _channelMmrRepository.GetEntity(n => n.CourseID == courseid);
            var len = videoLength.Video_Length > 0 ? videoLength.Video_Length : 100;
            if (course != null && !string.IsNullOrEmpty(model.TimeNode))
            {
                var h = Convert.ToDecimal(model.TimeNode.Substring(0, 2));
                var m = Convert.ToDecimal(model.TimeNode.Substring(2, 2));
                var s = Convert.ToDecimal(model.TimeNode.Substring(4, 2));
                time = h * 3600 + m * 60 + s;
                //Debug.WriteLine(time);
                //  Console.WriteLine(time + ":::::");
                CourseProcess process = new CourseProcess
                {
                    //PortalId = PortalId,
                    PortalId = "",
                    UserId = LoginUserInfo(),
                    Position = time,
                    CourseId = courseid
                };

                var courseProcess = _courseProcessRepository.GetEntity(mm => mm.COURSE_ID == coruse_number && mm.STUDENT_ID == userid);
                if (courseProcess != null)
                {

                    courseProcess.timems = int.Parse(time.ToString());
                    _courseProcessRepository.Update(courseProcess);
                }
                else
                {
                    AICC_J_HIGH_SCORE pp = new AICC_J_HIGH_SCORE
                    {
                        COURSE_ID = coruse_number,
                        length = int.Parse(len.ToString()),
                        OBJ_FIRST_DATE = DateTime.Now,
                        OBJ_HIGH_SCORE = "0",
                        OBJ_OBJECTIVE_ID = "1",
                        SESSION_ID = "0",
                        STUDENT_ID = userid,
                        timems = int.Parse(time.ToString())
                    };
                    _courseProcessRepository.Insert(pp);

                }

            }
            var courseProcesss = _courseProcessRepository.GetEntity(mm => mm.COURSE_ID == coruse_number && mm.STUDENT_ID == userid);

            UserStudyInfo usi = new UserStudyInfo();
            usi.CourseId = courseid.ToString();
            usi.StartTime = courseProcesss.OBJ_FIRST_DATE != null ? Convert.ToDateTime(courseProcesss.OBJ_FIRST_DATE) : Convert.ToDateTime("1900-01-01");
            usi.CurrentProgress = ((decimal)courseProcesss.timems.Value / (decimal)videoLength.Video_Length.Value).ToString("F2");
            usi.LastLoation = courseProcesss.timems.ToString();
            usi.LastNodeId = "0";
            usi.NodeList = new List<NodeInfo>();
            result.Data = usi;
            return result;
        }

        /// <summary>
        /// 获取课程详细
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCourseDetail(IdModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (model.Id == 0)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "请求错误";
                return result;
            }
            var userLoginId = LoginUserInfo();
            var userid = _userRepository.GetEntity(m => m.USER_ID == userLoginId).USER_NM;
            var course = _courseWareRepository.GetCourserModel(model.Id, userLoginId);
            if (course == null)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "课程已下线";
                return result;
            }
            var courseDownloadUrl = "";
            var coursePlayUrl = _channelMmrRepository.Getfirst(course.Id).Mms.Replace("\\", "/");
            var videoLength = _channelMmrRepository.Getfirst(model.Id);

            var len = videoLength.Video_Length > 0 ? videoLength.Video_Length : 100;

            string coursenumber = _courseWareRepository.GetEntity(p => p.COURSE_ID == model.Id).COURSE_NUMBER;
            if (_courseProcessRepository.IsAny(LoginUserInfo(), coursenumber) == "未选")
            {
                AICC_J_HIGH_SCORE pp = new AICC_J_HIGH_SCORE
                {
                    COURSE_ID = coursenumber,
                    length = int.Parse(len.ToString()),
                    OBJ_FIRST_DATE = DateTime.Now,
                    OBJ_HIGH_SCORE = "0",
                    OBJ_OBJECTIVE_ID = "1",
                    SESSION_ID = "0",
                    STUDENT_ID = LoginUserInfo(),
                    timems = 1
                };
                _courseProcessRepository.Insert(pp);
            }

            var json = new CourseInfo
            {
                CourseId = course.Id.ToString(),
                CourseName = course.Name,
                CourseType = coursePlayUrl.Contains("index.html") ? "H5" : "Mp4",
                RequiredFlag = "1",
                Credit = course.Credit.ToString("0.0"),
                CreateDate = course.CreateDate,
                DownloadUrl = courseDownloadUrl,
                DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                OnlineUrl = coursePlayUrl ?? "",
                Description = course.Description,
                Duration = ((decimal)len / 60).ToString("F0") + '.' + ((decimal)len % 60).ToString("F0"),
                TeacherName = course.Teacher,
                CourseImg = "http://www.cszsjy.com/Fwadmin/Manager/Admin/Lession/Save/" + course.Img,
                ExamId = course.Exam,
                CommentCredit = Convert.ToSingle(course.CommentCredit).ToString(),
                SelectFlag = _courseProcessRepository.IsAny(userLoginId, coursenumber),
                BrowseScore = course.Learning + "%",
                UserCount = 0,
                //课程分类 
                ChannelName = course.channelName,
                Remainder = ""

            };
            //获取进度
            var entity = _courseProcessRepository.GetEntity(m => m.COURSE_ID == coursenumber && m.STUDENT_ID == userLoginId);
            var lastpostion = entity != null ? entity.timems.Value : 0;
            var lastnodeid = "S001";





            //最后的位置
            json.LastLocation = lastpostion;////TODO 2015-02-28 09:24:08 添加LastNodeID
            json.BrowseScore = entity != null ? ((decimal)lastpostion / (decimal)len * 100).ToString("F2") + "%" : "0%";
            json.LastNodeId = lastnodeid ?? "S001";// lastnodeid,
            //评分
            json.AvgScore = course.Score;
            json.CourseSize = course.CourseSize ?? "0";
            json.ClickCount = course.ClickCount.ToString();
            result.Data = json;
            return result;
        }


        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <param name="parentId">父级编号</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage GetGroupList(ParentIdModel model)
        {
            var date = _userGroupRepository.GetALL();
            APIInvokeResult result = new APIInvokeResult();
            if (model.ParentId > 0)
            {
                date = date.Where(m => m.G_ParentID == model.ParentId);
            }

            var json = new
            {
                totalCount = date.Count(),
                GroupInfoList = date.ToList().Select(s => new
                {
                    UserGroupId = s.USER_GROUP_ID,
                    UserGroupName = s.USER_GROUP_NAME
                }).ToList()
            };
            result.Data = json;
            return result;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="userLogin"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ValidateUser(AppLoginModel model)
        {

            try
            {
                APIInvokeResult result = new APIInvokeResult();
                if (model.Account == null)
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "输入用户名";
                    return result;
                }
                var user = _userRepository.GetEntity(m => m.USER_ID == model.Account);
                if (user == null)
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "用户不存在";
                    return result;

                }
                if (user.USER_PWD != (model.Password))
                {
                    result.Type = ERRORCODE.Failed;
                    result.Message = "密码错误";
                    return result;

                }

                UserInfo userInfo = new UserInfo
                {
                    Birthday = "",
                    DegreeName = user.EName ?? "",
                    Degree = user.xueli_id != null ? user.xueli_id.Value.ToString() : "",
                    DepartmentName = "",
                    ElectiveCredit = "0",
                    Email = user.USER_EMAIL != null ? user.USER_EMAIL : "",
                    ExamCredit = "0",
                    Grade = "",
                    GradeName = "",
                    GroupId = 0,
                    GroupName = "",
                    IdCard = user.id_card ?? "",
                    Learncount = "0",
                    IsFirstLogin = "1",
                    Mobile = user.USER_TEL != null ? user.USER_TEL : "",
                    Username = user.USER_NAME,
                    MobileCredit = "",
                    Nation = "",
                    NeedCredit = "",
                    NeedRequiredCredit = "",
                    Party = "",
                    PassFlag = true,
                    PcCredit = "",
                    Portal = "",
                    PortalName = "",
                    RequiredCredit = "",
                    Roles = "",
                    ScoreRank = "",
                    Sex = "男",
                    Tel = user.USER_TEL ?? "",
                    TotalCredit = "",
                    TotalUserCount = 0,
                    TrainningCredit = "",
                    UserAccount = user.USER_ID,
                    UserZW = ""

                };
                //保存身份票据
                SetUserAuth(user.USER_ID, false);
                //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, entity.USER_NAME, DateTime.Now,
                //      DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", entity.USER_NAME, entity.USER_PWD),
                //      FormsAuthentication.FormsCookiePath);


                //保存登录名
                result.Message = "登录成功";
                result.Data = userInfo;
                return result;
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.ToString()),
                    ReasonPhrase = "error"
                };
                throw new HttpResponseException(resp);
            }

        }



        /// <summary>
        /// 未读消息数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetMessageUnreadCount()
        {
            APIInvokeResult result = new APIInvokeResult();
            result.Data = new { UnreadCount = 0 };
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
        [AllowAnonymous]
        public HttpResponseMessage Register(PostRegisterModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var isuser = _userRepository.GetEntities(m => m.USER_ID == model.Account).ToList();
            if (isuser.Count() > 0)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "用户名重复";
                return result;
            }

            var regUserModel = new USERS
            {
                USER_ID = model.Account,
                USER_PWD = model.Password,
                USER_TEL = model.Mobile,
                USER_NAME = model.Name,
                USER_GROUP_ID = model.GroupId,
                id_card = model.IdCard,
                USER_TYPE = 1,//1表示前台用户
                user_state = 0,//0表示正常
                LAST_LOGIN_TIME = DateTime.Now,
                LOGIN_TIMES = 0,
                zj = model.Grade
            };
            //注册表  由于用户表没有注册时间 
            var userAppeal = new UserAppeal
            {
                USER_ID = model.Account,
                USER_PWD = model.Password,
                USER_TEL = model.Mobile,
                USER_NAME = model.Name,
                id_card = model.IdCard,
                time = DateTime.Now,
            };
            bool symbol = false;

            int flag = _userRepository.Resister(regUserModel);

            symbol = _userAppealRepository.Insert(userAppeal);


            if (flag > 0 && symbol)
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
        /// 获取用户信息学时等。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserInfo()
        {
            var userid = LoginUserInfo();
            var user = _userRepository.GetEntity(m => m.USER_ID == userid);
            APIInvokeResult result = new APIInvokeResult();
            if (user == null)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "用户不存在";
                return result;
            }

            var usercrd = _userRepository.UserCreModel(user.USER_ID);
            UserInfo userInfo = new UserInfo
            {
                Birthday = "",
                DegreeName = user.EName ?? "",
                Degree = user.xueli_id != null ? user.xueli_id.Value.ToString() : "",
                DepartmentName = "",
                ElectiveCredit = usercrd.ToString(),
                Email = user.USER_EMAIL != null ? user.USER_EMAIL : "",
                ExamCredit = "0",
                Grade = user.zj != null && user.zj != 0 ? user.zj.ToString() : "",
                GradeName = user.zj != null && user.zj != 0 ? _gradeRepository.GetEntity(m => m.user_zj_nm == user.zj).user_zj_name : "",
                GroupId = user.USER_GROUP_ID != null && user.USER_GROUP_ID != 0 ? int.Parse(user.USER_GROUP_ID.ToString()) : 0,
                GroupName = user.USER_GROUP_ID != null && user.USER_GROUP_ID != 0 ? _userGroupRepository.GetEntity(m => m.USER_GROUP_ID == user.USER_GROUP_ID).USER_GROUP_NAME : "",
                IdCard = user.id_card ?? "",
                Learncount = "0",
                IsFirstLogin = "1",
                Mobile = user.USER_TEL != null ? user.USER_TEL : "",
                Username = user.USER_NAME,
                MobileCredit = usercrd.ToString(),
                Nation = "",
                NeedCredit = "",
                NeedRequiredCredit = "",
                Party = "",
                PassFlag = true,
                PcCredit = usercrd.ToString(),
                Portal = "",
                PortalName = "",
                RequiredCredit = usercrd.ToString(),
                Roles = "",
                ScoreRank = "",
                Sex = "男",
                Tel = user.USER_TEL ?? "",
                TotalCredit = usercrd.ToString(),
                TotalUserCount = 0,
                TrainningCredit = "",
                UserAccount = user.USER_ID,
                UserZW = ""

            };



            result.Data = userInfo;
            return result;
            //return Json(json);
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
                usermodel.USER_TEL = model.Tel;
                usermodel.USER_GROUP_ID = model.GroupId;
                usermodel.xueli_id = model.Degree;
                usermodel.zj = model.Grade;
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
        [UserAuthorize]
        public HttpResponseMessage SetUserPassword(SetPasswordModel model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userid = LoginUserInfo();
            var usermodel = _userRepository.GetEntity(m => m.USER_ID == userid);
            if (usermodel.USER_PWD != model.OldPassword)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "旧密码输入错误，请重新输入";
                return result;
            }
            usermodel.USER_PWD = model.Password;
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
        [UserAuthorize]
        public HttpResponseMessage GetUserCourseInfoList(UserCourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            List<CourserWareModel> list;
            int count = 0;
            var id = LoginUserInfo();

            list = _courseWareRepository.RelatedFinishCourseList(id, model.Finish, model.Page, model.Rows, out int totalcount);
            count = totalcount;


            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = count,
                List = list.Select((s, i) =>
                {
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
                        CourseType = "Mp4",
                        CreateDate = s.CreateDate,
                        FinishDate = DateTime.Now,
                        BrowseScore = ((decimal)s.Times / (decimal)(s.Length>0? s.Length :100) * 100).ToString("F2") + "%",
                        LastLocation = 0,////TODO 2015-02-28 09:24:08 添加LastNodeID
                        LastNodeId = lastnodeid ?? "S001",// lastnodeid,
                        TeacherName = s.Teacher,
                        Description = s.Description,
                        Duration = s.Length.ToString(),
                        OnlineUrl = onlineurl,// online != null ? online.Url : "",
                        CourseImg = "",
                        ChannelName = s.channelName,
                        RequiredFlag = "",
                        DownloadUrl = "",
                        DownloadUrlLow = "",
                        ExamId = "",
                        CommentCredit = "",
                        SelectFlag = "已选",
                        AvgScore = "0",
                        ClickCount = s.ClickCount.ToString(),
                        CourseSize = "",
                        Remainder = (s.Length - s.Times).ToString()
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

        [HttpPost]
        [AllowAnonymous]
        /// <summary>
        /// 获取推送消息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetMessageCenter(ApiMessage model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var list = _pushRepository.GetListByPage(model.Keyword, model.Page, model.Rows, out int total);

            var json = new ResultInfo<ApiMessageResult>
            {
                TotalCount = total,
                List = list.Select(p =>
                {
                    return new ApiMessageResult
                    {
                        Id = p.Id,
                        Content = p.Conetent,
                        Createdate = p.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                        ReadFlag = "False",
                        Title = p.Name,
                        Type = "Notice"
                    };
                }).ToList()
            };
            result.Data = json;
            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetNoticeInfoContent(int id)
        {
            APIInvokeResult result = new APIInvokeResult();
            var model = _pushRepository.GetEntity(p => p.Id == id);
            var json = model != null ? model.Conetent : "";
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                            <meta name='viewport' content='maximum-scale=2.0,minimum-scale=0.5,user-scalable=yes,width=device-width,initial-scale=1.0'></meta>
                            <title>xx</title>
                           <style>.source p img{width:100%}</style>
                        </head>
                        <body>
                             </div>" + 
                            "<div class='source' style='position:relative;padding-top:10px;word-wrap:break-word;width:100%'>" +
                               "" + json + "" +
                            "</div>" +
                            "</body> </html> ";
            result.Data = html;
            return Html(html);
        }

        private bool IsFileExists(string furl)
        {
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(furl);
                HttpWebResponse myRes = (HttpWebResponse)myReq.GetResponse();
                if (myRes.ContentLength > 0)
                {
                    myRes.Close();
                    return true;
                }
                else
                {
                    myRes.Close();
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
