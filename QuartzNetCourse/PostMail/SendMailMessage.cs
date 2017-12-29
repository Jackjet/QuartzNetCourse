using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.PostMail
{
    public class SendMailMessage
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubject">邮件标题</param>
        /// <param name="mailContent">邮件内容</param>
        /// <param name="mailAddress">邮件地址</param>
        /// <returns></returns>
        public static MailResult SendMessage(string mailSubject, string mailContent, string mailAddress)
        {
            string sendUser = ConfigurationManager.AppSettings["SendPersonalMailUser"];
            string sendPwd = ConfigurationManager.AppSettings["SendPersonalMailPwd"];
            MailResult result = new MailResult();
            result.Account = sendUser;
            result.Password = sendPwd;
            result.Title = mailSubject;
            result.Content = mailContent;
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("/web.config");

                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                if (mailAddress != null && mailSubject != null && mailContent != null)
                {
                    MailMessage mailMessage = new MailMessage();
                    //设置发件人的邮件地址
                    MailAddress senderMailAddress = new MailAddress(sendUser);
                    //添加发件人的邮件地址
                    mailMessage.From = senderMailAddress;

                    //添加收件人的邮件地址
                    foreach (var mail in mailAddress.Split(';'))
                    {
                        MailAddress receiveMailAddress = new MailAddress(mail);
                        mailMessage.To.Add(receiveMailAddress);
                    }

                    //设置邮件的主题
                    mailMessage.Subject = mailSubject;
                    mailMessage.SubjectEncoding = Encoding.UTF8;

                    //设置邮件的正文以及模板
                    //mailMessage.Body ="邮件测试"+ mailContent + ReadMailTemplate();
                    mailMessage.Body = mailContent;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;

                    //设置邮件的优先级别
                    mailMessage.Priority = MailPriority.High;

                    //发送电子邮件
                    SmtpClient smtpClient = new SmtpClient();
                    //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
                    smtpClient.Host = settings.Smtp.Network.Host;
                    //设置用于 SMTP 事务的端口，默认的是 25
                    smtpClient.Port = settings.Smtp.Network.Port;
                    //设置登录邮箱的用户名和密码
                    smtpClient.Credentials = new System.Net.NetworkCredential(sendUser, sendPwd);
                    //指定如何处理待发的电子邮件,电子邮件通过网络发送到 SMTP 服务器
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //是否使用安全套接字层 (SSL) 加密连接
                    smtpClient.EnableSsl = settings.Smtp.Network.EnableSsl;
                    //发送电子邮件
                    try
                    {
                        smtpClient.Send(mailMessage);
                        result.Isok = true;
                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {
                        result.Isok = false;
                        result.ErrorMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }
    }
    /// <summary>
    /// 发送邮箱结果
    /// </summary>
    public class MailResult
    {
        /// <summary>
        /// 结果
        /// </summary>
        public bool Isok { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
