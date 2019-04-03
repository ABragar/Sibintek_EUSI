using System.Collections.Generic;
using Base.UI.Service;

namespace Base.UI.Helpers
{
    public static class ConfirmImportMessageFormatter
    {
        public static string FormatConfirmImportMessage(IUiFasade uiFacade, string mnemonic, IList<string> fileDescriptions)
        {
            var config = uiFacade.GetViewModelConfig(mnemonic);

            return $"По " +
                   $"{(fileDescriptions.Count > 1 ? $"найденным {config.ImportConfirmNamePlural} " : $"найденной {config.ImportConfirmName}")} " +
                   $"{string.Join("и ", fileDescriptions)} " +
                   $"данные будут перезаписаны";
        }
    }
}
