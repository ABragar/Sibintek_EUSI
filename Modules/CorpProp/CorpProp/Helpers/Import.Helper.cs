using Base;
using Base.DAL;
using Base.Utils.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Helpers.ImportHistoryHelpers;
using CorpProp.Interfaces;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Представляет методы и свойства для импорта объектов системы из Excel файлов.
    /// </summary>
    public static class ImportHelper
    {
        public const string _FILE_NAME_TEMPLATE_CODE_YYYY_MM_dd = @"^[а-яА-Яa-zA-Z\d]*_[\d]{4}_[\d]{2}_[\d]{2}_\w*";

        /// <summary>
        /// Устанавливает объекту Системы значения из файла Excel.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="_type">Тип объекта.</param>
        /// <param name="_bo">Экземпляр объекта.</param>
        /// <param name="_row">Строка файла.</param>
        /// <param name="_columns">Колонка.</param>
        /// <param name="_error">Текст ошибки.</param>
        /// <returns></returns>
        public static void FillObject(
            this BaseObject _bo
            , IUnitOfWork uofw
            , Type _type
            , DataRow _row
            , DataColumnCollection _columns
            , ref string _error
            , ref ImportHistory history
            , Dictionary<string, string> colsMapping = null
            , bool dictCode = false)
        {
            try
            {
                if (_bo == null || _row == null || _columns == null)
                {
                    _error = $"Строка или колонки пусты.{System.Environment.NewLine}";
                    return;
                }
                PropertyInfo prImportDate = _type.GetProperty("ImportDate");
                PropertyInfo prImportUpdateDate = _type.GetProperty("ImportUpdateDate");

                if (_type.IsSubclassOf(typeof(DictObject)))
                {
                    object obj;
                    PropertyInfo prId = _type.GetProperty("ID");
                    PropertyInfo pr = _type.GetProperty("Name");
                    DataColumn col = _row.Table.Columns[colsMapping.FirstOrDefault(x => x.Value == "Name").Key];
                    DataColumn colCode = _row.Table.Columns[colsMapping.FirstOrDefault(x => x.Value == "Code").Key];
                    DataColumn colExternalId = colsMapping.FirstOrDefault(x => x.Value == "ExternalID").Key != null ? _row.Table.Columns[colsMapping.FirstOrDefault(x => x.Value == "ExternalID").Key] : null;
                    MethodInfo methodUow = typeof(ImportHelper).GetMethod("ChangeDictObjectState");
                    MethodInfo genericUow = methodUow.MakeGenericMethod(_type);

                    //Если грузим НСИ и отсутствует свойство ExternalID, то проверяем по коду +наименование
                    if (colExternalId == null)
                    {
                        obj = GetDictByCodeAndName(uofw, _type
                            , GetValue(uofw, pr.PropertyType, _row[col], ref _error).ToString()
                            , GetValue(uofw, pr.PropertyType, _row[colCode], ref _error).ToString()
                            , ref _error);
                    }
                    else
                    {
                        obj = GetDictByExternalID(uofw, _type,
                            GetValue(uofw, pr.PropertyType, _row[colExternalId], ref _error).ToString(), ref _error);
                    }
                    //TODO:Вернуть до момента загрузки данных на этапе ОПЭ
                    ////Если элемент НСИ уже был то элемент не должен обновляться полями из файла
                    //if(obj!=null)
                    //return (BaseObject)obj;

                    _bo = obj == null ? (BaseObject)genericUow.Invoke(null, new object[] { uofw, obj, "AddRequest", "Temporary" })
                        : (BaseObject)obj;
                }

                //пропускаем 1-ую колонку
                for (int i = 1; i < _columns.Count; i++)
                {
                    _bo.SetValue(uofw, _type, _row, _columns[i].ColumnName, ref _error, ref history, colsMapping, dictCode);
                }
                if (prImportDate != null)
                    prImportDate.SetValue(_bo, DateTime.Now);

                if (prImportUpdateDate != null)
                    prImportUpdateDate.SetValue(_bo, DateTime.Now);

                _error += $"";
                return;
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
            return;
        }

        public static void CheckColumnsExisting(Dictionary<string, string> fieldNamesDictionary,
            Dictionary<string, string> templateColumnsNamesDictionary, string errorType,
            ref ImportHistory history, string errorText = null)
        {
            foreach (var propNamePair in fieldNamesDictionary)
            {
                if (!templateColumnsNamesDictionary.ContainsValue(propNamePair.Value))
                {
                    history.ImportErrorLogs.Add(new ImportErrorLog
                    {
                        MessageDate = DateTime.Now,
                        ErrorText = string.IsNullOrEmpty(errorText) ?
                            $"Файл не соответствует шаблону. Столбец {propNamePair.Value} отсутствует."
                            : $"{errorText}: {propNamePair.Value}",
                        ErrorType = errorType
                    });
                }
            }
        }

        public static void SetValue(
            this BaseObject _bo
           , IUnitOfWork uofw
           , Type _type
           , DataRow _row
           , string propertyName
           , ref string _error
           , ref ImportHistory history
           , Dictionary<string, string> colsMapping = null
           , bool dictCode = false)
        {
            try
            {
                //TODO: обрабатывать свойства через Reflection

                PropertyInfo pr = _type.GetProperty(colsMapping[propertyName]);
                //если нет свойства - продолжаем
                //TODO: обработать лог
                if (pr == null || pr.SetMethod == null) return;
                if (!pr.CanWrite || !pr.GetSetMethod(true).IsPublic)
                    return;

                DataColumn col = _row.Table.Columns[propertyName];
                var value = (pr.Name == "IDEUP" || pr.Name == "AccountNumber") ? GetIDEUP(_row[col]) : GetValue(uofw, pr.PropertyType, _row[col], ref _error, dictCode);

                if (pr.Name == "IsRealEstate")
                {
                    PropertyInfo ppr = _type.GetProperty("IsRealEstateImpl");
                    if (ppr != null)
                    {
                        var bb = ImportAccountingObject.GetIsRealEstate(ppr.GetValue(_bo), value);
                        ppr.SetValue(_bo, bb);
                    }
                }
                pr.SetValue(_bo, value);
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Создает профиль пользователя.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="FIO">ФИО.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="mobile">Моб.телефон.</param>
        /// <param name="email">Почта.</param>
        /// <returns></returns>
        public static SibUser CreateSibUser(
        IUnitOfWork uofw
        , string FIO
        , string phone
        , string mobile
        , string email
        )
        {
            SibUser user = null;
            user = uofw.GetRepository<SibUser>().Create(new SibUser());
            var splitVal = FIO.Split(' ');
            if (splitVal.Count() > 0)
            {
                switch (splitVal.Count())
                {
                    case 1:
                        user.LastName = splitVal[0];
                        break;

                    case 2:
                        user.LastName = splitVal[0];
                        user.FirstName = splitVal[1];
                        break;

                    case 3:
                        user.LastName = splitVal[0];
                        user.FirstName = splitVal[1];
                        user.MiddleName = splitVal[2];
                        break;

                    default:
                        break;
                }

                user.Email = email;
                user.Phone = phone;
                user.Mobile = mobile;
            }
            return user;
        }

        public static object ChangeDictObjectState<T>(IUnitOfWork uofw, object obj, string statusCode, string stateCode) where T : DictObject
        {
            string error = "";
            var tmpObj = obj == null ? Activator.CreateInstance<T>() : (T)obj;

            tmpObj.ImportUpdateDate = DateTime.Now;
            tmpObj.DictObjectState = (DictObjectState)GetDictByCode(uofw, new DictObjectState().GetType(), stateCode, ref error);
            tmpObj.DictObjectStatus = (DictObjectStatus)GetDictByCode(uofw, new DictObjectStatus().GetType(), statusCode, ref error);

            return tmpObj;
        }

        /// <summary>
        /// Получает значение колонки и приводит к типу свойства объекта.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="pr">Метаданные свойства.</param>
        /// <param name="value">Значение ячейки.</param>
        /// <param name="_error">Тект ошибки.</param>
        /// <returns></returns>
        public static object GetValue(
            IUnitOfWork uofw
            , Type type
            , object value
            , ref string _error
            , bool dictCode = false)
        {
            object val = null;
            try
            {
                if (type == null || value == null || value == System.DBNull.Value)
                {
                    _error += "";
                    return null;
                }

                //строка
                if (Type.Equals(type, typeof(string)))
                {
                    if (value is double && (value.ToString().Contains("E") || value.ToString().Contains("e")))
                    {
                        double exp;
                        double.TryParse(value.ToString(), out exp);
                        val = exp.ToString("F0");
                    }
                    else
                        val = value.ToString();
                }
                //TODO: пока ищем справочники по коду, в дальнейшем перейти на ИД.
                //навигационное свойство типа DictObject
                else if (Type.Equals(type, typeof(DictObject)) || type.IsSubclassOf(typeof(DictObject)))
                {
                    string name = value.ToString();

                    if (dictCode || Type.Equals(type, typeof(Consolidation)) || type.IsSubclassOf(typeof(Consolidation)))
                    {
                        string code = name;
                        val = GetDictByCode(uofw, type, code, ref _error);
                    }
                    else
                    {
                        val = GetDictByName(uofw, type, name, ref _error);
                    }
                    if (val == null && Type.Equals(type, typeof(Entities.Law.ScheduleStateYear)))
                    {
                        val = new Entities.Law.ScheduleStateYear()
                        {
                            ID = 0,
                            Name = name
                        };
                    }

                    if (val == null)
                        val = GetDictByCode(uofw, type, name, ref _error);

                    if (val == null)
                        val = GetDictByExternalID(uofw, type, name, ref _error);
                }
                else if (Type.Equals(type, typeof(HDictObject)) || type.IsSubclassOf(typeof(HDictObject)))
                {
                    string name = value.ToString();
                    val = (dictCode) ? GetDictByCode(uofw, type, name, ref _error) : GetDictByName(uofw, type, name, ref _error);
                }
                else if (Type.Equals(type, typeof(Society)) || type.IsSubclassOf(typeof(Society)))
                {
                    string code = value.ToString();
                    val = GetSocietyByIDEUP(uofw, code);
                }
                else if (Type.Equals(type, typeof(Subject)) || type.IsSubclassOf(typeof(Subject)))
                {
                    string code = value.ToString();
                    val = GetSubjectBySDP(uofw, code);
                    if (val == null)
                        val = GetSubjectByINNAndKPP(uofw, code);
                }
                else if (Type.Equals(type, typeof(SibUser)) || type.IsSubclassOf(typeof(SibUser)))
                {
                    val = uofw.GetRepository<SibUser>()
                        .Filter(x => (x.LastName + " " + x.FirstName + " " + x.MiddleName) == value.ToString() && !x.Hidden)
                        .FirstOrDefault();
                }
                else if (Type.Equals(type, typeof(SibDeal)) || type.IsSubclassOf(typeof(SibDeal)))
                {
                    val = uofw.GetRepository<SibDeal>()
                        .Filter(x => x.Number == value.ToString() && !x.Hidden && !x.IsHistory)
                        .FirstOrDefault();
                }
                else if (Type.Equals(type, typeof(Entities.Law.Right)))
                {
                    string numb = value.ToString();

                    var rightRegNumber = uofw.GetRepository<Entities.Law.Right>().Filter(x => x.RegNumber == numb && !x.Hidden).FirstOrDefault();
                    if (rightRegNumber == null)
                    {
                        int strID = 0;
                        int.TryParse(numb, out strID);
                        if (strID != 0)
                        {
                            rightRegNumber = uofw.GetRepository<Entities.Law.Right>().Filter(x => x.ID == strID).FirstOrDefault();
                        }
                        else
                        {
                            var right = new Entities.Law.Right();
                            right.RegNumber = numb;
                            rightRegNumber = right;
                        }
                    }
                    val = rightRegNumber;
                }

                else if (Type.Equals(type, typeof(FileCard)) || type.IsSubclassOf(typeof(FileCard)))
                {
                    string strValue = value.ToString();
                    var fileCardByName = uofw.GetRepository<FileCard>().Filter(x => x.Name == strValue).FirstOrDefault();
                    if (fileCardByName == null)
                    {
                        int strID = 0;
                        int.TryParse(strValue, out strID);
                        if (strID != 0)
                        {
                            fileCardByName = uofw.GetRepository<CorpProp.Entities.Document.FileCard>().Filter(x => x.ID == strID).FirstOrDefault();
                        }
                        else
                        {

                            var categoryID = uofw.GetRepository<Entities.Document.CardFolder>().Filter(f => !f.Hidden).Min(m => m.ID);
                            var file = new Entities.Document.FileCardOne();
                            file.CategoryID = categoryID;
                            file.Name = strValue;
                            Entities.Document.FileCardPermission fileCardPerm = uofw.GetRepository<Entities.Document.FileCardPermission>()
                            .Filter(f => !f.Hidden && f.AccessModifier == Entities.Document.AccessModifier.Everyone)
                            .FirstOrDefault();
                            file.FileCardPermission = fileCardPerm ?? null;
                            file.FileCardPermissionID = fileCardPerm?.ID;
                            fileCardByName = file;
                        }
                    }
                    val = fileCardByName;

                }

                // иное навигационное свойство
                else if (Type.Equals(type, typeof(BaseObject)) || type.IsSubclassOf(typeof(BaseObject)))
                {
                    int id = 0;
                    Int32.TryParse(value.ToString(), out id);
                    val = GetBaseObject(uofw, type, id, ref _error);
                }
                // дата/время
                else if (Type.Equals(type, typeof(DateTime)) || Type.Equals(type, typeof(DateTime?)))
                {
                    DateTime dt = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                    //TODO: доработать с учетом форматов дат!
                    DateTime.TryParse(value.ToString(), out dt);
                    if (dt != System.Data.SqlTypes.SqlDateTime.MinValue.Value && dt != DateTime.MinValue)
                        val = dt;
                }
                else if (Type.Equals(type, typeof(int)))
                {
                    int id = 0;
                    Int32.TryParse(value.ToString(), out id);
                    val = id;
                }
                else if (Type.Equals(type, typeof(int?)))
                {
                    int id = 0;
                    if (Int32.TryParse(value.ToString(), out id))
                        val = id;
                }
                else if (Type.Equals(type, typeof(decimal)))
                {
                    decimal dec = 0m;
                    decimal.TryParse(value.ToString(), out dec);
                    val = dec;
                }
                else if (Type.Equals(type, typeof(decimal?)))
                {
                    decimal dec = 0m;
                    if (decimal.TryParse(value.ToString(), out dec))
                        val = dec;
                }
                else if (Type.Equals(type, typeof(bool)))
                {
                    bool bb = false;
                    int b = 0;
                    if (bool.TryParse(value.ToString(), out bb))
                        val = bb;
                    else if (int.TryParse(value.ToString(), out b))
                        val = (b == 1);
                    else val = bb;
                }
                else if (Type.Equals(type, typeof(bool?)))
                {
                    bool bb = false;
                    int b = 0;
                    if (bool.TryParse(value.ToString(), out bb))
                        val = bb;
                    else if (int.TryParse(value.ToString(), out b))
                        val = (b == 1);

                }
                //TODO: доработать другие типы.
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
                return val;
            }
            //TODO: доработать лог
            return val;
        }



        public static Appraisal GetAppraisalByDateAndNumber(IUnitOfWork uofw, object number, object date, ref string _error)
        {
            Appraisal obj = null;
            DateTime dt = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            if (number != null
                && !String.IsNullOrEmpty(number.ToString())
                && date != null
                && DateTime.TryParse(date.ToString(), out dt))
                obj = uofw.GetRepository<Appraisal>()
                    .Filter(f => f.ReportNumber == number.ToString() && f.AppraisalDate == dt && !f.Hidden && !f.IsHistory)
                    .FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// Получает объект BaseObject по его ИД.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="type">Тип объекта поиска.</param>
        /// <param name="id">ИД объекта поиска.</param>
        /// <param name="_error">Строка ошибки.</param>
        /// <returns></returns>
        public static object GetBaseObject(IUnitOfWork uofw, Type type, int id, ref string _error)
        {
            object obj = null;
            try
            {
                if (id == 0) return null;
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);
                //TODO: метод Find вызывает ошибку приведения типов, в случае, 
                //если ищем дочерний объект по ИД, относящемуся к другому дочернему типу
                //пытаемся найти BisnessArea c ИД =1, а находим SibTaskStatus.

                //находим объект по ИД              
                object[] paramss = new object[] { new object[] { id } };
                MethodInfo method = reposit.GetType().GetMethod("Find", new Type[] { typeof(object[]) });
                obj = method.Invoke(reposit, paramss);
                return obj;

            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }
            return obj;

        }




        /// <summary>
        /// Получает объект типа DictObject по его коду. 
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="code">Код.</param>
        /// <param name="_error">Текст ошибки.</param>
        /// <returns></returns>
        public static object GetDictByCode(IUnitOfWork uofw, Type type, string code, ref string _error)
        {
            object obj = null;
            try
            {
                if (String.IsNullOrEmpty(code)) return null;
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);

                var hiddenObj = new { Value = false };
                var hiddenExpr = Expression.Property(Expression.Constant(hiddenObj), "Value");

                var codeObj = new { Value = code };
                var codeExpr = Expression.Property(Expression.Constant(codeObj), "Value");

                //фильтруем по коду
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                    Expression.And(
                        Expression.Equal(
                            Expression.Property(parameter, "Code"),
                            codeExpr
                        ),
                        Expression.Equal(
                            Expression.Property(parameter, "Hidden"),
                            hiddenExpr
                        )
                    )
                    , parameter);

                MethodInfo method = reposit.GetType().GetMethod("Filter");
                var filter = method.Invoke(reposit, new object[] { lambda });

                ParameterExpression param = Expression.Parameter(type, "x");
                obj = ((IQueryable)filter).Provider.Execute(Expression.Call(
                                  typeof(Enumerable),
                                  "FirstOrDefault",
                                  new Type[] { type },
                                  ((IQueryable)filter).Expression));

                return obj;
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }
            return obj;
        }

        /// <summary>
        /// Получает объект типа DictObject по его коду.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="code">Код.</param>
        /// <param name="_error">Текст ошибки.</param>
        /// <returns></returns>
        public static object GetDictByName(IUnitOfWork uofw, Type type, string name, ref string _error)
        {
            object obj = null;
            try
            {
                if (String.IsNullOrEmpty(name)) return null;
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);

                var hiddenObj = new { Value = false };
                var hiddenExpr = Expression.Property(Expression.Constant(hiddenObj), "Value");

                var nameObj = new { Value = name };
                var nameExpr = Expression.Property(Expression.Constant(nameObj), "Value");

                //фильтруем по коду
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                    Expression.And(
                        Expression.Equal(
                            Expression.Property(parameter, "Name"),
                            nameExpr
                        ),
                        Expression.Equal(
                            Expression.Property(parameter, "Hidden"),
                            hiddenExpr
                        )
                    ),
                    parameter);

                MethodInfo method = reposit.GetType().GetMethod("Filter");
                var filter = method.Invoke(reposit, new object[] { lambda });

                ParameterExpression param = Expression.Parameter(type, "x");
                obj = ((IQueryable)filter).Provider.Execute(Expression.Call(
                                  typeof(Enumerable),
                                  "FirstOrDefault",
                                  new Type[] { type },
                                  ((IQueryable)filter).Expression));

                return obj;
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }
            return obj;
        }

        public static object GetDictByCodeAndName(IUnitOfWork uofw, Type type, string name, string code, ref string _error)
        {
            object obj = null;
            try
            {
                if (String.IsNullOrEmpty(name)) return null;
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);

                var hiddenObj = new { Value = true };
                var hiddenExpr = Expression.Property(Expression.Constant(hiddenObj), "Value");

                var nameObj = new { Value = name };
                var nameExpr = Expression.Property(Expression.Constant(nameObj), "Value");

                var codeObj = new { Value = code };
                var codeExpr = Expression.Property(Expression.Constant(codeObj), "Value");

                //фильтруем по коду
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                    Expression.And(
                        (
                            Expression.Equal(
                                Expression.Property(parameter, "Hidden"),
                                hiddenExpr
                            )
                        ),
                        (Expression.And(
                            Expression.Equal(
                                Expression.Property(parameter, "Name"),
                                nameExpr
                            ),
                            Expression.Equal(
                                Expression.Property(parameter, "Code"),
                                codeExpr
                            )

                        ))
                    ),
                    parameter);

                MethodInfo method = reposit.GetType().GetMethod("Filter");
                var filter = method.Invoke(reposit, new object[] { lambda });

                ParameterExpression param = Expression.Parameter(type, "x");
                obj = ((IQueryable)filter).Provider.Execute(Expression.Call(
                                  typeof(Enumerable),
                                  "FirstOrDefault",
                                  new Type[] { type },
                                  ((IQueryable)filter).Expression));

                return obj;
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }
            return obj;
        }

        public static object GetDictByExternalID(IUnitOfWork uofw, Type type, string externalId, ref string _error)
        {
            object obj = null;

            try
            {
                if (String.IsNullOrEmpty(externalId))
                    return obj;

                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow?.MakeGenericMethod(type);
                var reposit = genericUow?.Invoke(uofw, null);

                var hiddenObj = new { Value = false };
                var hiddenExpr = Expression.Property(Expression.Constant(hiddenObj), "Value");

                var externalIdObj = new { Value = externalId };
                var externalIdExpr = Expression.Property(Expression.Constant(externalIdObj), "Value");

                //фильтруем
                var parameter = Expression.Parameter(type, "dict");
                var lambda = Expression.Lambda(
                    Expression.And(
                        Expression.Equal(
                            Expression.Property(parameter, "Hidden"),
                            hiddenExpr
                        ),
                        Expression.Equal(
                            Expression.Property(parameter, "ExternalID"),
                            externalIdExpr
                        )
                    ),
                    parameter);

                MethodInfo method = reposit?.GetType().GetMethod("Filter");
                var filter = method?.Invoke(reposit, new object[] { lambda });

                if (filter == null)
                    return obj;

                obj = ((IQueryable)filter).Provider.Execute(Expression.Call(
                    typeof(Enumerable),
                    "FirstOrDefault",
                    new Type[] { type },
                    ((IQueryable)filter).Expression));
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }

            return obj;
        }

        public static void UpdateOlderDictObject<T>(IUnitOfWork uofw, Type type) where T : DictObject
        {
            MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
            MethodInfo genericUow = methodUow.MakeGenericMethod(type);
            DateTime dt = DateTime.Today;
            var reposit = genericUow.Invoke(uofw, null);

            var parameter = Expression.Parameter(type, "dict");
            var lambda = Expression.Lambda(
                Expression.And(
                    Expression.Or(
                        Expression.LessThanOrEqual(
                            Expression.Property(parameter, "ImportUpdateDate"),
                            Expression.Constant(dt, typeof(DateTime?))
                        ),
                        Expression.Equal(
                            Expression.Property(parameter, "ImportUpdateDate"),
                            Expression.Constant(null, typeof(DateTime?))
                        )
                    ),
                    Expression.Equal(
                        Expression.Property(parameter, "Hidden"),
                        Expression.Constant(false)
                    )
                ),
                parameter);

            MethodInfo method = reposit.GetType().GetMethod("Filter");
            var filter = method.Invoke(reposit, new object[] { lambda });
            var list = (filter as IQueryable).Cast<T>().ToList();

            if (list.Count > 0)
            {
                foreach (var el in list)
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));
                    obj = (T)ChangeDictObjectState<T>(uofw, el, "DelRequest", "Temporary");
                    uofw.GetRepository<T>().Update(obj);
                    uofw.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Получает значение ячейки строки по наименованию колонки.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="type">Тип получаемого значения.</param>
        /// <param name="row">Строка.</param>
        /// <param name="colName">Наименование колонки.</param>
        /// <returns></returns>
        public static object GetValueByName(
            IUnitOfWork uofw
            , Type type
            , DataRow row
            , string colName
            , Dictionary<string, string> colsMapping)
        {
            object value = null;
            string err = "";
            try
            {
                string columnName = (colsMapping == null) ? colName : colsMapping.FirstOrDefault(x => x.Value == colName).Key;
                DataColumn column = row.Table.Columns[columnName];
                value = ImportHelper.GetValue(uofw, type, row[column], ref err);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// Находит и возвращает ОГ по ИД ЕУП.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="IDEUP">ИД ЕУП.</param>
        /// <returns></returns>
        public static Society GetSocietyByIDEUP(IUnitOfWork uofw, string IDEUP)
        {
            IDEUP = GetIDEUP(IDEUP);
            if (String.IsNullOrEmpty(IDEUP)) return null;
            return uofw.GetRepository<Society>()
                .Filter(x => x.IDEUP == IDEUP && !x.Hidden && !x.IsHistory)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получение ОГ по КСК.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="KSK">КСК.</param>
        /// <returns></returns>
        public static Society GetSocietyByKSK(IUnitOfWork unitOfWork, string KSK)
        {
            if (string.IsNullOrWhiteSpace(KSK)) { return null; }
            else
            {
                KSK = KSK.Trim();
                return unitOfWork.GetRepository<Society>().Filter(f =>
                    f.KSK == KSK &&
                    !f.IsHistory &&
                    !f.Hidden &&
                    f.DateExclusionFromPerimeter == null).FirstOrDefault();
            }
        }

        /// <summary>
        /// Получение ОГ по коду БЕ.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="ConsolidationCode">Код БЕ.</param>
        /// <returns></returns>
        public static Society GetSocietyByConsolidationCode(IUnitOfWork unitOfWork, string ConsolidationCode, ref string _error)
        {
            try
            {
                var be = GetDictByCode(unitOfWork, typeof(Consolidation), ConsolidationCode, ref _error);

                Consolidation cons = null;
                Society soc = null;

                if (Type.Equals(be, typeof(Consolidation)))
                {
                    cons = be as Consolidation;
                    soc = unitOfWork.GetRepository<Society>()
                        .FilterAsNoTracking(x => !x.Hidden && !x.IsHistory && (x.INN == cons.INN || x.ConsolidationUnitID == cons.ID) && x.DateExclusionFromPerimeter == null)
                        .FirstOrDefault();
                    return soc;
                }
            }
            catch (Exception ex)
            {
                _error += $"{ex.ToStringWithInner()} {System.Environment.NewLine}";
            }
            return null;

        }

        /// <summary>
        /// Находит и возвращает ДП или Оценщика по СДП.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="IDEUP">ИД ЕУП.</param>
        /// <returns></returns>
        public static Subject GetSubjectBySDP(IUnitOfWork uofw, string SDP)
        {
            if (String.IsNullOrWhiteSpace(SDP))
            {
                return null;
            }
            else
            {
                SDP = SDP.Trim();
                return uofw.GetRepository<Subject>()
                    .Filter(x => x.SDP == SDP && !x.Hidden && !x.IsHistory)
                    .FirstOrDefault()
                    ;
            }
        }

        /// <summary>
        /// Находит и возвращает ДП или Оценщика по ИНН или ИНН/КПП.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="IDEUP">ИД ЕУП.</param>
        /// <returns></returns>
        public static Subject GetSubjectByINNAndKPP(IUnitOfWork uofw, string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;
            var arr = value.Split('/');
            var inn = arr[0].Trim();
            var kpp = (arr.Length > 1) ? arr[1].Trim() : "";

            Subject subj = null;
            if (!String.IsNullOrEmpty(inn))
            {
                if (!String.IsNullOrEmpty(kpp))
                    subj = uofw.GetRepository<Subject>()
                    .Filter(x => !x.Hidden && !x.IsHistory && x.INN == inn && x.KPP == kpp)
                    .FirstOrDefault()
                    ;
                if (subj == null)
                    subj = uofw.GetRepository<Subject>()
                    .Filter(x => !x.Hidden && !x.IsHistory && x.INN == inn)
                    .FirstOrDefault()
                    ;
            }
            return subj;
        }

        public static void SimpleInstance<T>(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref ImportHistory history
            ) where T : BaseObject
        {
            try
            {
                string err = "";
                var obj = (T)Activator.CreateInstance(typeof(T));
                obj.FillObject(uofw, typeof(T),
                          row, row.Table.Columns, ref err, ref history, colsNameMapping);

                if (Type.Equals(typeof(T), typeof(DictObject)) || obj.GetType().IsSubclassOf(typeof(DictObject)))
                {
                    DictObject dc = obj as DictObject;

                    if (dc != null)
                    {
                        if (String.IsNullOrEmpty(dc.PublishCode) && !String.IsNullOrEmpty(dc.Code))
                            dc.PublishCode = dc.Code;
                        //TODO: в случае импорта данных НСИ, необходимо обновлять и поле Code.
                        if (String.IsNullOrEmpty(dc.Code) && !String.IsNullOrEmpty(dc.PublishCode))
                            dc.Code = dc.PublishCode;
                        dc.TrimCode();
                    }
                }

                if (obj.ID != 0)
                    uofw.GetRepository<T>().Update(obj);
                else
                    uofw.GetRepository<T>().Create(obj);
                uofw.SaveChanges();
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Находит и возвращает тип импортируемых объектов по его названию 1-ой колонки в таблице DataTable.
        /// </summary>
        /// <param name="entryTable">Таблица.</param>
        /// <param name="err">Строка ошибки.</param>
        /// <returns></returns>
        public static Type GetEntityType(DataTable entryTable, ref string err)
        {
            try
            {
                string typeName = entryTable.Rows[2][1].ToString();
                if (String.IsNullOrEmpty(typeName))
                {
                    err += "Неверный формат файла.";
                    return null;
                }

                Type type = TypesHelper.GetTypeByName(typeName);
                if (type == null)
                {
                    err += "Не удалось определить тип импортируемых объектов.";
                    return null;
                }
                return type;
            }
            catch
            {
                err += "Не удалось определить тип импортируемых объектов.";
                return null;
            }
        }

        /// <summary>
        /// Конвертирует строковое значение ИД ЕУП в число и обратно.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="value"></param>
        /// <param name="_error"></param>
        /// <returns></returns>
        public static string GetIDEUP(object idEUP)
        {
            return (idEUP != null) ? idEUP.ToString().Trim() : "";
            /*
            int id = 0;
            if (idEUP != null)
                Int32.TryParse(idEUP.ToString().Trim(), out id);
            if (id != 0)
                return id.ToString().Trim();
            return null;
            */
        }

        /// <summary>
        /// Создает запись истории импорта.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="fileName">Наименование файла.</param>
        /// <param name="userID">ИД пользователя.</param>
        /// <param name="report">Текст ошибки.</param>
        public static ImportHistory CreateImportHistory(
            IUnitOfWork uofw
            , string fileName
            , int? userID
            , string report = null)
        {
            ImportHistory imp = new ImportHistory()
            {
                ResultText = report,
                ImportDateTime = DateTime.Now,
                FileName = fileName
            };

            if (userID != null)
            {
                SibUser us = uofw.GetRepository<SibUser>()
                    .Filter(x => x.UserID == userID && !x.Hidden)
                    .FirstOrDefault();
                if (us != null)
                    imp.SibUser = us;
            }

            return imp;
        }

        /// <summary>
        /// Находит и возвращает регион по его коду.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="code">Код региона.</param>
        /// <returns>Регион.</returns>
        public static SibRegion FindRegionByCode(IUnitOfWork uofw, string code)
        {
            if (String.IsNullOrEmpty(code))
                return null;
            return
                uofw.GetRepository<SibRegion>()
                .Filter(x => !x.Hidden && !x.IsHistory && x.Code == code)
                .FirstOrDefault();
        }

        /// <summary>
        /// Находит справочник типа DictObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uofw"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T FindDictObject<T>(IUnitOfWork uofw, string code) where T : DictObject
        {
            if (String.IsNullOrEmpty(code))
                return null;
            return
                uofw.GetRepository<T>()
                .Filter(x => !x.Hidden && !x.IsHistory && x.Code == code)
                .FirstOrDefault();
        }

        /// <summary>
        /// Находит справочник типа HDictObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uofw"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T FindHDictObject<T>(IUnitOfWork uofw, string code) where T : HDictObject
        {
            if (String.IsNullOrEmpty(code))
                return null;
            return
                uofw.GetRepository<T>()
                .Filter(x => !x.Hidden && x.Code == code)
                .FirstOrDefault();
        }

        /// <summary>
        /// Находит регион по коду.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SibRegion FindRegionOKATO(IUnitOfWork uofw, OKATO obj)
        {
            if (obj == null || String.IsNullOrEmpty(obj.Code) || obj.Code.Length < 2)
                return null;
            string rr = obj.Code.Substring(0, 2);

            return
                uofw.GetRepository<SibRegion>().Find(x => x.Code == rr);
        }

        /// <summary>
        /// Мэппинг имени колонки таблицы с системным свойством.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ColumnsNameMapping(DataTable table)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            int fieldRow = FindFieldSystemNameRow(table);
            for (int c = 1; c < table.Columns.Count; c++)
            {
                if (!result.ContainsKey(table.Columns[c].ColumnName))
                    result.Add(
                        table.Columns[c].ColumnName,
                        table.Rows[fieldRow][c].ToString());
            }
            return result;
        }

        public static string FindSystemName(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "системное имя объекта" || row.ItemArray[0].ToString().ToLower() == "typeobject")
                    return row.ItemArray[1].ToString();
            }
            return "";
        }

        /// <summary>
        /// Возвращает знаечние текущего пользователя, указанного в файле импорта.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <returns></returns>
        public static string FindCurrentFileUser(DataTable table)
        {
            var vals = new List<string>() { "Текущий пользователь".ToLower(), nameof(ImportHistory.CurrentFileUser).ToLower() };
            Func<object, bool> condition = col => (col != DBNull.Value && vals.Contains(col.ToString().Trim().ToLower()));

            var row = table.AsEnumerable().Where(r => r.ItemArray.Any(condition)).FirstOrDefault();
            if (row != null)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    if (condition(row[i]))
                        return row[i + 1].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Возвращает дату/время актуальности данных, указанных в файле импорта.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <returns>Дата/Время актуальности данных, указанных в файле.</returns>
        public static DateTime? GetActualityDate(DataTable table)
        {
            string templateDateFormat = "dd.MM.yyyy HH:mm:ss";
            var vals = new List<string>() { "Актуальность данных".ToLower(), nameof(ImportHistory.ActualityDate).ToLower() };
            Func<object, bool> condition = col => (col != DBNull.Value && vals.Contains(col.ToString().Trim().ToLower()));

            var row = table.AsEnumerable().Where(r => r.ItemArray.Any(condition)).FirstOrDefault();
            if (row != null)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    if (condition(row[i]))
                    {
                        var dateString = row.ItemArray[i + 1].ToString();
                        if (DateTime.TryParseExact(dateString, templateDateFormat,
                            System.Globalization.CultureInfo.GetCultureInfo("ru-RU"),
                            System.Globalization.DateTimeStyles.None, out var date))
                            return date;
                    }
                }
            }
            return null;
        }

        public static string FindTypeName(ExcelDataReader.IExcelDataReader reader)
        {
            var table = reader.GetVisbleTables()[0];
            return FindSystemName(table);
        }

        public static int FindDataVersionValue(ExcelDataReader.IExcelDataReader reader)
        {
            int valueVersion = 0;
            var table = reader.GetVisbleTables()[0];
            string mnemonic = ImportHelper.FindSystemName(table);
            Type entityType = TypesHelper.GetTypeByName(mnemonic);
            if (entityType.Name.ToLower() != "estateregistration")
                valueVersion = GetDataVersionValue(table);
            else
                valueVersion = GetERDataVersionValue(table);
            return valueVersion;
        }

        /// <summary>
        /// Найти системное поле и получить значение Актуальности данных
        /// </summary>
        /// <param name="reader">ExcelDataReader</param>
        /// <returns>DateTime or DateTime.MinValue</returns>
        public static DateTime FindActualityDate(ExcelDataReader.IExcelDataReader reader)
        {
            DateTime valueVersion = DateTime.MinValue;
            var table = reader.GetVisbleTables()[0];
            valueVersion = GetActualityDateValue(table);
            return valueVersion;
        }


        /// <summary>
        /// Метод для получение словаря с номерами столбцов в шаблоне
        /// </summary>
        /// <param name="propNamesDictionary">Словарь свойств, для которых нужны номера столбцов</param>
        /// <param name="colsNameMapping">Все столбцы в шаблоне</param>
        /// <param name="cleanDataTable">Шаблон, обрезанный начиная со строки "№ поля"</param>
        /// <param name="required">Флаг, благодаря которому вызывается исключение, если колонки нет в шаблоне</param>
        /// <returns>Возвращает Dictionary<string, int> из имен свойств и номеров столбцов</returns>
        public static Dictionary<string, int> GetDictionaryOfColumnNumbers(Dictionary<string, string> propNamesDictionary,
            Dictionary<string, string> colsNameMapping, DataTable cleanDataTable, bool required = false)
        {
            Dictionary<string, int> colNumsDict = new Dictionary<string, int>();
            foreach (var propNamePair in propNamesDictionary)
            {
                string columnName = colsNameMapping.FirstOrDefault(x => x.Value == propNamePair.Value).Key;
                int columnNumber = GetColumnNumber(cleanDataTable, columnName, required);
                colNumsDict.Add(propNamePair.Key, columnNumber);
            }

            return colNumsDict;
        }

        public static int FindFieldSystemNameRow(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "системное имя атрибута")
                    return i;
            }
            return 3;
        }

        private static int FindFieldUserNameRow(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "наименование поля")
                    return i;
            }
            return 3;
        }

        private static int FindFieldStartRow(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "№ п/п" || row.ItemArray[0].ToString().ToLower() == "№ поля")
                    return i;
            }
            return 3;
        }

        public static int GetRowSystemNameRow(DataTable table)
        {
            return FindFieldSystemNameRow(table);
        }

        public static int GetRowUserNameRow(DataTable table)
        {
            return FindFieldUserNameRow(table);
        }

        public static int GetRowStartIndex(DataTable table)
        {
            return FindFieldStartRow(table) + 1; //FindFieldSystemNameRow(table);
        }

        public static string GetTemplateName(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "наименование формата")
                    return (row.ItemArray[1] != null) ? row.ItemArray[1].ToString() : "";
            }
            return "";
        }

        /// <summary>
        /// Найти системное поле и получить значение Версии данных
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int GetDataVersionValue(DataTable table)
        {
            int value = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().ToLower() == "версия данных" || row.ItemArray[0].ToString().ToLower() == "dataversion")
                {
                    string tmp = (row.ItemArray[1] != null) ? row.ItemArray[1].ToString() : "0";
                    int.TryParse(tmp, out value);
                    break;
                }
            }
            return value;
        }

        /// <summary>
        /// Найти системное поле и получить значение Версии данных
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static int GetERDataVersionValue(DataTable table)
        {
            int valueColIndex = 0;
            var rowSysProp = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemField").Any()).FirstOrDefault();
            var filedColIndex = 0;
            var valueIndex = 0;
            var tmp = 0;
            try
            {
                for (int i = 0; i < rowSysProp.ItemArray.Length; i++)
                {
                    object col = rowSysProp.ItemArray[i];
                    if (col != null && col.ToString().ToLower() == "dataversion")
                    {
                        filedColIndex = i;
                        break;
                    }
                }

                if (valueColIndex != 0)
                    return valueColIndex;

                var rowValueProp = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "Value").Any()).FirstOrDefault();

                for (int i = 0; i < rowValueProp.ItemArray.Length; i++)
                {
                    object col = rowValueProp.ItemArray[i];
                    if (col != null && col.ToString() == "Value")
                    {
                        valueIndex = i;
                        break;
                    }
                }

                var rowValue = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "DataVersion").Any()).FirstOrDefault();
                int.TryParse((rowValue.ItemArray[valueIndex] != null) ? rowValue.ItemArray[valueIndex].ToString() : "0", out tmp);
                return tmp;
            }
            catch { return tmp; }
        }

        /// <summary>
        /// Найти системное поле и получить значение Актуальности данных
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <returns>DateTime or DateTime.MinValue</returns>
        public static DateTime GetActualityDateValue(DataTable table)
        {
            DateTime value = DateTime.MinValue;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray[0].ToString().Trim().ToLower() == "актуальность данных" || row.ItemArray[0].ToString().Trim().ToLower() == "actualitydate")
                {
                    string tmp = (row.ItemArray[1] != null) ? row.ItemArray[1].ToString() : "";
                    DateTime.TryParse(tmp, out value);
                    break;
                }
            }
            return value;
        }


        public static int GetColumnNumber(DataTable cleanDataTable, string columnName, bool required = false)
        {
            var columnNumber = 0;
            for (int i = 0; i < cleanDataTable.Columns.Count; i++)
            {
                if (cleanDataTable.Columns[i].ColumnName == columnName)
                {
                    columnNumber = i;
                    break;
                }
            }
            if (required && columnNumber == 0)
            {
                throw new ImportException("Обязательная колонка не найдена.");
            }
            return columnNumber;
        }

        /// <summary>
        /// Создает объект имущества из ОБУ.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uofw"></param>
        /// <param name="obj"></param>
        /// <param name="history"></param>
        public static T SimpleEstate<T>(
            IUnitOfWork uofw
            , AccountingObject obj
            , ref ImportHistory history) where T : Estate
        {
            try
            {
                object[] paramss = new object[] { uofw, obj };
                var est = (T)Activator.CreateInstance(typeof(T), paramss);
                return est;
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
            return null;
        }

        public static object CreateEstateByMnemonic(
         IUnitOfWork uofw
         , string mnemonic
        )
        {
            if (String.IsNullOrEmpty(mnemonic))
                return null;
            Type entityType = TypesHelper.GetTypeByName(mnemonic);
            if (entityType != null)
            {
                MethodInfo methodUow = typeof(ImportHelper).GetMethod("SimpleEstate");
                MethodInfo genericUow = methodUow.MakeGenericMethod(entityType);
                return ImportHelper.CreateRepositoryObject(uofw, entityType, null);
            }
            return null;
        }

        /// <summary>
        /// Создает объект в репозитории соответствующего типа.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="type"></param>
        /// <param name="est"></param>
        /// <returns></returns>
        public static object CreateRepositoryObject(IUnitOfWork uofw, Type type, object est)
        {
            object obj = null;
            if (est == null) return null;
            try
            {
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);

                //создаем в репозитории объект
                object[] paramss = new object[] { est };
                MethodInfo method = reposit.GetType().GetMethod("Create");
                obj = method.Invoke(reposit, paramss);
                return obj;
            }
            catch
            {
            }
            return obj;
        }

        public static object UpdateRepositoryObject(IUnitOfWork uofw, Type type, object est)
        {
            object obj = null;
            if (est == null) return null;
            try
            {
                //берем репозиторий
                MethodInfo methodUow = uofw.GetType().GetMethod("GetRepository");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);
                var reposit = genericUow.Invoke(uofw, null);

                //создаем в репозитории объект
                object[] paramss = new object[] { est };
                MethodInfo method = reposit.GetType().GetMethod("Update");
                obj = method.Invoke(reposit, paramss);
                return obj;
            }
            catch
            {
            }
            return obj;
        }

        /// <summary>
        /// Возвращает версию загрузки.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="societyId">ИД ОГ.</param>
        /// <param name="period">Период.</param>
        /// <returns>Версия.</returns>
        public static int GetVersion(IUnitOfWork unitOfWork, string ideup, DateTime period, string mnemonic)
        {
            int idCons = 0;
            bool codeNotNull = !String.IsNullOrWhiteSpace(ideup);
            var qConsID = unitOfWork.GetRepository<Consolidation>()
                .FilterAsNoTracking(f =>
                !f.Hidden
                && codeNotNull
                && f.Code == ideup).Select(s => s.ID).ToList();
            if (qConsID != null && qConsID.Count > 0)
                Int32.TryParse(qConsID[0].ToString(), out idCons);
            var q = unitOfWork.GetRepository<ImportHistory>()
                .FilterAsNoTracking(f =>
                !f.IsHistory
                && !f.Hidden
                && f.IsSuccess
                && f.Period == period
                && f.Society != null
                &&
                ((f.Society.IDEUP == ideup && codeNotNull)
                ||
                 (idCons != 0 && f.Society.ConsolidationUnitID == idCons)
                )
                && f.Mnemonic == mnemonic).Select(s => s.Version).ToList();

            if (q.Count > 0)
                return q.Max(m => m.Value);
            else
                return 0;
        }

        public static DataTable GetDataTableWithoutHeader(DataTable tableWithHeader)
        {
            int rowIndex = ImportHelper.FindFieldStartRow(tableWithHeader);
            var cleanDataHeader = tableWithHeader.Copy();
            for (int i = 0; i < rowIndex; i++)
            {
                cleanDataHeader.Rows[i].Delete();
            }
            cleanDataHeader.AcceptChanges();
            return cleanDataHeader;
        }

        /// <summary>
        /// Возвращает копию исходной талицы с системным наименованием колонок.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable GetNamedDataOnlyTable(DataTable source)
        {
            DataTable result = source.Copy();
            int fieldRow = ImportHelper.FindFieldSystemNameRow(result);
            int k = 0;

            foreach (DataColumn column in result.Columns)
            {
                string columnName = result.Rows[fieldRow][k].ToString();
                if (!String.IsNullOrWhiteSpace(columnName))
                    column.ColumnName = columnName;
                k++;
            }
            //удаление из DataTable хедера таблицы
            var startRow = ImportHelper.GetRowStartIndex(result);
            for (int i = 0; i < startRow; i++)
            {
                result.Rows[i].Delete();
            }

            result.AcceptChanges();
            return result;
        }

        /// <summary>
        /// Метод для анализа значения поля в шаблоне на наличие значения
        /// </summary>
        /// <param name="shortDataTable">Шаблон, содержащий только строки с данными</param>
        /// <param name="dataRow">Текущая строка в анализируемом шаблоне shortDataTable</param>
        /// <param name="columnNumber">Номер столбца</param>
        /// <param name="fieldName">Наименование поля</param>
        /// <param name="history">История импорта</param>
        /// <param name="errorText">Текст ошибки</param>
        public static void ValidateRequiredFieldForNullValue(DataTable shortDataTable, DataRow dataRow,
            int columnNumber, string fieldName, ref ImportHistory history, string errorText = "")
        {
            string value = dataRow[columnNumber].ToString();

            if (string.IsNullOrEmpty(value))
            {
                int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                if (string.IsNullOrEmpty(errorText))
                {
                    history.ImportErrorLogs.AddError(errorRowNumber, columnNumber, fieldName, ErrorType.Required);
                }
                else
                {
                    history.ImportErrorLogs.AddError(errorRowNumber, columnNumber, fieldName, errorText, ErrorType.Required);
                }
            }
        }

        /// <summary>
        /// Проверяет импортировался ли идентичный файл ранее.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="input">Поток.</param>
        /// <returns>True если идентичный файл имопртировался ранее, иначе False.</returns>
        public static bool CheckRepeatImport(IUnitOfWork uow, System.IO.Stream input)
        {
            bool result = false;
            if (input == null)
                return false;
            input.Position = 0;
            var hash = Entities.Document.FileCard.GetHash(input.ToByteArray());

            result = uow.GetRepository<ImportHistory>()
                .FilterAsNoTracking(f => !f.Hidden && f.IsSuccess && !f.IsCanceled
                 && f.FileCard != null && f.FileCard.Hash == hash)
                .Any();
            input.Position = 0;
            return result;
        }

        /// <summary>
        /// Проверяет какая версия данных импортируется.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="input">Поток.</param>
        /// <returns>True если с такой же или меньшей версией данных импортировался ранее, иначе False.</returns>
        public static bool CheckDataVersion(IUnitOfWork uow, string filename, ExcelDataReader.IExcelDataReader reader)
        {
            int tDataVersion = FindDataVersionValue(reader);
            return CheckDataVersionWasImport(uow, filename, tDataVersion);
        }

        /// <summary>
        /// Проверяет импортировался ли идентичный файл ранее.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="input">Поток.</param>
        /// <returns>True если идентичный файл имопртировался ранее, иначе False.</returns>
        public static bool CheckDataVersionWasImport(IUnitOfWork uow, string name, int dataVersion)
        {
            if (name == null)
                return false;
            //var tmp = uow.GetRepository<ImportHistory>()
            //    .FilterAsNoTracking(f => !f.Hidden && f.IsSuccess && !f.IsCanceled
            //     && f.FileCard != null && f.FileCard.Name == name && f.DataVersion >= dataVersion)
            //    .ToList();
            return uow.GetRepository<ImportHistory>()
                .FilterAsNoTracking(f => !f.Hidden && f.IsSuccess && !f.IsCanceled
                 && f.FileCard != null && f.FileCard.Name == name && f.DataVersion >= dataVersion)
                .Any();
        }

        /// <summary>
        /// Проверяет наличие в системе более актуальных данных чем импортируются.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="input">Поток.</param>
        /// <returns>True если более актуальные данные имопртировались ранее, иначе False.</returns>
        public static bool CheckActualityDate(IUnitOfWork uow, string filename, ExcelDataReader.IExcelDataReader reader, ref string errText)
        {
            DateTime tActualityDate = FindActualityDate(reader);
            if (tActualityDate == DateTime.MinValue)
            {
                errText = "В шаблоне импорта отсутствует или некорректно указана информация об актуальности данных.";
                return true;
            }
            else
                return CheckActualityDateWasImport(uow, filename, tActualityDate);
        }

        /// <summary>
        /// Проверяет, в файле указана актуальность данных больше чем есть в системе или в файле актуальность не указана.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="input">Поток.</param>
        /// <returns>False если более актуальные данные имопртировались ранее, иначе True (True и если с такой же актуальностью уже импортировался).</returns>
        public static bool CheckActualityDateWasImport(IUnitOfWork uow, string name, DateTime actualityDate)
        {
            if (name != null && actualityDate != DateTime.MinValue)
            {
                return uow.GetRepository<ImportHistory>()
                    .FilterAsNoTracking(f => !f.Hidden && f.IsSuccess && !f.IsCanceled
                     && f.FileCard != null && f.FileCard.Name == name && f.ActualityDate.Value > actualityDate)
                    .Any();
            }
            else
                return true;
        }



        /// <summary>
        /// Возвращает дату начала или конца периода
        /// </summary>
        /// <param name="arFileName">Массив строк, формируемый на основе парсинга имени файла</param>
        /// <param name="periodPoint">Начало или конец периода</param>
        /// <returns>Начало периода если periodPoint == "Start", конец периода если periodPoint == "End"</returns>
        public static DateTime GetPeriodStartEnd(string[] arFileName, string periodPoint)
        {
            DateTime period = new DateTime(int.Parse(arFileName[1]), int.Parse(arFileName[2]), int.Parse(arFileName[3]));
            var periodStart = new DateTime(period.Year, period.Month, 1);
            switch (periodPoint)
            {
                case "Start":
                    {
                        return periodStart;
                    }
                case "End":
                    {
                        return periodStart.AddMonths(1).AddDays(-1);
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        /// <summary>
        /// Возвращает ConsolidationID 
        /// </summary>
        /// <param name="arFileName">Массив строк, формируемый на основе парсинга имени файла</param>
        public static string GetConsolidationIDFromFileName(string[] arFileName)
        {
            return GetIDEUP(arFileName[0]);
        }

        public static string GetFailedHistoryResultText(string resultText, int count)
        {
            return $"{resultText} Завершено с ошибками. Всего обработано объектов: {count}.\n";
        }

        public static string GetSuccessHistoryResultText(string mnemonic, string resultText, int count)
        {
            return GetSuccessHistoryResultTextStrategy(mnemonic, resultText, count, new ImportResultTextGenerator());
        }

        public static string GetSuccessHistoryResultTextStrategy(string mnemonic, string resultText, int count, IImportResultTextGenerator helper)
        {
            return helper.GetSuccessResultText(mnemonic, resultText, count);
        }

        /// <summary>
        /// Возвращает дату в формате YYYY.MM.dd из передаваемых строковых значений.
        /// </summary>
        /// <param name="arFileName"></param>
        /// <returns></returns>
        public static DateTime? GetPeriod_YYYY_MM_DD(string year, string month, string day)
        {
            try
            {
                return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));                
            }
            catch
            {
                return null;
            }
            
        }
    }
}
