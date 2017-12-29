using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse
{
    public enum SysStatus
    {
        正常 = 0,
        删除 = 1,
        归档 = 2
    }

    public enum Hot
    {
        热点 = 1,
        普通 = 0
    }

    public enum NewsType
    {
        通知公告 = 0,
        学校新闻 = 1,
        媒体报道 = 2,
        招聘信息 = 3
    }

    public enum AdvertisType
    {
        联系我们 = 0,
        网站简介 = 1,
        友情链接 = 2
    }

    public enum PushTime
    {
        推送一次 = 0,
        每周推送一次 = 1
    }

    public enum AutoNotice
    {
        待批改作业 = 0,
        待批试卷 = 1,
        调查问卷 = 2,
        资源审核 = 3,
        学生报名 = 4,
        学生考试 = 5,
        学生任务 = 6,
        邮件消息 = 7,
        系统消息 = 8,
        发布作业 = 9,
        邮箱验证 = 10,
        找回密码=11
    }
    public enum MessageStatus
    {
        未读 = 0,
        已读 = 1
    }
    public enum isSend
    {
        未发送 = 0,
        已发送 = 1
    }

    public enum MessageTiming
    {
        立即发送 = 0,
        定时发送 = 1
    }
}
