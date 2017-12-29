using log4net;
using Quartz;
using QuartzNetCourse.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.QuartzJobs
{
    public sealed class TimingBackUpJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExamPaperJob));
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("TimingBackUpJob正在工作...");
            SystemDataSource sdbb = new SystemDataSource();
            string CurrTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            CurrTime = CurrTime.Replace("-", "");
            CurrTime = CurrTime.Replace(":", "");
            CurrTime = CurrTime.Replace(" ", "");
            CurrTime = CurrTime.Substring(0, 12);
            sdbb.restoreFile = "C:\\_db_Schoo_Cube_" + CurrTime + ".BAK";
            bool result = sdbb.Operate(true);
            if (result)
            {
                _logger.Info("服务时间：" + DateTime.Now + ",备份数据库成功！");
            }
            else
            {
                _logger.Info("服务时间：" + DateTime.Now + ",备份数据库失败！");
            }
            //SystemDataSource sds = new SystemDataSource();
            //sds.Server = ".";
            //sds.UID = "sa";
            //sds.PWD = "sa123??";
            //sds.Database = "dnt2";
            
            //string path = System.Environment.CurrentDirectory;
            //if (!string.IsNullOrWhiteSpace(path))
            //{
            //    path = path.Substring(0, path.LastIndexOf("\\"));
            //    path = path.Substring(0, path.LastIndexOf("bin")) + System.Configuration.ConfigurationManager.AppSettings["FilePath"];
            //}
            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);//不存在就创建目录 
            //}
            //sds.BackPath = path.Replace("\\","//");
            //sds.DbBackup();
        }
    }
}
