using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.QuartzJobs
{
    public sealed class CourseJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CourseJob));
        public void Execute(IJobExecutionContext context)
        {
            //上课提醒
        }
    }
}
