using EUSI.Entities.Accounting;
using System.Collections.Generic;

namespace EUSI.Validators
{
    public class AccMovMSFOCredit08Validator : AccMovMSFOCreditBaseValidator
    {
        private const string AccountGKCreditValidValue = "08";

        protected override Dictionary<string, string> _propNamesDictionary { get; set; }

        protected override string _accountGKCreditValidValue { get; set; }

        public AccMovMSFOCredit08Validator()
        {
            _propNamesDictionary = new Dictionary<string, string>
            {
                { "propNameConsolidation", nameof(AccountingMovingMSFO.Consolidation) },
                { "propNameSubPosition", nameof(AccountingMovingMSFO.SubPosition) },
                { "propNameDocDate", nameof(AccountingMovingMSFO.DocDate) },
                { "propNameDate", nameof(AccountingMovingMSFO.Date) },
                { "propNameCost", nameof(AccountingMovingMSFO.Cost) },
                { "propNamePositionStorno", nameof(AccountingMovingMSFO.PositionStorno) },
                { "propNameAccountGKDebit", nameof(AccountingMovingMSFO.AccountGKDebit) },
                { "propNamePositionDebit", nameof(AccountingMovingMSFO.PositionDebit) },
                { "propNameIXOInitialDebit", nameof(AccountingMovingMSFO.IXOInitialDebit) },
                { "propNameBusinessUnitDebit", nameof(AccountingMovingMSFO.BusinessUnitDebit) },
                { "propNameGroupObjDebit", nameof(AccountingMovingMSFO.GroupObjDebit) },
                { "propNameAccountGKCredit", nameof(AccountingMovingMSFO.AccountGKCredit) }
            };

            _accountGKCreditValidValue = AccountGKCreditValidValue;
        }
    }
}
