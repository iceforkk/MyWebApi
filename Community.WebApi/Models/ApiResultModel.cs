using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Community.WebApi.Models
{
    #region User

    public class UserWechatModel
    {
        public int UserId { get; set; }

        public string OpenId { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserAccount { get; set; }
    }


    public class UserOtherInfoModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ClientId { get; set; }
        public string UserAccount { get; set; }
    }
    public partial class OutlineTrainingCreditModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>

        public int UserId { get; set; }

        [Required]
        [Display(Name = "账号")]
        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string GroupName { get; set; }

        public decimal OtherPcCredit { get; set; }

        public string Item { get; set; }

        public string TrainingName { get; set; }

        public string Media { get; set; }

        public string CreateDate { get; set; }

        public decimal OutlineCredit { get; set; }

        public decimal DeductCredit { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }
    }

    #endregion User

    #region Notice

    public class NoticeInfo
    {
        public int NoticeId { get; set; }

        public string NoticeTitle { get; set; }

        public string NoticeContent { get; set; }

        public int ContentId { get; set; }

        public DateTime NoticeCreatedate { get; set; }

        public string NoticeImg { get; set; }

        public string NoticeAuthor { get; set; }

        public string Attachment { get; set; }

        public string Description { get; set; }
    }

    public class MyNoticeInfo : NoticeInfo
    {
        public bool ReadFlag { get; set; }

        public string NoticeType { get; set; }
    }

    public class MessageCenterResult
    {
        /// <summary>
        /// 消息总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 未读数量
        /// </summary>
        public int NotReadCount { get; set; }

        /// <summary>
        /// 消息列表
        /// </summary>
        public IEnumerable<MessageCenterDetail> List { get; set; }
    }

    public class MessageCenterDetail
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 内容编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool ReadFlag { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime Createdate { get; set; }
    }

    #endregion Notice

    #region Training

    public class TrainingUserSignInTable
    {
        /// <summary>
        /// 培训班编号
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// 今天是否需要签到
        /// </summary>
        public bool OpenFlag { get; set; }

        /// <summary>
        /// 上午签到
        /// </summary>
        public SignDetail AmSignIn { get; set; }

        /// <summary>
        /// 上午签退
        /// </summary>
        public SignDetail AmSignOut { get; set; }

        /// <summary>
        /// 下午签到
        /// </summary>
        public SignDetail PmSignIn { get; set; }

        /// <summary>
        /// 下午签退
        /// </summary>
        public SignDetail PmSignOut { get; set; }

        /// <summary>
        /// 签到码
        /// </summary>
        public string SignInCode { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignInDate { get; set; }
    }

    public class SignDetail
    {
        public string SignType { get; set; }

        public TimeSpan BeginTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool SignFlag { get; set; }

        public string RequireSignTime { get; set; }

        public DateTime SignDate { get; set; }
    }

    #endregion Training

    #region Comment

    public class CommentResult
    {
        public CommentResult()
        {
            this.List = new List<CommentResult>();
        }

        /// <summary>
        /// 标题，讨论无标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型码
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 评论Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 主体Id
        /// </summary>
        public int MainId { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 点赞次数
        /// </summary>
        public int TopNum { get; set; }

        /// <summary>
        /// 被回复学员名称
        /// </summary>
        public string ReplyUserName { get; set; }

        /// <summary>
        /// 评论日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public List<CommentResult> List { get; set; }
    }

    #endregion Comment

    #region Guid

    public class UserUserSkillResult
    {
        public int SkillId { get; set; }

        public string SkillCode { get; set; }

        public string SkillName { get; set; }

        public decimal SkillValue { get; set; }

        public decimal RequiredValue { get; set; }
    }

    #endregion Guid

    #region Summary

    public class TimeSummaryModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 平台编号
        /// </summary>
        public int PortalId { get; set; }

        /// <summary>
        /// 在线人数
        /// </summary>
        public int OnlineCount { get; set; }
    }

    public class DaySummaryModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 平台编号
        /// </summary>
        [Display(Name = "平台编号")]
        public int PortalId { get; set; }

        /// <summary>
        /// App使用人数
        /// </summary>
        public int AppUserCount { get; set; }

        /// <summary>
        /// 访问人数
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 微信绑定人数
        /// </summary>
        public int WechatBindCount { get; set; }
    }

    public class RealTimeDataModel
    {
        /// <summary>
        /// 每小时登录情况
        /// </summary>
        public List<TimeSummaryModel> TimeSummary { get; set; }

        /// <summary>
        /// 每天汇总情况 访问人数 微信绑定人数等
        /// </summary>
        public List<DaySummaryModel> DaySummary { get; set; }

        /// <summary>
        /// 课程数量
        /// </summary>
        public int CourseCount { get; set; }

        /// <summary>
        /// 注册人数
        /// </summary>
        public int UserCount { get; set; }
    }

    #endregion Summary

    public class CourseChannel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }
        public string Code { get; internal set; }
        public string NickName { get; internal set; }
        public string Status { get; internal set; }
        public string Img { get; internal set; }
        public object CourseChannelList { get; internal set; }
        public object CourseChannel1 { get; internal set; }
        public object PortalChannel { get; internal set; }
        public string[] PlatFormCourseChannel { get; internal set; }
    }
    public class ArticleInfo
    {
        public int ArticleId { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string ArticleChannel { get; set; }
        public object ArticleCreateDate { get; set; }
        public object ArticleAuthor { get; set; }

        public string ClickCount { get; set; }

        public string Description { get; set; }

        public string Rcount { get; set; }

        public string Tag { get; set; }
    }
    public class RankInfo
    {

        public int index { get; set; }

        public string name { get; set; }

        public string value { get; set; }

    }
    public class BookTypeInfo
    {
        public int BookTypeId { get; set; }
        public string BookTypeName { get; set; }

        public int ParentId { get; set; }

    }
    public class BookInfo
    {
        public int BookNameId { get; set; }
        public string BookName { get; set; }
        public string BookType { get; set; }
        public string AutoName { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string BookImg { get; set; }
        public int ClickCount { get; set; }
        public string BookUrl { get; set; }
    }
    public class ChapterInfo
    {
        public int BookTitelId { get; internal set; }
        public string Title { get; internal set; }
        public DateTime CreateTime { get; internal set; }
        public string DownloadUrl { get; internal set; }
        public int ParentId { get; internal set; }
    }

    public class CourseChannelInfo
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; internal set; }
        public string ParentId { get; internal set; }
        public string ImgUrl { get; internal set; }
        public string CourseCount { get; internal set; }
    }


    public class CourseCategoryResult
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int ParentId { get; internal set; }
        public string Img { get; internal set; }
    }

    public class CourseProcess
    {
        public string Node { get; set; }
        public string Location { get; set; }
		public decimal Position { get; internal set; }
		public string Status { get; internal set; }
		public string UserId { get; internal set; }
		public string PortalId { get; internal set; }
		public int CourseId { get; internal set; }
		public DateTime CreateDate { get; internal set; }
		public DateTime SendDate { get; internal set; }
	}

	public class QueryCourseProcess
	{
		public string UserId { get; internal set; }
		public string PortalId { get; internal set; }
		public int CourseId { get; internal set; }
	}
    public class ApiMessageResult
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
        
        public string Type { get; set; }

        public string ReadFlag { get; set; }

        public string Createdate { get; set; }

    }
}