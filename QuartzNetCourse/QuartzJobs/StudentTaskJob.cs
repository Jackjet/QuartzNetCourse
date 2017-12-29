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
    public sealed class StudentTaskJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(StudentTaskJob));
        public void Execute(IJobExecutionContext context)
        {
            //学生做任务
            _logger.Info("StudentTaskJob：正在工作......");
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.学生任务);
        }
    }
}
