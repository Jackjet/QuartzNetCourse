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
    public sealed class ResourceExamineJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ResourceExamineJob));
        public void Execute(IJobExecutionContext context)
        {
            //资源审核
            _logger.Info("ResourceExamineJob：正在工作......");
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.资源审核);
        }
    }
}
