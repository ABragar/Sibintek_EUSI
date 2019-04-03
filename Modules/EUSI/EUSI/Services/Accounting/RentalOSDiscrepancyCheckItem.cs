using System;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;

namespace EUSI.Services.Accounting
{
    internal class RentalOSDiscrepancyCheckItem
    {
        private readonly string _name;
        private readonly string _sysName;
        private readonly RentalOSDiscrepancyCondition _condition;
        private readonly Func<RentalOS, string> _fsdValue;
        private readonly Func<AccountingObject, string> _busValue;
        private readonly string format = @"{0} (значение ФСД: {1}, значение БУС: {2}), " + System.Environment.NewLine;

        public RentalOSDiscrepancyCheckItem(string sysName, string name, RentalOSDiscrepancyCondition condition, Func<RentalOS, string> fsdValue, Func<AccountingObject, string> busValue)
        {
            _sysName = sysName;
            _name = name;
            _condition = condition;
            _fsdValue = fsdValue;
            _busValue = busValue;
        }

        public RentalOSDiscrepancyCondition Condition => _condition;

        public string GetMessage(RentalOS rent, AccountingObject os)
        {
            return String.Format(format, _name, _fsdValue.Invoke(rent), _busValue.Invoke(os));
        }

        public string SystemName => _sysName;

        public string Name
        {
            get => _name;
        }
    }
}