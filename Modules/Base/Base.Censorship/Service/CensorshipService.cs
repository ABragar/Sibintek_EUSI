using Base.Censorship.Entities;
using Base.Settings;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Base.Helpers;
using Base.Service;
using Base.Utils.Common.Caching;

namespace Base.Censorship.Service
{
    public class CensorshipService: SettingService<CensorshipSetting>, ICensorshipService
    {
        public CensorshipService(IBaseObjectServiceFacade facade, ISimpleCacheWrapper cacheWrapper, IHelperJsonConverter json_converter) : base(facade, cacheWrapper, json_converter)
        {
        }

        public void CheckObsceneLexis(string message)
        {
            var config = Get();
            if (config == null)
                throw new NullReferenceException("CensorshipConfig is null");

            if (!config.TurnOn) return;

            var wordFilter = new Regex(config.Regex, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            var match = wordFilter.Match(message.Replace('ё', 'е'));

            while (match.Success)
            {
                if (!config.WhiteListArray.Any(x => match.Value.Contains(x)))
                    throw new CensorshipException("Текст содержит недопустимое слово: " + match.Value);

                match = match.NextMatch();
            }  
        }
    }
}
