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

namespace Community.WebApi.Controllers
{
    public class CourseWareController : BaseController
    {

        readonly ICourseWareRepository _courseWareRepository;
        readonly IChannelRepository _channelRepository;
        readonly IChannelMmrRepository _channelMmrRepository;
        readonly IChannelLiessonRepository _channelLiessonRepository;
        readonly IUserRepository _userRepository;
        readonly ICourseProcessRepository _courseProcessRepository;
        public CourseWareController(
            ICourseWareRepository courseWareRepository,
            IChannelRepository channelRepository,
            IChannelMmrRepository channelMmrRepository,
            IChannelLiessonRepository channelLiessonRepository,
            IUserRepository userRepository,
            ICourseProcessRepository courseProcessRepository
            )
        {
            _courseWareRepository = courseWareRepository;
            _channelRepository = channelRepository;
            _channelMmrRepository = channelMmrRepository;
            _channelLiessonRepository = channelLiessonRepository;
            _userRepository = userRepository;
            _courseProcessRepository = courseProcessRepository;
        }

        /// <summary>
        /// 课程频道列表
        /// </summary>
        /// <param name="parentid">父级频道编号</param>
        /// <returns></returns> 
        [HttpPost]
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
                    ParentId = 1,
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
                    ParentId = s.ParentId.ToString(),
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
        public HttpResponseMessage GetCourseInfoList(CourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            if (model.ChannelId < 0)
            {
                model.ChannelId = 1;
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
            var loginid = LoginUserInfo();
            var userid = _userRepository.GetEntity(m => m.USER_ID == loginid).USER_NM;
            var date = _courseWareRepository.GetCourseWareList(model.Keyword, model.TeacherName, model.ChannelId, model.Page, model.Rows, out int totalcount);
            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = s.Standards.ToString(),
                            RequiredFlag = s.RequiredFlag ? "1" : "0",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = coursePlayUrl != null ? coursePlayUrl : "",
                            Description = s.Description == null ? "" : s.Description,
                            Duration = s.Duration.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = s.Learning >= 0 ? "已选" : "未选",
                            BrowseScore = s.Learning >= 0 ? s.Learning.ToString("#0.00") + "%" : "0%",
                            AvgScore = s.CommentCredit.ToString(),
                            CourseSize = s.CourseSize ?? "0",
                            ClickCount = s.ClickCount.ToString(),
                            NodeList = new List<NodeInfo>(),
                            //课程分类 
                            ChannelName = s.channelName
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
        public HttpResponseMessage WeekCourse(CourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            model.Sort = "ClickCount";
            model.Order = "desc";
            var userid = _userRepository.GetEntity(m => m.USER_ID == LoginUserInfo()).USER_NM;
            var date = _courseWareRepository.GetCourseWareList(model.Keyword, model.TeacherName, model.ChannelId, model.Page, model.Rows, out int totalcount);
            var json = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = s.Standards.ToString(),
                            RequiredFlag = "1",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = coursePlayUrl ?? "",
                            Description = s.Description ?? "",
                            Duration = s.Standards.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = s.Learning >= 0 ? "已选" : "未选",
                            BrowseScore = s.Learning >= 0 ? s.Learning.ToString("#0.00") + "%" : "0%",
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
        /// 相关课程
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RelatedCourse(RelatedCourseModelP model)
        {
            APIInvokeResult result = new APIInvokeResult();
            var userid = LoginUserInfo();
            var date = _courseWareRepository.RelatedCourseList(userid, model.CourseId, model.Page, model.Rows, out int totalcount);
            var jsonr = new ResultInfo<CourseInfo>
            {
                TotalCount = totalcount,
                List = date.Select(s =>
                {
                    var courseDownloadUrl = "";
                    var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == s.Id).Mms;
                    {
                        var ci = new CourseInfo
                        {
                            CourseId = s.Id.ToString(),
                            CourseName = s.Name,
                            CourseType = s.Standards.ToString(),
                            RequiredFlag = "1",
                            Credit = s.Credit.ToString("0.0"),
                            CreateDate = s.CreateDate,
                            DownloadUrl = courseDownloadUrl,
                            DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                            OnlineUrl = coursePlayUrl ?? "",
                            Description = s.Description ?? "",
                            Duration = s.Standards.ToString(),
                            TeacherName = s.Teacher,
                            CourseImg = s.Img,
                            ExamId = s.Exam,
                            CommentCredit = Convert.ToSingle(s.CommentCredit).ToString(),
                            SelectFlag = s.Learning >= 0 ? "已选" : "未选",
                            BrowseScore = s.Learning >= 0 ? s.Learning.ToString("#0.00") + "%" : "0%",
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
            var courserUrl = _channelMmrRepository.GetEntity(w => w.CourseID == courseid);
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
                    entity.UserId = LoginUserInfo() ;
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
						STUDENT_ID = _userRepository.GetEntity(m => m.USER_ID == LoginUserInfo()).USER_NM.ToString(),
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
            decimal time = 0;
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
                //if (SiteGlobalConfig.CourseProcessLogFlag)
                //{
                //    try
                //    {
                //        ServicesItems.CourseProcessLogServices.AddEntity(new CourseProcessLog
                //        {
                //            PortalId = process.PortalId,
                //            UserId = process.UserId,
                //            CourseId = process.CourseId,
                //            CreateDate = process.CreateDate,
                //            SendDate = process.SendDate,
                //            Position = process.Position,
                //            Location = process.Location,
                //            Status = process.Status,
                //            Node = process.Node
                //        });
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}
                //if (Core.CourseProcess(LoginUserBatchId, process, CourseStandards.Mp4) <= 0)
                //{
                //    return Fail("提交失败");
                //}
            }

            //QueryUserCourseReg query = new QueryUserCourseReg();
            //query.CourseId = courseid;
            //query.UserId = LoginUserId;
            //query.HistoryFlag = false;
            //query.PortalId = PortalId;
            //var userReg = ServicesItems.UserCourseRegServices.DataList(1, int.MaxValue, "Id", "Desc", query).FirstOrDefault();
            //if (userReg == null)
            //{
            //    return Fail("未有选课记录");
            //}

            //QueryCourseProcess query2 = new QueryCourseProcess();
            //query2.UserId = LoginUserInfo();
            //query2.PortalId = "";
            //query2.CourseId = courseid;
            //var courseProcess = ServicesItems.CourseProcessServices.DataList(1, int.MaxValue, "Id", "desc", query2).ToList();
			var courseProcess = _courseProcessRepository.GetEntity(m => m.COURSE_ID == courseid.ToString());
			//var cs = courseProcess.Where(w => w.CourseId == userReg.CourseId).OrderByDescending(o => o.SendDate);
            UserStudyInfo usi = new UserStudyInfo();
			usi.CourseId = courseid.ToString();
			usi.StartTime = courseProcess.OBJ_FIRST_DATE!=null? Convert.ToDateTime(courseProcess.OBJ_FIRST_DATE) :Convert.ToDateTime("1900-01-01"); 
			usi.CurrentProgress = courseProcess.timems.ToString();
            usi.LastLoation = courseProcess.SESSION_ID;
			//usi.LastNodeId = cs.Select(ss => ss.Location).DefaultIfEmpty("0").FirstOrDefault();
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
            var userid = _userRepository.GetEntity(m => m.USER_ID == LoginUserInfo()).USER_NM;
            var course = _courseWareRepository.GetCourserModel(model.Id, LoginUserInfo());
            if (course == null)
            {
                result.Type = ERRORCODE.Failed;
                result.Message = "课程已下线";
                return result;
            }
            var courseDownloadUrl = "";
            var coursePlayUrl = _channelMmrRepository.GetEntity(m => m.CourseID == course.Id).Mms;
            var json = new CourseInfo
            {
                CourseId = course.Id.ToString(),
                CourseName = course.Name,
                CourseType = course.Type,
                RequiredFlag = "1",
                Credit = course.Credit.ToString("0.0"),
                CreateDate = course.CreateDate,
                DownloadUrl = courseDownloadUrl,
                DownloadUrlLow = courseDownloadUrl.Replace(".mp4", "_low.mp4"),
                OnlineUrl = coursePlayUrl ?? "",
                Description = course.Description,
                Duration = course.CourseSize.ToString(),
                TeacherName = course.Teacher,
                CourseImg = course.Img,
                ExamId = course.Exam,
                CommentCredit = Convert.ToSingle(course.CommentCredit).ToString(),
                SelectFlag = course.Learning >= 0 ? "已选" : "未选",
                BrowseScore = course.Learning >= 0 ? course.Learning.ToString("#0.00") + "%" : "0%",
                UserCount = 0,
                //课程分类 
                ChannelName = course.channelName
            };
            //获取进度
            var entity = _courseProcessRepository.GetEntity(m => m.COURSE_ID == json.CourseId && m.STUDENT_ID == userid.ToString());
            var lastpostion = entity != null ? entity.timems.Value : 0;
            var lastnodeid = "S001";
            //最后的位置
            json.LastLocation = lastpostion;////TODO 2015-02-28 09:24:08 添加LastNodeID
            json.LastNodeId = lastnodeid ?? "S001";// lastnodeid,
            //评分
            json.AvgScore = course.Score;
            json.CourseSize = course.CourseSize ?? "0";
            json.ClickCount = course.ClickCount.ToString();
            result.Data = json;
            return result;
        }
    }
}
