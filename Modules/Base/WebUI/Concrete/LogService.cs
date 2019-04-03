using Base.Service;
using Base.Service.Log;
using System;
using System.IO;
using System.Text;

namespace WebUI.Concrete
{
    public class LogService : ILogService
    {
        private readonly IPathHelper _pathHelper;

        public LogService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;

            string logDir = pathHelper.GetLogDirectory();

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        private string GetFileName()
        {
            return Path.Combine(_pathHelper.GetLogDirectory(), $"log-{DateTime.Now:dd-MM-yyyy}.log");
        }

        private readonly object _locker = new object();
        public void Log(string message)
        {

            string msg = $"{DateTime.Now:HH:mm:ss} -- {message}{Environment.NewLine}";

            string _fileName = GetFileName();

            lock (_locker)
            {
                try
                {
                    if (!File.Exists(_fileName))
                    {
                        File.WriteAllText(_fileName, msg);
                    }
                    else
                    {
                        File.AppendAllText(_fileName, msg);
                    }
                }
                catch (Exception)
                {
                    //TODO : плохо тут, надо бы обработать ошибку
                }
            }

        }
    }
}