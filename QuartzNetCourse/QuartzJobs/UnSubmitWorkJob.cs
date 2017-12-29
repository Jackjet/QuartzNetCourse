using log4net;
using Quartz;
using QuartzNetCourse.AutoNotices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.QuartzJobs
{
    public sealed class UnSubmitWorkJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UnSubmitWorkJob));
        public void Execute(IJobExecutionContext context)  //hyd
        {
            _logger.Info("UnSubmitWorkJob正在工作...");
            //系统消息通知
            AutoPost ap = new AutoPost();
            ap.checkExam((int)AutoNotice.系统消息);
        }
    }
}
