using System;
using System.Linq;
using EUSI.Entities.Estate;

namespace EUSI.Import
{
    /// <summary>
    /// Класс доп. контроля при импорте заявке согласно задачи 14601
    ///  </summary>
    public class EstateRegistrationComplianceChecker
    {
        private readonly EstateRegistration _estateRegistration;
        private readonly bool _isSubjIsOg;
        private readonly bool _isOgConnectedEusi;
        private bool _result = true;

        public EstateRegistrationComplianceChecker(EstateRegistration estateRegistration, bool isSubjIsOg, bool isOgConnectedEusi)
        {
            _estateRegistration = estateRegistration;
            _isSubjIsOg = isSubjIsOg;
            _isOgConnectedEusi = isOgConnectedEusi;
        }

        /// <summary>
        /// //Если контрагент не является обществом группы, то считается, что контрагент сторонняя организация и не подключён к ЕУСИ
        /// </summary>
        /// <returns></returns>
        public EstateRegistrationComplianceChecker CheckAvailableERTypeIfNotOg()
        {
            if (_result)
            {
                if (!_isSubjIsOg &&
                    new[] { "AccountingObject", "NMA", "NKS", "OSArenda" }.Contains(_estateRegistration.ERType?.Code))
                {
                    _result = false;
                    throw new Exception($"Недопустимый вид объекта заявки {_estateRegistration.ERType?.Name} для контрагента не являющегося обществом группы");
                }
            }

            return this;
        }

        /// <summary>
        /// Если контрагент является ОГ подключенным к ЕУСИ, допустимый вид объекта заявки Внутригрупповые перемещения со способом поступления кроме «Передача в аренду/пользование»
        /// </summary>
        /// <returns></returns>
        public EstateRegistrationComplianceChecker CheckAvailableERTypeIfOgConnectedEusi()
        {
            if (_result)
            {
                if (_isOgConnectedEusi && _estateRegistration.ERType?.Code == "OSVGP")
                {
                    _result = false;
                    throw new Exception($"Недопустимый вид объекта заявки {_estateRegistration.ERType?.Name} для контрагента являющегося обществом группы подключенному к ЕУСИ");
                }
            }

            return this;
        }

        /// <summary>
        /// Если контрагент является ОГ НЕ подключенным к ЕУСИ, допустимые варианты вида объекта заявки:
        /// ОС (кроме аренды); НМА; НКС; ОС (аренда); Внутригрупповые перемещения со способом поступления «Передача в аренду/пользование».
        /// </summary>
        /// <returns></returns>
        public EstateRegistrationComplianceChecker CheckAvailableERTypeIfOgNotConnectedEusi()
        {
            if (_result)
            {
                if (!_isOgConnectedEusi &&
                    (new[] { "AccountingObject", "NMA", "NKS", "OSArenda" }.Contains(_estateRegistration.ERType?.Code) ||
                     (_estateRegistration.ERType?.Code == "OSVGP" &&
                      _estateRegistration.ERReceiptReason?.Code == "RentOut")))
                {
                    _result = false;
                    throw new Exception($"Недопустимый вид объекта заявки {_estateRegistration.ERType?.Name} для контрагента являющегося обществом группы не подключенному к ЕУСИ");
                }
            }

            return this;
        }

        public bool GetResult()
        {
            return _result;
        }
    }
}