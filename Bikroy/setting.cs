using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Bikroy
{
   public  class setting
    {       
		public MySqlConnection conn = new MySqlConnection();
		public MySqlCommand cmd = new MySqlCommand();
		MySqlDataReader dr;
		MySqlDataAdapter da;

		DataSet ds;
       DataTable dt;
		/// <summary>
		/// This function will get the connnection string from web.config
		/// </summary>
		/// <returns>connection string</returns>
		/// <remarks></remarks>
		public string getConnectionString()
		{
			string objConnectionString = "";
			objConnectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();

			return objConnectionString;
		}



		/// <summary>
		/// This function will create a connection to database
		/// </summary>
		/// <remarks></remarks>
		public void createConnection()
		{
			string connstr = getConnectionString();
			conn = new MySqlConnection(connstr);
			try {
				if (conn.State == ConnectionState.Open) {
					conn.Close();
				}
				conn.Open();

			} catch (Exception ex) {
			}
		}




		/// <summary>
		/// This function will help to insert values to database.
		/// </summary>
		/// <param name="sqlStr">This is a sql string for insert to database</param>
		/// <returns>A integer will return.</returns>
		/// <remarks>If the function returns a non zero value then the values submitted succesfully.</remarks>
		public int InsertOrUpdateOrDeleteValueToDatabase(string sqlStr)
		{
			int Status = 0;

			try {
				createConnection();
				cmd = new MySqlCommand(sqlStr, conn);
				cmd.CommandTimeout = 0;
				Status = cmd.ExecuteNonQuery();

				return Status;

			} catch (Exception ex) {
				return Status;
			}
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>gd</returns>
		/// <remarks>sdfsdfsdfsd sdsdfsdf sdfsdfsd sdfsdf</remarks>
		public DataTable selectAllfromDatabaseAndReturnDataTable(string sql)
		{
			try {
				createConnection();
				cmd = new MySqlCommand(sql, conn);
				da = new MySqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);

			} catch (Exception ex) {
				dt = null;
			}

			return dt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql">sql select command</param>
		/// <returns>Mysqldatareader</returns>
		/// <remarks></remarks>
		public MySqlDataReader selectFromDataBaseAndreturnDatareader(string sql)
		{
			try {
				createConnection();
				cmd = new MySqlCommand(sql, conn);
				dr = cmd.ExecuteReader();

			} catch (Exception ex) {
				dr = null;
			}

			return dr;
		}
		/// <summary>
		/// insert apostropy in database
		/// </summary>
		/// <param name="text">string to be replaced</param>
		/// <returns>string</returns>
		/// <remarks></remarks>
		public string apostropy(string text)
		{
			if (string.IsNullOrEmpty(text)) {
				return "";
			} else {
				text = text.Replace("'", "''");
				return text;
			}

		}
        

    }
}
