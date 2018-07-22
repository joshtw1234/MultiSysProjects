using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilityUILib
{
   
    public class Db_API
    {
        string dbPath = string.Empty;
        public string DBPath
        {
            get { return dbPath; }
        }

        SQLiteConnection sqlCnn;
        public Db_API(string _dbPath)
        {
            sqlCnn = CreateSqlConnection(_dbPath);
            dbPath = _dbPath;
        }
#if true
        #region Sql Process Insert, delete, update etc.
        public bool CreatTableByName(string tbName)
        {
            bool rev = false;
            string tbStr = string.Format("CREATE TABLE IF NOT EXISTS {0} (PcdName varchar,typeID int,typeName varchar,Strings varchar);", tbName);
            List<string> lstComm = new List<string>();
            lstComm.Add(tbStr);
            rev = RunSqlCommand(lstComm);
            return rev;
        }

        public bool InsertToTable(string tbNmae, string insIDs, string insVals)
        {
            bool rev = false;
            //INSERT INTO OMENTest (PcdName,typeID,typeName,Strings) VALUES ('ccc',2,'TTTN','TTTS');
            string insStr = string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tbNmae, "PcdName,typeID,typeName,Strings", "'TTT',5,'TTTN','TTTS'");
            List<string> lstComm = new List<string>();
            lstComm.Add(insStr);
            rev = RunSqlCommand(lstComm);
            return rev;
        }
        /// <summary>
        /// Get the all table name in the DB connection
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        List<string> getTableList(SQLiteConnection conn)
        {
            List<string> list_rtn = new List<string>();

            string str_command = @"SELECT name FROM sqlite_master WHERE type='table'ORDER BY name;";
            SQLiteCommand command = new SQLiteCommand(str_command, conn);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list_rtn.Add(reader[0].ToString().ToLower());
            }
            return list_rtn;
        }
        #endregion

        #region Sql Opreator
        /// <summary>
        /// Create a connection and return it
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        SQLiteConnection CreateSqlConnection(string dbPath)
        {
            SQLiteConnection connection;
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);

            }
            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbPath));
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Close the Connection
        /// </summary>
        /// <param name="SqlConn"></param>
        void CloseSqlConnection(SQLiteConnection SqlConn)
        {
            SqlConn.Close();
            SqlConn.Dispose();
        }

        /// <summary>
        /// Run the list command for input connection 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="list"></param>
        bool RunSqlCommand(List<string> list)
        {
            bool rev = false;
            using (SQLiteTransaction trans = sqlCnn.BeginTransaction())
            {
                SQLiteCommand command = new SQLiteCommand(sqlCnn);
                foreach (string commandText in list)
                {
                    try
                    {
                        if (commandText.Trim().Length == 0)
                        {
                            continue;
                        }
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return rev;
                    }
                }
                trans.Commit();
            }
            rev = true;
            return rev;
        }
        #endregion
#else
        public enum DatabaseName
    {
        ProjectDB = 0,
        AppConfigDB,
        TagsDB,
        PackageXml
    }
          string databasePath;
        string tagDatabasePath;
        string AppConfigdbPath;
        string packageTagXmlDirPath;

        #region Public Area
        public List<string> GetDbTable(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return null;
            }
            return getTableList(CreateSqlConnection(dbPath));
        }
        #endregion

        #region Sql Opreator
        /// <summary>
        /// Create a connection and return it
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        SQLiteConnection CreateSqlConnection(string dbPath)
        {
            SQLiteConnection connection;
            if (!File.Exists(dbPath))
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(dbPath);
               
            }
            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbPath));
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Close the Connection
        /// </summary>
        /// <param name="SqlConn"></param>
        void CloseSqlConnection(SQLiteConnection SqlConn)
        {
            SqlConn.Close();
            SqlConn.Dispose();
        }
        /// <summary>
        /// Run the list command for input connection 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="list"></param>
        void RunSqlCommand(System.Data.SQLite.SQLiteConnection connection, List<string> list)
        {
            int i = 0;
            using (System.Data.SQLite.SQLiteTransaction trans = connection.BeginTransaction())
            {
                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection);
                foreach (string commandText in list)
                {
                    try
                    {
                        if (commandText.Trim().Length == 0)
                        {
                            continue;
                        }
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                        i++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                trans.Commit();
            }

            list.Clear();
        }

        #endregion

        #region Sql Process Insert, delete, update etc.
        /// <summary>
        /// Get the all table name in the DB connection
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        List<string> getTableList(SQLiteConnection conn)
        {
            List<string> list_rtn = new List<string>();

            string str_command = @"SELECT name FROM sqlite_master WHERE type='table'ORDER BY name;";
            SQLiteCommand command = new SQLiteCommand(str_command, conn);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list_rtn.Add(reader[0].ToString().ToLower());
            }
            return list_rtn;
        }
        /// <summary>
        /// Get mapping database file path by emuration type
        /// </summary>
        /// <param name="eDatabaseName"></param>
        /// <returns>mapping database file path</returns>
        string GetDatabasePath(DatabaseName eDatabaseName)
        {
            switch (eDatabaseName)
            {
                case DatabaseName.ProjectDB:
                    return databasePath;
                case DatabaseName.AppConfigDB:
                    return AppConfigdbPath;
                case DatabaseName.TagsDB:
                    return tagDatabasePath;
                default:
                    break;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the all Filed name in the DB connection
        /// </summary>
        /// <param name="conn"></param>
        /// <returns>Filed names in Table</returns>
        List<string> getFieldList(SQLiteConnection conn, string tableName)
        {
            List<string> list_rtn = new List<string>();

            string str_command = @"PRAGMA table_info([" + tableName + "])";
            SQLiteCommand command = new SQLiteCommand(str_command, conn);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list_rtn.Add(reader[1].ToString());
            }
            return list_rtn;
        }

        /// <summary>
        /// Get row datas by indicating table
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strSqlWhere"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> SelectTable(DatabaseName eDatabaseName, string strTableName, string strSqlWhere)
        {
            string databaseFilePath = GetDatabasePath(eDatabaseName);
            SQLiteCommand Sql_command;
            SQLiteDataReader Sql_reader;
            string str_SelectCmd = string.Empty;
            string setLimt = string.Empty;
            SQLiteConnection SqlConn = CreateSqlConnection(databaseFilePath);
            Dictionary<string, object> dicFieldValues = null;
            List<Dictionary<string, object>> lstSelectResults = new List<Dictionary<string, object>>();

            List<string> lstTable = getTableList(SqlConn);
            if (lstTable.Contains(strTableName.ToLower()))
            {
                str_SelectCmd = string.Format("SELECT * FROM [{0}]", strTableName);
                if (!string.IsNullOrEmpty(strSqlWhere))
                    str_SelectCmd += string.Format(" WHERE {0}", strSqlWhere);

                Sql_command = new SQLiteCommand(str_SelectCmd, SqlConn);
                Sql_reader = Sql_command.ExecuteReader();

                List<object> lstObjects = new List<object>();
                List<string> lstFieldNames = getFieldList(SqlConn, strTableName);

                while (Sql_reader.Read())
                {
                    dicFieldValues = new Dictionary<string, object>();
                    foreach (string field in lstFieldNames)
                        dicFieldValues.Add(field, Sql_reader[field]);

                    lstSelectResults.Add(dicFieldValues);
                }
            }
            //return list_rtn;
            return lstSelectResults;
        }

        bool InsertRecordsBySqlStrings(DatabaseName eDatabaseName, List<string> lstCmds)
        {
            string databaseFilePath = GetDatabasePath(eDatabaseName);
            try
            {
                SQLiteConnection Sqlconn = CreateSqlConnection(databaseFilePath);
                RunSqlCommand(Sqlconn, lstCmds);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        bool DeleteRecordsInTable(DatabaseName eDatabaseName, string tableName, string sqlWhere)
        {
            string databaseFilePath = GetDatabasePath(eDatabaseName);
            List<string> lstCmd = new List<string>();
            lstCmd.Add(string.Format("DELETE FROM {0} WHERE {1}", tableName, sqlWhere));
            try
            {
                SQLiteConnection Sqlconn = CreateSqlConnection(databaseFilePath);
                RunSqlCommand(Sqlconn, lstCmd);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        List<Dictionary<string, object>> SelectTable(DatabaseName eDatabaseName, string strTableName, List<string> lstCol, string strSqlWhere)
        {
            string databaseFilePath = GetDatabasePath(eDatabaseName);
            SQLiteCommand Sql_command;
            SQLiteDataReader Sql_reader;
            string str_SelectCmd = string.Empty;
            string setLimt = string.Empty;
            SQLiteConnection SqlConn = CreateSqlConnection(databaseFilePath);
            Dictionary<string, object> dicFieldValues = null;
            List<Dictionary<string, object>> lstSelectResults = new List<Dictionary<string, object>>();

            // set columns we want to select
            string strCol = string.Empty;
            if (lstCol != null)
            {
                foreach (string col in lstCol)
                {
                    strCol += col + " ";
                }
            }
            if (!string.IsNullOrEmpty(strCol.Trim()))
                strCol = strCol.Trim().Replace(" ", ", ");
            else
                strCol = "*";

            List<string> lstTable = getTableList(SqlConn);
            if (lstTable.Contains(strTableName.ToLower()))
            {
                str_SelectCmd = string.Format("SELECT {0} FROM {1}", strCol, strTableName);
                if (!string.IsNullOrEmpty(strSqlWhere))
                    str_SelectCmd += string.Format(" WHERE {0}", strSqlWhere);

                Sql_command = new SQLiteCommand(str_SelectCmd, SqlConn);
                Sql_reader = Sql_command.ExecuteReader();

                List<object> lstObjects = new List<object>();
                List<string> lstFieldNames = getFieldList(SqlConn, strTableName);

                while (Sql_reader.Read())
                {
                    if (strCol.Equals("*"))
                    {
                        dicFieldValues = new Dictionary<string, object>();
                        foreach (string field in lstFieldNames)
                            dicFieldValues.Add(field, Sql_reader[field]);

                        lstSelectResults.Add(dicFieldValues);
                    }
                    else
                    {
                        dicFieldValues = new Dictionary<string, object>();
                        foreach (string field in lstCol)
                            dicFieldValues.Add(field, Sql_reader[field]);

                        lstSelectResults.Add(dicFieldValues);
                    }
                }
            }

            return lstSelectResults;
        }

        bool CheckTableIsExisted(string dbPath, string tableName)
        {
            if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath) ||
                string.IsNullOrEmpty(tableName))
                return false;

            bool rc = false;

            SQLiteCommand Sql_command;
            SQLiteDataReader Sql_reader;
            string strCmd =
                "SELECT name FROM sqlite_master " +
                "WHERE type = 'table' AND name = '" + tableName + "'";

            try
            {
                System.Data.SQLite.SQLiteConnection SqlConn = CreateSqlConnection(dbPath);
                Sql_command = new SQLiteCommand(strCmd, SqlConn);
                Sql_reader = Sql_command.ExecuteReader();

                while (Sql_reader.Read())
                {
                    rc = true;
                }
                CloseSqlConnection(SqlConn);
            }
            catch
            {
                rc = false;
            }

            return rc;
        }

        void DropTable(string tableName, string dbPath)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(dbPath) || !System.IO.File.Exists(dbPath))
                return;

            System.Data.SQLite.SQLiteConnection conn = CreateSqlConnection(dbPath);
            List<string> command_list = new List<string>();

            string strCmd = string.Format("Drop table if exists '{0}'", tableName);

            command_list.Add(strCmd);

            RunSqlCommand(conn, command_list);
            CloseSqlConnection(conn);
        }

        void ClearTable(string tableName, string dbPath)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(dbPath) || !System.IO.File.Exists(dbPath))
                return;

            System.Data.SQLite.SQLiteConnection conn = CreateSqlConnection(dbPath);
            List<string> command_list = new List<string>();

            string strCmd = string.Format("Delete from '{0}'", tableName);

            command_list.Add(strCmd);

            RunSqlCommand(conn, command_list);
            CloseSqlConnection(conn);
        }

        Dictionary<string, string> GetColumnType(string tableName, string dbPath)
        {
            Dictionary<string, string> dicColType = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(dbPath) || !System.IO.File.Exists(dbPath))
                return dicColType;

            System.Data.SQLite.SQLiteConnection SqlConn = CreateSqlConnection(dbPath);

            string strCmd = string.Format("pragma table_info({0})", tableName);
            SQLiteCommand Sql_command = new SQLiteCommand(strCmd, SqlConn);
            SQLiteDataReader Sql_reader = Sql_command.ExecuteReader();

            while (Sql_reader.Read())
            {
                dicColType[Sql_reader["name"].ToString()] = Sql_reader["type"].ToString();
            }

            CloseSqlConnection(SqlConn);

            return dicColType;
        }
        #endregion
#endif

    }
}
