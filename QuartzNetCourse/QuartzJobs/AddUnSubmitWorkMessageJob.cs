using log4net;
using Quartz;
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
    public sealed class AddUnSubmitWorkMessageJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AddUnSubmitWorkMessageJob));
        public void Execute(IJobExecutionContext context)  //hyd
        {
            _logger.Info("AddUnSubmitWorkMessageJob正在工作...");
            //添加未提交作业通知
            AddUnSubmitWorkMessage();
        }

        #region 添加“学员在作业提交时间之内没有提交作业”的通知
        private void AddUnSubmitWorkMessage()
        {
            try
            {
                string sql = @"select work.Id,work.CouseID,work.Name,co.CourceType from Course_Work work
                       left join Course co on work.CouseID=co.ID
                       where work.IsDelete=0 and convert(varchar(10),dateadd(d,1,work.EndTime),21)= convert(varchar(10),getdate(),21) ";
                DataTable workDt = SQLHelp.ExecuteDataTable(sql, CommandType.Text, null);//获取可能需要发通知的作业
                if (workDt != null && workDt.Rows.Count > 0)
                {
                    StringBuilder sbSql4org = new StringBuilder();
                    sbSql4org.Append(@"INSERT INTO System_Message (Title,Contents,[Type],IsDelete,CreateTime,Creator,Receiver,Href,Status,isSend,ReceiverEmail,CreatorName,ReceiverName,Timing,FilePath)
                                              values(@Title,@Contents,@Type,@IsDelete,@CreateTime,@Creator,@Receiver,@Href,@Status,@isSend,@ReceiverEmail,@CreatorName,@ReceiverName,@Timing,@FilePath)");
                    foreach (DataRow row in workDt.Rows)
                    {
                        DataTable studyDt = StudyTheCourseStu(row["CouseID"].ToString(), row["CourceType"].ToString());//获取学习这门课的学生
                        List<DataRow> rtnDt = (from dic in studyDt.AsEnumerable() select dic).ToList();
                        #region 根据作业id获取提交作业的学生
                        string corrsql = @"select CreateUID from Course_WorkCorrectRel where WorkId=@WorkId ";
                        List<SqlParameter> pms = new List<SqlParameter>();
                        pms.Add(new SqlParameter("@WorkId", row["Id"].ToString()));
                        DataTable corrDt = SQLHelp.ExecuteDataTable(corrsql, CommandType.Text, pms.ToArray());
                        #endregion
                        if (corrDt.Rows.Count > 0)
                        {
                            string[] corrArray = corrDt.AsEnumerable().Select(corrRow => corrRow["CreateUID"].ToString()).ToArray();
                            rtnDt = (from dic in studyDt.AsEnumerable()
                                     where corrArray.Contains(dic["IDCard"].ToString())==false
                                     select dic).ToList();
                        }
                        if (rtnDt.Count > 0)  //未提交作业的学生
                        {
                            string type = "8", title = "未提交作业";
                            string href = "/OnlineLearning/StuLessonDetail.aspx?itemid=" + row["CouseID"].ToString() + "&nav_index=4&relname=&flag=1&tabconid=" + row["Id"].ToString();
                            string contents = "您未在规定时间内提交作业——" + row["Name"].ToString() + "<br/><h3><a href=" + href + ">点击此处可查看详细信息</a></h3>";
                            for (int i = 0; i < rtnDt.Count; i++)
                            {                                
                                List<SqlParameter> spList = new List<SqlParameter>();
                                spList.Add(new SqlParameter("@Title", title));
                                spList.Add(new SqlParameter("@Contents", contents));
                                spList.Add(new SqlParameter("@Type", type));
                                spList.Add(new SqlParameter("@Receiver", rtnDt[i]["IDCard"].ToString()));
                                spList.Add(new SqlParameter("@Href", href));
                                #region 判断通知是否已存在
                                string isexistSql = @"select count(1) from System_Message where Title=@Title and Contents=@Contents 
                                                   and [Type]=@Type and Receiver=@Receiver and Href=@Href ";
                                int est_count =Convert.ToInt32(SQLHelp.ExecuteScalar(isexistSql, CommandType.Text, spList.ToArray()));
                                #endregion
                                if (est_count == 0) //不存在
                                {
                                    spList.Add(new SqlParameter("@IsDelete", ((int)SysStatus.正常).ToString()));
                                    spList.Add(new SqlParameter("@CreateTime", DateTime.Now));
                                    spList.Add(new SqlParameter("@Creator", ""));
                                    spList.Add(new SqlParameter("@Status", ((int)MessageStatus.未读).ToString()));
                                    spList.Add(new SqlParameter("@isSend", ((int)isSend.未发送).ToString()));
                                    spList.Add(new SqlParameter("@ReceiverEmail", ""));
                                    spList.Add(new SqlParameter("@CreatorName", ""));
                                    spList.Add(new SqlParameter("@ReceiverName", rtnDt[i]["Name"].ToString()));
                                    spList.Add(new SqlParameter("@Timing", "0"));
                                    spList.Add(new SqlParameter("@FilePath", ""));
                                    int number = SQLHelp.ExecuteNonQuery(sbSql4org.ToString(), CommandType.Text, spList.ToArray());
                                }  
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Info("AddUnSubmitWorkMessageJob：" + ex.Message);
            }
        }
        #endregion

        #region 正在学习该课程的同学
        private DataTable StudyTheCourseStu(string courseID, string courseType)
        {
            DataTable rtnDt = new DataTable();
            List<SqlParameter> pms = new List<SqlParameter>();
            pms.Add(new SqlParameter("@CourseID", courseID));
            string sql = courseType == "1" ? "select ClassID as IDS from ClassCourse where CourseID=@CourseID" : "select StuNo as IDS from Couse_Selstuinfo where Status=1 and CourseID=@CourseID";
            DataTable idsDt = SQLHelp.ExecuteDataTable(sql, CommandType.Text, pms.ToArray());

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            if (idsDt != null && idsDt.Rows.Count > 0)
            {
                string ids = string.Join(",", idsDt.AsEnumerable().Select(row => row["IDS"].ToString()).ToArray());
                if (!string.IsNullOrEmpty(ids))
                {
                    rtnDt = GetStudentData(ids, courseType);
                }
            }
            return rtnDt;
        }
        #endregion

        #region 获得学生数据
        public DataTable GetStudentData(string ids, string courseType)
        {
            try
            {
                string sql = @"select a.* from Plat_Student a where a.IsDelete=0 ";
                if (courseType == "1")
                {
                    sql += " and a.ClassID in (" + ids + ")";
                }
                else
                {
                    string[] sdf = ids.Split(',');
                    string IDCards = "";
                    foreach (string item in sdf)
                    {
                        IDCards += ",'" + item + "'";
                    }
                    IDCards = IDCards.Substring(1);
                    sql += " and a.IDCard in (" + IDCards + ")";
                }
                return ExecuteDataTable_BasePlat(sql, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
        #endregion

        #region 返回DataTable对象的方法
        /// <summary>
        /// 返回DataTable对象的方法
        /// </summary>
        /// <param name="sql">要执行的Sql语句</param>
        /// <param name="cmdType">要执行的命令类型</param>
        /// <param name="pms">传入的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable_BasePlat(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            try
            {
                DataSet dbs = new DataSet();
                using (SqlConnection SqlConn = new SqlConnection(ConfigurationManager.AppSettings["connStr_BasePlat"]))
                {
                    using (SqlCommand command = new SqlCommand(sql, SqlConn))
                    {
                        command.CommandType = CommandType.Text;
                        if (pms != null)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddRange(pms);
                        }
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = command;
                        sda.Fill(dbs, "data");
                    }
                }
                return dbs.Tables[0];
            }
            catch (Exception ex)
            {

                return new DataTable();
            }
        }
        #endregion
    }
}
