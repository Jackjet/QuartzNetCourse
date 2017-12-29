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
    public sealed class ExamPaperJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExamPaperJob));
        public void Execute(IJobExecutionContext context)
        {
            //待批试卷
            _logger.Info("ExamPaperJob：正在工作......");
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.待批试卷);
        }

        
    }
}
