using log4net;
using Quartz;
using QuartzNetCourse.AutoNotices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.QuartzJobs
{
    public sealed class EmailJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EmailJob));

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("EmailJob：正在工作......");
            EmailPost ep = new EmailPost();
            ep.sendEmail((int)AutoNotice.邮件消息);
        }
    }
}
