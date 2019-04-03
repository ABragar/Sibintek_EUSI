using CorpProp.Entities.Accounting;
using CorpProp.Interfaces;
using System.Collections.Generic;

namespace CorpProp.Helpers.ImportHistoryHelpers
{
    public class ImportResultTextGenerator : IImportResultTextGenerator
    {
        private readonly Dictionary<string, string> _successResultTexts = new Dictionary<string, string>
        {
            { nameof(AccountingObject), "Загрузка завершена успешно. Загружено объектов ОС\\НМА:" }
        };

        public string GetSuccessResultText(string mnemonic, string resultText, int count)
        {
            if (_successResultTexts.ContainsKey(mnemonic))
            {
                return $"{_successResultTexts[mnemonic]} {count}. \n";
            }
            else
            {
                return $"{resultText} Импорт завершен. Всего обработано объектов: {count}.\n";
            }
        }
    }
}
