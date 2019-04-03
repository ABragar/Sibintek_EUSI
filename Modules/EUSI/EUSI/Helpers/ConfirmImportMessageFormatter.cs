using System.Collections.Generic;
using Base.UI.Service;

namespace EUSI.Helpers
{
    public static class ConfirmImportMessageFormatter
    {
        public static string FormatConfirmImportMessage(string singleName, string pluralName, IList<string> fileDescriptions)
        {
            return $"По " +
                   $"{(fileDescriptions.Count > 1 ? $"найденным {pluralName} " : $"найденной {singleName}")} " +
                   $"{string.Join("и ", fileDescriptions)} " +
                   $"данные будут перезаписаны";
        }
    }
}