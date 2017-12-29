﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuartzNetCourse.PostMail;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace QuartzNetCourse.AutoNotices
{
    public class EmailPost
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EmailPost));

        public void sendEmail(int type) 
        {
            using (SqlTransaction trans = SQLHelp.BeginTransaction()) 
            {
                try
                {
                    string sql = "select * from System_Message where isSend=" + (int)isSend.未发送 + " and IsDelete!=" + (int)SysStatus.删除 + " and [Type]=" + type + " and Timing=" + (int)MessageTiming.定时发送;
                    DataTable dt = SQLHelp.ExecuteDataTable(sql, CommandType.Text);
                    string ids = "";
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DateTime pushDate = Convert.ToDateTime(row["CreateTime"]);
                            DateTime curDate = DateTime.Now;
                            TimeSpan ts = pushDate - curDate;
                            if (ts.Minutes <= 2)
                            {
                                ids += row["Id"].ToString() + ",";
                            }
                            if (!string.IsNullOrWhiteSpace(ids) && ids.Length > 0)
                            {
                                ids = ids.Substring(0, ids.Length - 1);
                                string upSql = "update System_Message set isSend=" + (int)isSend.已发送 + " where Id in (" + ids + ")";
                                int number = SQLHelp.ExecuteNonQuery(upSql, CommandType.Text);
                                trans.Commit();
                            }
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            string Subject = string.IsNullOrWhiteSpace(Convert.ToString(row["Title"])) ? "无题" : row["Title"].ToString();
                            string Body = string.IsNullOrWhiteSpace(Convert.ToString(row["Contents"])) ? "无内容" : row["Contents"].ToString();
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(row["ReceiverEmail"])))
                            {
                                string href = "";
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(row["CreatorName"]))) Body += "<br/><h4>发件人：" + row["CreatorName"].ToString() + "</h4>";
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(row["Href"])))
                                {
                                    href = "<br/><h3><a href=" + row["Href"].ToString() + ">点击此处可查看详细信息</a></h3>";
                                }
                                SendMailMessage.SendMessage(Subject, Body + href, row["ReceiverEmail"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Info("EmailPost：" + ex.Message);
                    trans.Rollback();
                }
            }
        }
    }
}
