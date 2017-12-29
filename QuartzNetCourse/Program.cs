using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzNetCourse
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                //x.Service<TownCrier>(s =>
                //{
                //    s.ConstructUsing(settings => new TownCrier());
                //    s.WhenStarted(tc => tc.Start());
                //    s.WhenStopped(tc => tc.Stop());
                //});
                x.Service<ServiceRunner>();
                x.UseLog4Net();
                x.RunAsLocalSystem();


                x.SetDescription("此服务相关通知有系统数据备份、待批该作业、调查问卷、待批试卷。");
                x.SetDisplayName("QuartzNetCourse远程教育通知服务");
                x.SetServiceName("QuartzNetCourse远程教育通知服务");

                x.EnablePauseAndContinue();
            });
            //var properties = new System.Collections.Specialized.NameValueCollection();
            //properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //Quartz.IScheduler sched = Quartz.Impl.StdSchedulerFactory.GetDefaultScheduler();
            //System.Threading.Thread.Sleep(3000);
            //sched.PauseJob(new Quartz.JobKey("WorkJob", "Work"));
            //sched.PauseTrigger(new Quartz.TriggerKey("WorkJobTrigger", "Work"));
            //sched.UnscheduleJob(new Quartz.TriggerKey("WorkJob"));
        }
    }
}
