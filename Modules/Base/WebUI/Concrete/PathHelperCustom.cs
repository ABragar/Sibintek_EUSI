using System;
using System.Configuration;
using Base.Service;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace WebUI.Concrete
{
    public class PathHelperCustom : PathHelper
    {
        static PathHelperCustom()
        {
            Update();
        }

        private static string GetFileFullNameFromConfig(string configName)
        {
            var configValue = ConfigurationManager.AppSettings[configName];
            if (configValue == null)
                return null;
            var fileValue = new FileInfo(configValue);
            return fileValue.FullName;
        }

        public static void Update()
        {
            _rootDirectory = GetFileFullNameFromConfig("RootDirectory");
            _tempDirectory = GetFileFullNameFromConfig("TempDirectory");
            _filesDirectory = GetFileFullNameFromConfig("FilesDirectory");
            _logDirectory = GetFileFullNameFromConfig("LogDirectory");
            _videoConvertDirectory = GetFileFullNameFromConfig("VideoConvertDirectory");
            _audioConvertDirectory = GetFileFullNameFromConfig("AudioConvertDirectory");
            _contentDirectory = GetFileFullNameFromConfig("ContentDirectory");
            _viewsDirectory = GetFileFullNameFromConfig("ViewsDirectory");
            _viewsSharedDirectory = GetFileFullNameFromConfig("ViewsSharedDirectory");
            _appDataDirectory = GetFileFullNameFromConfig("AppDataDirectory");
        }

        private static string _rootDirectory;
        public override string GetRootDirectory()
        {
            return _rootDirectory ?? HttpRuntime.AppDomainAppPath;
        }
        private static string _tempDirectory;
        public override string GetTempDirectory()
        {
            return _tempDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "Temp");
        }
        private static string _filesDirectory;
        public override string GetFilesDirectory()
        {
            return _filesDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "Files");
        }
        private static string _logDirectory;
        public override string GetLogDirectory()
        {
            return _logDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "Log");
        }
        private static string _videoConvertDirectory;
        public override string GetVideoConvertDirectory()
        {
            return _videoConvertDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "VideoConvert");
        }
        private static string _audioConvertDirectory;
        public override string GetAudioConvertDirectory()
        {
            return _audioConvertDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "AudioConvert");
        }
        private static string _contentDirectory;
        public override string GetContentDirectory()
        {
            return _contentDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "Content");
        }
        private static string _viewsDirectory;
        public override string GetViewsDirectory()
        {
            return _viewsDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "Views");
        }
        private static string _viewsSharedDirectory;
        public override string GetViewsSharedDirectory()
        {
            return _viewsSharedDirectory ?? Path.Combine(GetViewsDirectory(), "Shared");
        }
        private static string _appDataDirectory;
        public override string GetAppDataDirectory()
        {
            return _appDataDirectory ?? Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data");
        }
    }
}