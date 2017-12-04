using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Community.WebApi.Models
{
    public class WechatLogin : AppLoginModel
    {
        /// <summary>
        /// 微信票据
        /// </summary>
        public string Code { get; set; }
    }

    public class WechatAppModel
    {
        public string url { get; set; }
    }

    public class PostRegisterModel
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary> 
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// 单位 编号
        /// </summary> 
        public int GroupId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 职级 编号
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string SmgCode { get; set; }

        public int? PortalId { get; set; }
    }

    public class UpdateUserModel
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>

        public string DepartmentName { get; set; }

        /// <summary>
        /// 单位 编号
        /// </summary>
     
        public int GroupId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IdCard { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 职级 编号
        /// </summary>
      
        public int Grade { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
       
        public string Business { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
      
        public string Sex { get; set; }

        /// <summary>
        /// 名族
        /// </summary>
       
        public string Nation { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
      
        public int Degree { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
     
        public string Party { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 办公室电话
        /// </summary>
      
        public string Tel { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        //[Required]
        //public string SmgCode { get; set; }
    }

    public class AppLoginModel
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string CId { get; set; }
    }

    /// <summary>
    /// 忘记密码
    /// </summary>
    public class PostPwdModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string SmgCode { get; set; }
    }

    /// <summary>
    /// 修改手机号码
    /// </summary>
    public class PostMobileModel
    {
        /// <summary>
        /// 旧手机号码
        /// </summary>
        public string OldMobile { get; set; }

        /// <summary>
        /// 新号码
        /// </summary>
        public string NewMobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string SmgCode { get; set; }
    }

    public class PageRowsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRowsModel"/> class.
        /// </summary>
        public PageRowsModel()
        {
            Page = 1;
            Rows = 10;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { get; set; }
    }

    public class PageSortModel : PageRowsModel
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 排序方向 （asc desc）
        /// </summary>
        public string Order { get; set; }
    }

    public class KeywordModel : PageRowsModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    public class NoticeInfoModelP : PageRowsModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    public class PageSortKey : PageSortModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    public class ArticleModelP : KeywordModel
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类编码 如果传了该值 则CategoryId无效。
        /// </summary>
        public string CategoryCode { get; set; }
    }

    public class CourseModelP : PageSortKey
    {
        /// <summary>
        /// 教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 课程频道Id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 课程频道编码，默认不传，特殊情况下才传
        /// </summary>
        public string ChannelCode { get; set; }
    }

    public class HistoryCourseModelP : PageSortKey
    {
        /// <summary>
        /// All:表示所有， Today:表示今天记录，Earlier:表示更早
        /// </summary>
        public string Type { get; set; }
    }

    public class TrainingCourseModelP : PageSortKey
    {
        /// <summary>
        /// 关键字
        /// </summary>
        //public string Keyword { get; set; }

        /// <summary>
        /// 培训班编号
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// 教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 是否完成 1是； 0|不传 否
        /// </summary>
        public int? Finish { get; set; }
    }

    public class UserCourseModelP : PageRowsModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 1：完成课程，0：未完成课程 ,2：表示全部
        /// </summary>
        public int Finish { get; set; }
    }

    public class CourseCommentModelP : PageRowsModel
    {
        public CourseCommentModelP()
        {
            Rows = 20;
        }

        /// <summary>
        /// 课程编号
        /// </summary>
        public int courseId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    public class AddCourseCommentModelP
    {
        public AddCourseCommentModelP()
        {
            Score = 5;
        }

        /// <summary>
        /// 课程编号
        /// </summary>
        [Required(ErrorMessage = "课程是必须的")]
        public int CourseId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [MinLength(5, ErrorMessage = "不少于5个字")]
        [MaxLength(255, ErrorMessage = "不能超过255个字")]
        [Required(ErrorMessage = "评论内容必须多于{0}个字")]
        public string Content { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [Range(0.5, 5, ErrorMessage = "评分范围在{0}~{1}")]
        public decimal Score { get; set; }
    }

    public class NodeInfo
    {
        /// <summary>
        /// 节点编码：S001
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 上一次该节点位置
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 节点播放地址
        /// </summary>
        public string Mp4 { get; set; }

        /// <summary>
        /// 节点文件大小
        /// </summary>
        public string Size { get; set; }
    }

    public class CourseInfo
    {
        public CourseInfo()
        {
            NodeList = new List<NodeInfo>();
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程标准
        /// </summary>
        public string RequiredFlag { get; set; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public string Credit { get; set; }

        /// <summary>
        /// 上线日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 完成课程时间
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// 最近访问时间
        /// </summary>
        public DateTime ActiveDate { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// 低分辨率视频下载地址
        /// </summary>
        public string DownloadUrlLow { get; set; }

        /// <summary>
        /// 在线播放地址
        /// </summary>
        public string OnlineUrl { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 上一次播放位置
        /// </summary>
        public decimal LastLocation { get; set; }

        /// <summary>
        /// 上一次播放节点
        /// </summary>
        public string LastNodeId { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<NodeInfo> NodeList { get; set; }

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 课程图片
        /// </summary>
        public string CourseImg { get; set; }

        /// <summary>
        /// 考试编号
        /// </summary>
        public string ExamId { get; set; }

        /// <summary>
        /// 评论学时
        /// </summary>
        public string CommentCredit { get; set; }

        /// <summary>
        /// 是否选课
        /// </summary>
        public string SelectFlag { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public string BrowseScore { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public string AvgScore { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// 课件大小
        /// </summary>
        public string CourseSize { get; set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        public string ClickCount { get; set; }

        /// <summary>
        /// 已经学习人数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 课程频道
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 剩余时间（分钟）
        /// </summary>
        public string Remainder { get; set; }
    }

    public class UserStudyInfo
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 当前进度
        /// </summary>
        public string CurrentProgress { get; set; }

        /// <summary>
        /// 上次位置
        /// </summary>
        public string LastLoation { get; set; }

        /// <summary>
        /// 最近节点
        /// </summary>
        public string LastNodeId { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public List<NodeInfo> NodeList { get; set; }
    }

    public class SyncUserStudyDataModel
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalCount { get; set; }

        /// <summary>
        /// 学习数据
        /// </summary>
        public List<UserStudyInfo> UserStudyInfoList { get; set; }
    }

    public class JYProcessDataModel
    {
        /// <summary>
        /// 节点编码：S001
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// 单节点进度时间(秒)
        /// </summary>
        public decimal Time { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        public string Status { get; set; }
    }

    public class SubmitCourseProcessDataModel
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        [Required(ErrorMessage = "数据丢失")]
        public int courseId { get; set; }

        /// <summary>
        /// 提交的进度数据
        /// </summary>
        public List<JYProcessDataModel> Data { get; set; }
    }

    public class Mp4CourseProcessDataModel
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// MP4时间位置 格式： 000534
        /// </summary>
        public string TimeNode { get; set; }
    }

    public class UserSelectCourseModel
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        [Required]
        public int CourseId { get; set; }

        /// <summary>
        /// add为添加课程，del为删除课程
        /// </summary>
        [Required]
        public string addAndDel { get; set; }
    }

    public class RankModelP
    {
        /// <summary>
        /// 排行分类 1：学时， 2：课程，3：单位
        /// </summary>
        public int RankType { get; set; }

        /// <summary>
        /// 个数
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// ExamModelP
    /// </summary>
    /// <seealso cref="JYOnline.AdminTwo.Model.PageRowsModel" />
    public class ExamModelP : PageRowsModel
    {
        /// <summary>
        /// 考试完成情况类型 All:全部； UnJoin:未考； UnFinish：未完成；Finish:已完成；
        /// </summary>
        /// <value>
        /// All
        /// </value>
        public string ExamType { get; set; }

        /// <summary>
        /// 考试频道
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }

    public class PostExamModel
    {
        /// <summary>
        /// 试题编号
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// 试题答案  A  ABCD 要按顺序 不能是BA
        /// </summary>
        public string Answer { get; set; }
    }

    public class PostUserExamModelP
    {
        /// <summary>
        /// 考试编号
        /// </summary>
        public int ExamId { get; set; }

        /// <summary>
        /// 旧题重做 默认不传
        /// </summary>

        public string PassExam { get; set; }

        /// <summary>
        /// 提交数据
        /// </summary>
        public List<PostExamModel> Data { get; set; }
    }

    public class DiscussModelP
    {
        /// <summary>
        /// 评论如果有主体则传评论主体Id（无则0），如果回复评论则传第一层评论Id.
        /// </summary>
        public int MainId { get; set; }

        /// <summary>
        /// 父级Id,如果是回复，则传被回复的Id（可以是第一层 或第二层 等等）,否则传0或不传；
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 可按照 CreateDate 或TopNum 排序，默认 CreateDate
    /// </summary>
    /// <seealso cref="JYOnline.AdminTwo.Model.PageSortModel" />
    public class DiscussListModelP : PageSortModel
    {
        public DiscussListModelP()
        {
            Sort = "CreateDate";
        }

        /// <summary>
        /// 如果要获取第一层评论则传评论主体Id（无则0），如果获取子层评论则传第一层评论Id.
        /// </summary>
        public int MainId { get; set; }
    }

    /// <summary>
    /// 可按照 CreateDate 或TopNum 排序，默认 CreateDate
    /// </summary>
    /// <seealso cref="JYOnline.AdminTwo.Model.PageSortModel" />
    public class MessageListModelP : PageSortModel
    {
        public MessageListModelP()
        {
            Sort = "CreateDate";
        }

        /// <summary>
        /// 如果要获取第一层评论则传评论主体Id（无则0），如果获取子层评论则传第一层评论Id.
        /// </summary>
        public int MainId { get; set; }

        public string ClassCode { get; set; }
    }

    public class CommentModelP
    {
        /// <summary>
        /// 评论对象 如果是回复，则填被回复那条评论的MainId
        /// </summary>
        public int MainId { get; set; }

        public int ParentId { get; set; }

        /// <summary>
        /// 评论标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论类型
        /// </summary>
        public string ClassCode { get; set; }
    }

    public class DelCourseCommentModel
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public int CourseId { get; set; }
    }

    public class ParentIdModel
    {
        /// <summary>
        /// 父级编号
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父级编码 (填了这个 就可以不用填ParentId了)
        /// </summary>
        public string ParentCode { get; set; }
    }

    public class IdModel
    {
        /// <summary>
        /// 对应编号
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "不能为空")]
        public int Id { get; set; }
    }

    public class IdsModel
    {
        /// <summary>
        /// 对应编号
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "不能为空")]
        public List<int> ids { get; set; }
    }

    public class DateModel
    {
        public DateTime Date { get; set; }
    }

    public class SendMsgModel
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string MobileNo { get; set; }
    }

    public class AccountModel
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
    }

    public class SetPasswordModel
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    public class MacModel
    {
        /// <summary>
        /// 物理地址
        /// </summary>
        public string Mac { get; set; }
    }

    public class SignType
    {
        /// <summary>
        /// 签到类型 add  total series top10
        /// </summary>
        public string Type { get; set; }
    }

    public class SeriesModelP
    {
        /// <summary>
        /// 专题数量
        /// </summary>
        public int SeriesCount
        {
            get;
            set;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string Img
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url
        {
            get;
            set;
        }
    }

    #region Training

    public class TrainingClassModelP : PageRowsModel
    {
        /// <summary>
        /// 培训班类型编号
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 培训班名称搜索
        /// </summary>
        public string TrainName { get; set; }

        /// <summary>
        /// 参加情况 Join：参加； UnJoin：未参加 ；UnAudit：审核中；
        /// </summary>
        public string JoinStatus { get; set; }
    }

    public class TrainingSignInTableModelP : PageRowsModel
    {
        public TrainingSignInTableModelP()
        {
            Order = "desc";
        }

        /// <summary>
        /// 培训班Id
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// true表示只获取今天的签到表，否则不填或false，则是整个列表。
        /// </summary>
        public bool TodayFlag { get; set; }

        /// <summary>
        /// 签到日期排序方向
        /// </summary>
        public string Order { get; set; }
    }

    public class TrainingSignModelP
    {
        /// <summary>
        /// 培训班编号
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// 签到类型 AMIn AMOut  PMIn  PMOut
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 当前经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 当前纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 签到位置名称
        /// </summary>
        public string Position { get; set; }
    }

    #endregion Training

    #region Book

    public class BookSearch : PageRowsModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 电子书分类
        /// </summary>
        public int BookTypeId { get; set; }

        /// <summary>
        /// 分类编码 如果传了该值 则BookTypeId无效。
        /// </summary>
        public string BookTypeCode { get; set; }
    }

    public class BookChapterSearch : PageRowsModel
    {
        /// <summary>
        /// 电子书编号
        /// </summary>
        public int BookId { get; set; }
    }

    #endregion Book

    #region Course

    public class RelatedCourseModelP : PageRowsModel
    {
        public int CourseId { get; set; }
    }

    #endregion Course

    // public class ExmaInfo

    #region Exam

    public class ExamAchievement
    {
        public bool PassFlag { get; set; }

        public decimal Score { get; set; }

        public int TotalCount { get; set; }

        public int RightCount { get; set; }

        public int DetailId { get; set; }
    }

    #endregion Exam

    #region 消息
    public class ApiMessage
    {
        public string Keyword { get; set; }

        public int Page { get; set; }

        public int Rows { get; set; }
    }
    #endregion 消息
}