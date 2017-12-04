using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Model
{
    public class UserInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 用户名 帐号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 平台id
        /// </summary>
        public string Portal { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PortalName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>
        public int GroupId { get; set; }


        /// <summary>
        /// 职级 id
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 职级名称
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string UserZW { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 学历id
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// 学历名称
        /// </summary>
        public string DegreeName { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        public string Party { get; set; }

        /// <summary>
        /// 当前总学时
        /// </summary>
        public string TotalCredit { get; set; }

        /// <summary>
        /// 学习次数
        /// </summary>
        public string Learncount { get; set; }

        /// <summary>
        /// 学分排名
        /// </summary>
        public string ScoreRank { get; set; }

        /// <summary>
        /// 规定总分
        /// </summary>
        public string NeedCredit { get; set; }

        /// <summary>
        /// 规定必修分
        /// </summary>
        public string NeedRequiredCredit { get; set; }

        /// <summary>
        /// 获得必修分
        /// </summary>
        public string RequiredCredit { get; set; }

        /// <summary>
        /// 获得选修分
        /// </summary>
        public string ElectiveCredit { get; set; }


        /// <summary>
        /// 获得考试学分
        /// </summary>
        public string ExamCredit { get; set; }

        /// <summary>
        /// 是否第一次登录
        /// </summary>
        public string IsFirstLogin { get; set; }

        /// <summary>
        /// 固网学分
        /// </summary>
        public string PcCredit { get; set; }

        /// <summary>
        /// 移动学分
        /// </summary>
        public string MobileCredit { get; set; }

        /// <summary>
        /// 总用户人数
        /// </summary>
        public int TotalUserCount { get; set; }

        /// <summary>
        /// 培训学分
        /// </summary>
        public string TrainningCredit { get; set; }

        /// <summary>
        /// 考核是否通过
        /// </summary>
        public bool PassFlag { get; set; }
    }
}
