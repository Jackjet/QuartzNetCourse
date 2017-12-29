using log4net;
using Quartz;
using QuartzNetCourse.AutoNotices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.QuartzJobs
{
    public sealed class WorkJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(WorkJob));
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("WorkJob正在工作...");
            //待批该作业
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.待批改作业);
        }

        //public void SendMessage(string toUser, string [] toCCUser, string toMCUser, string Subject, string Body)
        //{
            
        //    watch.Reset();
        //    watch.Start();
        //    long count = toCCUser.Length;//有多少个收件人
        //    bool isAsync = true;
        //    MailHelper mail = new MailHelper(isAsync);
        //    for (long i = 0; i < count; i++)
        //    {
        //        this.toUser = toCCUser[i];
        //        this.SendMessageAsync(mail, false, "测试", "第" + (i + 1) + "条", true, true);
        //    }
        //    mail.SetBatchMailCount(count);
        //}

        //private void SendMessageAsync(MailHelper mail, bool isSimple, string shiyan, string msgCount, bool autoReleaseSmtp, bool isReuse)
        //{
        //    AppendReplyMsg(String.Format("{0}：{1}\"异步\"邮件开始。{2}{3}", shiyan, msgCount, watch.ElapsedMilliseconds, Environment.NewLine));

        //    if (!isReuse || !mail.ExistsSmtpClient())
        //    {
        //        SmtpClient client = new SmtpHelper(ConfigEmail.TestEmailType, false, ConfigEmail.TestUserName, ConfigEmail.TestPassword).SmtpClient;
        //        mail.AsycUserState = String.Format("{0}：{1}\"异步\"邮件", shiyan, msgCount);
        //        client.SendCompleted += (send, args) =>
        //        {
        //            AsyncCompletedEventArgs arg = args;

        //            if (arg.Error == null)
        //            {
        //                // 需要注意的事使用 MailHelper 发送异步邮件，其UserState是 MailUserState 类型
        //                AppendReplyMsg(((MailUserState)args.UserState).UserState.ToString() + "已发送完成." + watch.ElapsedMilliseconds + Environment.NewLine);
        //            }
        //            else
        //            {
        //                AppendReplyMsg(String.Format("{0} 异常：{1}{2}{3}"
        //                    , ((MailUserState)args.UserState).UserState.ToString() + "发送失败."
        //                    , (arg.Error.InnerException == null ? arg.Error.Message : arg.Error.Message + arg.Error.InnerException.Message)
        //                    , watch.ElapsedMilliseconds, Environment.NewLine));
        //                // 标识异常已处理，否则若有异常，会抛出异常
        //                ((MailUserState)args.UserState).IsErrorHandle = true;
        //            }
        //        };
        //        mail.SetSmtpClient(client, autoReleaseSmtp);
        //    }
        //    else
        //    {
        //        mail.AsycUserState = String.Format("{0}：{1}\"异步\"邮件已发送完成。", shiyan, msgCount);
        //    }

        //    mail.From = ConfigEmail.TestFromAddress;
        //    mail.FromDisplayName = ConfigEmail.GetAddressName(ConfigEmail.TestFromAddress);

        //    string to = toUser;
        //    string cc = toCCUser;
        //    string bcc = toMCUser;
        //    if (to.Length > 0)
        //        mail.AddReceive(EmailAddrType.To, to, ConfigEmail.GetAddressName(to));
        //    if (cc.Length > 0)
        //        mail.AddReceive(EmailAddrType.CC, cc, ConfigEmail.GetAddressName(cc));
        //    if (bcc.Length > 0)
        //        mail.AddReceive(EmailAddrType.Bcc, bcc, ConfigEmail.GetAddressName(bcc));

        //    mail.Subject = Subject;
        //    // Guid.NewGuid() 防止重复内容，被SMTP服务器拒绝接收邮件
        //    mail.Body = Body + Guid.NewGuid();
        //    mail.IsBodyHtml = true;

        //    if (filePaths != null && filePaths.Count > 0)
        //    {
        //        foreach (string filePath in FilePaths)
        //        {
        //            mail.AddAttachment(filePath);
        //        }
        //    }

        //    Dictionary<MailInfoType, string> dic = mail.CheckSendMail();
        //    if (dic.Count > 0 && MailInfoHelper.ExistsError(dic))
        //    {
        //        // 反馈“错误+提示”信息
        //        AppendReplyMsg(MailInfoHelper.GetMailInfoStr(dic));
        //    }
        //    else
        //    {
        //        string msg = String.Empty;
        //        if (dic.Count > 0)
        //        {
        //            // 反馈“提示”信息
        //            msg = MailInfoHelper.GetMailInfoStr(dic);
        //        }

        //        try
        //        {
        //            // 发送
        //            if (isSimple)
        //            {
        //                mail.SendOneMail();
        //            }
        //            else
        //            {
        //                mail.SendBatchMail();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // 反馈异常信息
        //            AppendReplyMsg(String.Format("{0}\"异步\"异常：（{1}）{2}{3}", msgCount, watch.ElapsedMilliseconds, ex.Message, Environment.NewLine));

        //        }
        //        finally
        //        {
        //            // 输出到界面
        //            if (msg.Length > 0)
        //                AppendReplyMsg(msg + Environment.NewLine);
        //        }
        //    }

        //    mail.Reset();
        //}

        //private void AppendReplyMsg(string msg)
        //{
        //    _logger.Info(msg);
        //}
    }
}
