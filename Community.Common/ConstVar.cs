using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Common
{
    /// <summary>
    /// 公共枚举
    /// </summary>

    /// <summary>
    /// 错误等级
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>1：情报</summary>
        INFO = 1,

        /// <summary>2：调试</summary>
        DEBUG = 2,

        /// <summary>3：警告</summary>
        WARN = 3,

        /// <summary>4：错误</summary>
        ERROR = 4,

        /// <summary>5：致命</summary>
        FATAL = 5
    }
    /// <summary>
    /// 上传类型
    /// </summary>
    public enum UploadType
    {
        /// <summary>1：新闻图片</summary>
        NewsImage = 1,

        /// <summary>2：公司文档</summary>
        CompanyDocument = 2,

        /// <summary>3：计划附件</summary>
        PlanAttachment = 3,

        /// <summary>4：个人文档</summary>
        UserDocument = 4,

        /// <summary>5：用户头像</summary>
        HeadImage = 5,

        /// <summary>6：IM文件</summary>
        ImFile = 6,

        /// <summary>7：目标文件</summary>
        ObjectiveFile = 7,

        /// <summary>8:流程首页文档</summary>
        FlowIndexFile = 8,

        /// <summary>9:会议附件</summary>
        MeetingFile = 9
    }

    /// <summary>
    /// 在职状态
    /// </summary>
    public enum workStatus
    {
        /// <summary>0：离职</summary>
        Quit = 0,

        /// <summary>1：在职</summary>
        OnWork = 1,

        /// <summary>2：退休</summary>
        Retired = 2
    }

    /// <summary>
    /// 表单状态
    /// </summary>
    public enum userFormStatus
    {
        /// <summary>1、待提交</summary>
        unSubmit = 1,

        /// <summary>2、流程中</summary>
        flowing = 2,

        /// <summary>50、已办结</summary>
        hasCompleted = 50
    }

    /// <summary>
    /// 控件权限
    /// </summary>
    public enum nodeControlStatus
    {
        /// <summary>0、隐藏 </summary>
        hide = 0,

        /// <summary>1、只读</summary>
        readOnly = 1,

        /// <summary>2、编辑</summary>
        edit = 2
    }



    public enum ReturnField
    {
        /// <summary>0:成功</summary>
        success = 0,

        /// <summary>-1:失败</summary>
        error = -1
    }
}
