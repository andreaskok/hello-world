using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Logs
{
    public static class ERPLog
    {
        public static void WriteDebug(string sLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logTime = String.Empty;
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Debug\\";
            logFilePath = logFilePath + "Debug-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            try
            {
                log = new StreamWriter(fileStream);
                logTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sLog = logTime + ": " + sLog;
                log.WriteLine(sLog);
                log.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WriteLog ex=" + ex.Message);
            }
            finally
            {

            }

        }

        public static void WriteInfo(string sLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logTime = String.Empty;
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Info\\";
            logFilePath = logFilePath + "Info-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            try
            {
                log = new StreamWriter(fileStream);
                logTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sLog = logTime + ": " + sLog;
                log.WriteLine(sLog);
                log.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WriteLog ex=" + ex.Message);
            }
            finally
            {

            }

        }
        public static void WriteWarning(string sLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logTime = String.Empty;
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Warning\\";
            logFilePath = logFilePath + "Warn-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            try
            {
                log = new StreamWriter(fileStream);
                logTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sLog = logTime + ": " + sLog;
                log.WriteLine(sLog);
                log.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WriteLog ex=" + ex.Message);
            }
            finally
            {

            }

        }
        public static void WriteError(string sLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logTime = String.Empty;
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Error\\";
            logFilePath = logFilePath + "Error-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            try
            {
                log = new StreamWriter(fileStream);
                logTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sLog = logTime + ": " + sLog;
                log.WriteLine(sLog);
                log.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WriteLog ex=" + ex.Message);
            }
            finally
            {

            }

        }
        public static void WriteFatal(string sLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logTime = String.Empty;
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Fatal\\";
            logFilePath = logFilePath + "Fatal-" + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "log";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            try
            {
                log = new StreamWriter(fileStream);
                logTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sLog = logTime + ": " + sLog;
                log.WriteLine(sLog);
                log.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WriteLog ex=" + ex.Message);
            }
            finally
            {

            }

        }
    }
}
