using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCourse.EntityModel
{
    public class SystemDataSource
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(SystemDataSource));
        /// <summary>  
        /// 服务器  
        /// </summary>  
        private string server = System.Configuration.ConfigurationManager.AppSettings["services"];
        public string Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        /// <summary>  
        /// 登录名  
        /// </summary>  
        private string uid = System.Configuration.ConfigurationManager.AppSettings["uid"];
        public string UID
        {
            get { return this.uid; }
            set { this.uid = value; }
        }
        /// <summary>  
        /// 登录密码  
        /// </summary>  
        private string pwd = System.Configuration.ConfigurationManager.AppSettings["pwd"];
        public string PWD
        {
            get { return this.pwd; }
            set { this.pwd = value; }
        }
        /// <summary>  
        /// 要操作的数据库  
        /// </summary>  
        private string database = System.Configuration.ConfigurationManager.AppSettings["dataName"];
        public string Database
        {
            get { return this.database; }
            set { this.database = value; }
        }
        /// <summary>  
        /// 数据库连接字符串  
        /// </summary>  
        private string conn = System.Configuration.ConfigurationManager.AppSettings["connStr"];
        /// <summary>  
        /// 备份路经  
        /// </summary>  
        private string backPath = System.Configuration.ConfigurationManager.AppSettings["backUpDataPath"];
        public string BackPath
        {
            get { return this.backPath; }
            set { this.backPath = value; }
        }
        /// <summary>  
        /// 还原文件路经  
        /// </summary>  
        public string restoreFile;
        public string RestoreFile
        {
            get { return this.restoreFile; }
            set { this.restoreFile = value; }
        }
        //private ProgressBar bar;  

        //public ProgressBar Bar  
        //{  
        //    get { return bar; }  
        //    set { bar = value; }  
        //}  
        /// <summary>  
        /// DbBackUpAndRestore类的构造函数  
        /// </summary>  
        public SystemDataSource()
        {
        }
        /// <summary>  
        /// 切割字符串  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="bg"></param>  
        /// <param name="ed"></param>  
        /// <returns></returns>  
        public string StringCut(string str, string bg, string ed)
        {
            string sub;
            sub = str.Substring(str.IndexOf(bg) + bg.Length);
            sub = sub.Substring(0, sub.IndexOf(";"));
            return sub;
        }
        /// <summary>  
        /// 构造文件名  
        /// </summary>  
        /// <returns>文件名</returns>  
        private void CreatePath()
        {
            string CurrTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            CurrTime = CurrTime.Replace("-", "");
            CurrTime = CurrTime.Replace(":", "");
            CurrTime = CurrTime.Replace(" ", "");
            CurrTime = CurrTime.Substring(0, 12);
            backPath += "//_db_Schoo_Cube_" + CurrTime + ".BAK";
        }
        private void Step(string message, int percent)
        {
            //Bar.Value = percent;  
        }


        /// <summary>  
        /// 数据库备份  
        /// </summary>  
        /// <returns>备份是否成功</returns>  
        //public bool DbBackup()
        //{
        //    CreatePath();
        //    SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
        //    SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
        //    try
        //    {
        //        oSQLServer.LoginSecure = false;
        //        oSQLServer.Connect(server, uid, pwd);
        //        oBackup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
        //        //SQLDMO.BackupSink_PercentCompleteEventHandler pceh = new SQLDMO.BackupSink_PercentCompleteEventHandler(Step);  
        //        //oBackup.PercentComplete += pceh;  
        //        oBackup.Database = database;
        //        oBackup.Files = backPath;
        //        oBackup.BackupSetName = database;
        //        oBackup.BackupSetDescription = "数据库备份";
        //        oBackup.Initialize = true;
        //        oBackup.SQLBackup(oSQLServer);
        //        LogHelper.Info("当前时间" + DateTime.Now + "备份数据库操作成功！");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Info(ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        oSQLServer.DisConnect();
        //    }
        //}
        /// <summary>  
        /// 数据库恢复  
        /// </summary>  
        //public string DbRestore()
        //{
        //    if (exepro() != true)
        //    {
        //        return "操作失败";
        //    }
        //    else
        //    {
        //        SQLDMO.Restore oRestore = new SQLDMO.RestoreClass();
        //        SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
        //        try
        //        {
        //            exepro();
        //            oSQLServer.LoginSecure = false;
        //            oSQLServer.Connect(server, uid, pwd);
        //            oRestore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
        //            //SQLDMO.RestoreSink_PercentCompleteEventHandler pceh = new SQLDMO.RestoreSink_PercentCompleteEventHandler(Step);  
        //            //oRestore.PercentComplete += pceh;  
        //            oRestore.Database = database;
        //            ///自行修改  
        //            oRestore.Files = restoreFile;
        //            oRestore.FileNumber = 1;
        //            oRestore.ReplaceDatabase = true;
        //            oRestore.SQLRestore(oSQLServer);
        //            LogHelper.Info("当前时间" + DateTime.Now + "恢复数据库操作成功！");
        //            return "数据库恢复成功";
        //        }
        //        catch (Exception e)
        //        {
        //            LogHelper.Info(e.Message);
        //            return "恢复数据库失败，原因:" + e.Message;

        //        }
        //        finally
        //        {
        //            oSQLServer.DisConnect();
        //        }
        //    }
        //}
        /// <summary>  
        /// 杀死当前库的所有进程  
        /// </summary>  
        /// <returns></returns>  
        //private bool exepro()
        //{
        //    bool success = true;
        //    SQLDMO.SQLServer svr = new SQLDMO.SQLServerClass();
        //    try
        //    {
        //        svr.Connect(server, uid, pwd);
        //        //取得所有的进程列表  
        //        SQLDMO.QueryResults qr = svr.EnumProcesses(-1);
        //        int iColPIDNum = -1;
        //        int iColDbName = -1;
        //        //找到和要恢复数据库相关的进程  
        //        for (int i = 1; i <= qr.Columns; i++)
        //        {
        //            string strName = qr.get_ColumnName(i);
        //            if (strName.ToUpper().Trim() == "SPID")
        //            {
        //                iColPIDNum = i;
        //            }
        //            else if (strName.ToUpper().Trim() == "DBNAME")
        //            {
        //                iColDbName = i;
        //            }
        //            if (iColPIDNum != -1 && iColDbName != -1)
        //                break;
        //        }
        //        //将相关进程关闭     
        //        for (int i = 1; i <= qr.Rows; i++)
        //        {
        //            int lPID = qr.GetColumnLong(i, iColPIDNum);
        //            string strDBName = qr.GetColumnString(i, iColDbName);
        //            if (strDBName.ToUpper() == database)
        //                svr.KillProcess(lPID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Info(ex.Message);
        //        success = false;
        //    }
        //    return success;
        //}
        public bool Operate(bool isBackup)
        {
            //备份：use master;backup database @name to disk=@path;  
            //恢复：use master;restore database @name from disk=@path;   
            SqlConnection connection = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + uid + ";password=" + pwd + ";");
            try
            {
                if (restoreFile != null && !restoreFile.ToLower().EndsWith(".bak"))
                {
                    restoreFile += ".bak";
                }
                if (isBackup)//备份数据库   
                {
                    SqlParameter[] param = new SqlParameter[]{
                   new SqlParameter("@name", Database),
                   new SqlParameter("@path", restoreFile)
                };
                    int number = SQLHelp.ExecuteNonQuery("use master;backup database @name to disk=@path;", CommandType.Text, param);
                    //SqlCommand command = new SqlCommand("use master;backup database @name to disk=@path;", connection);
                    //command.Parameters.AddWithValue("@name", Database);
                    //command.Parameters.AddWithValue("@path", restoreFile);
                    //connection.Open();
                    //command.ExecuteNonQuery();
                    //connection.Close();
                }
                else//恢复数据库   
                {
                    SqlCommand command = new SqlCommand("use master;restore database @name from disk=@path;", connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@name", Database);
                    command.Parameters.AddWithValue("@path", restoreFile);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("数据库备份失败！", ex);
                return false;
            }

        }
    }
}
