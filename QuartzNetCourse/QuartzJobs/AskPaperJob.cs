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
    public sealed class AskPaperJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AskPaperJob));
        public void Execute(IJobExecutionContext context)
        {
            //调查问卷
            _logger.Info("AskPaperJob：正在工作......");
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.调查问卷);
        }
    }
}
