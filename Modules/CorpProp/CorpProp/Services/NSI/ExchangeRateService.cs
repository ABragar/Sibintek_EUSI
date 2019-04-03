using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Base.DAL;
using Base.Enums;
using Base.Events;
using Base.Service;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace CorpProp.Services.NSI
{
    public interface IExchangeRateService : IBaseObjectService<ExchangeRate>
    {
        bool DisableEvents { get; }

        void Import(string filePath, IUnitOfWork unitOfWork);
    }

    public class ExchangeRateService : BaseObjectService<ExchangeRate>, IExchangeRateService
    {
        private static readonly int SqlNotUniqueEntityExceptionCode = 2601;

        public ExchangeRateService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        // Перегружено для перехвата DbUpdateExcpetion, которое кидает БД
        // при попытке добавить курс валюты на дату, на которую он уже задан
        public override ExchangeRate Create(IUnitOfWork unitOfWork, ExchangeRate obj)
        {
            try
            {
                return base.Create(unitOfWork, obj);
            }
            catch (DbUpdateException e)
                when ((e.InnerException.InnerException as SqlException)?.Number == SqlNotUniqueEntityExceptionCode)
            {
                throw new ExchangeRateAlreadyDefinedException(obj);
            }
        }

        // Перегружено для перехвата DbUpdateExcpetion, которое кидает БД
        // при попытке добавить курс валюты на дату, на которую он уже задан
        public override IReadOnlyCollection<ExchangeRate> CreateCollection(IUnitOfWork unitOfWork,
            IReadOnlyCollection<ExchangeRate> collection)
        {
            SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(ExchangeRate), TypePermission.Create);

            var sortOrder = GetMaxSortOrder(unitOfWork) + 1;

            var res = new List<ExchangeRate>();

            foreach (var obj in collection)
                try
                {
                    if (obj.SortOrder == -1)
                        obj.SortOrder = sortOrder++;

                    var objectSaver = GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

                    unitOfWork.GetRepository<ExchangeRate>().Create(objectSaver.Dest);

                    res.Add(objectSaver.Dest);
                }
                catch (DbUpdateException e)
                    when ((e.InnerException.InnerException as SqlException)?.Number == SqlNotUniqueEntityExceptionCode)
                {
                    throw new ExchangeRateAlreadyDefinedException(obj);
                }

            unitOfWork.SaveChanges();


            if (!DisableEvents)
                foreach (var obj in res)
                {
                    var item = obj;
                    OnCreate.Raise(() => new OnCreate<ExchangeRate>(item, unitOfWork));
                }

            return res;
        }

        // Перегружено для перехвата DbUpdateExcpetion, которое кидает БД
        // при попытке добавить курс валюты на дату, на которую он уже задан
        public override ExchangeRate Update(IUnitOfWork unitOfWork, ExchangeRate obj)
        {
            try
            {
                return base.Update(unitOfWork, obj);
            }
            catch (DbUpdateException e)
                when ((e.InnerException.InnerException as SqlException)?.Number == SqlNotUniqueEntityExceptionCode)
            {
                throw new ExchangeRateAlreadyDefinedException(obj);
            }
        }

        // Перегружено для перехвата DbUpdateExcpetion, которое кидает БД
        // при попытке добавить курс валюты на дату, на которую он уже задан
        public override IReadOnlyCollection<ExchangeRate> UpdateCollection(IUnitOfWork unitOfWork,
            IReadOnlyCollection<ExchangeRate> collection)
        {
            var res = new List<IObjectSaver<ExchangeRate>>();

            double sortOrder = -1;

            foreach (var obj in collection)
                try
                {
                    var objectSaver = GetForSave(unitOfWork, unitOfWork.GetObjectSaver(obj, null));

                    if (objectSaver.IsNew)
                    {
                        SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(ExchangeRate), TypePermission.Create);

                        if (objectSaver.Dest.SortOrder == -1)
                        {
                            if (sortOrder == -1)
                                sortOrder = GetMaxSortOrder(unitOfWork) + 1;

                            objectSaver.Dest.SortOrder = sortOrder++;
                        }

                        unitOfWork.GetRepository<ExchangeRate>().Create(objectSaver.Dest);
                    }
                    else
                    {
                        SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(ExchangeRate), TypePermission.Write);
                        //TODO: access
                        //SecurityService.ThrowIfAccessDenied(unitOfWork, typeof(T), obj.ID, TypePermission.Write, AccessType.Update);

                        unitOfWork.GetRepository<ExchangeRate>().Update(objectSaver.Dest);
                    }

                    res.Add(objectSaver);
                }
                catch (DbUpdateException e)
                    when ((e.InnerException.InnerException as SqlException)?.Number == SqlNotUniqueEntityExceptionCode)
                {
                    throw new ExchangeRateAlreadyDefinedException(obj);
                }

            unitOfWork.SaveChanges();


            if (!DisableEvents)
                foreach (var objectSaver in res)
                {
                    var item = objectSaver.Dest;
                    if (objectSaver.IsNew)
                        OnCreate.Raise(() => new OnCreate<ExchangeRate>(item, unitOfWork));
                    else
                        OnUpdate.Raise(
                            () => new OnUpdate<ExchangeRate>(objectSaver.Original, objectSaver.Dest, unitOfWork));
                }

            return res.Select(x => x.Dest).ToList();
        }

        public void Import(string filePath, IUnitOfWork unitOfWork)
        {
            var tempFileDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filePath));
            Directory.CreateDirectory(tempFileDir);
            var tempFilePath = Path.Combine(tempFileDir, Path.GetRandomFileName().Substring(0, 7) + ".dbf");
            CreateHardLink(tempFilePath, filePath, IntPtr.Zero);

            var fileData = DbfReader.Invoke(tempFilePath);

            foreach (var exchangeRate in ToExchangeRates(fileData.Tables[0].Rows, unitOfWork))
                Create(unitOfWork, exchangeRate);
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
        );

        private static IEnumerable<ExchangeRate> ToExchangeRates(DataRowCollection dataRowCollection,
            IUnitOfWork unitOfWork)
        {
            var idByCodes = new Dictionary<string, int>();
            return from DataRow dataRow in dataRowCollection
                select new ExchangeRate
                {
                    Amount = 1,
                    Rate = (decimal) dataRow.ItemArray[2],
                    Date = (DateTime) dataRow.ItemArray[0],
                    CurrencyID =
                        GetCurrencyIdByCode(unitOfWork, idByCodes, dataRow.ItemArray[3].ToString(),
                            dataRow.ItemArray[4].ToString())
                };
        }

        private static int GetCurrencyIdByCode(IUnitOfWork unitOfWork, Dictionary<string, int> idByCodes,
            string numericCode, string letterCode)
        {
            if (idByCodes.ContainsKey(numericCode))
                return idByCodes[numericCode];
            var currency = unitOfWork.GetRepository<Currency>()
                .All()
                .ToList()
                .FirstOrDefault(
                    p =>
                        p.Code == numericCode &&
                        string.Equals(p.LetterCode.ToLower(), letterCode.ToLower()));

            if (currency == null)
                throw new ArgumentException("Валюта с кодом " + letterCode + " (" + numericCode + ") не найдена.");
            var id = currency
                .ID;
            idByCodes[numericCode] = id;
            return id;
        }

        public class ExchangeRateAlreadyDefinedException : Exception
        {
            public ExchangeRateAlreadyDefinedException()
            {
            }

            public ExchangeRateAlreadyDefinedException(string message) : base(message)
            {
            }

            public ExchangeRateAlreadyDefinedException(ExchangeRate exchangeRate)
                : base(
                    "Курс валюты " + exchangeRate.Currency.Code + " на дату " + exchangeRate.Date.ToShortDateString() +
                    " уже задан.")
            {
            }

            public ExchangeRateAlreadyDefinedException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            protected ExchangeRateAlreadyDefinedException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        private class FakeReadOnlyCollection<T> : IReadOnlyCollection<T>
        {
            private readonly IEnumerable<T> _enumerable;

            public FakeReadOnlyCollection(IEnumerable<T> enumerable, int count)
            {
                _enumerable = enumerable;
                Count = count;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _enumerable.GetEnumerator();
            }

            public int Count { get; }
        }
    }
}
