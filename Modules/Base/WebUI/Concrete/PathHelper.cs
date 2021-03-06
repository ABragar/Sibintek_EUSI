﻿using System;
using Base.Service;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace WebUI.Concrete
{
    public class PathHelper : IPathHelper
    {
        public string GetRootDirectory()
        {
            return HttpRuntime.AppDomainAppPath;
        }

        public string GetTempDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Temp");
        }

        public string GetFilesDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Files");
        }

        public string GetLogDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Log");
        }


        public string GetVideoConvertDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "VideoConvert");
        }


        public string GetAudioConvertDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "AudioConvert");
        }

        public string GetContentDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Content");
        }

        public string GetViewsDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Views");
        }

        public string GetViewsSharedDirectory()
        {
            return Path.Combine(GetViewsDirectory(), "Shared");
        }

        public string GetAppDataDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data");
        }

        public string GetHostUrl()
        {
            return WebConfigurationManager.AppSettings["currentHostName"];
        }

        public string GetHostUrl(string rightPart)
        {
            return new Uri(new Uri(this.GetHostUrl()), rightPart).ToString();
        }

        public string GetHostName()
        {
            return new Uri(this.GetHostUrl()).Host;
        }
    }
}