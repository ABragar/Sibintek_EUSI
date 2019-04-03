using System;
using System.Linq.Expressions;
using System.Web.DynamicData;
using Base;
using Base.Attributes;
using Base.UI;
using Base.UI.Presets;
using Base.UI.ViewModal;
using Kendo.Mvc.UI.Fluent;
using WebUI.BoundsRegister;
using WebUI.Models;

namespace WebUI.Service
{
    public interface IColumnBoundRegisterService
    {
        /// <summary>
        /// Регистрация баунда по типу колонки
        /// </summary>
        /// <param name="type">Тип колонки</param>
        /// <returns>Билдер для построения баунда</returns>
        BoundBuilder Register(Type type);

        /// <summary>
        /// Регистрация баунда по Enum.PropertyDataType колонки
        /// </summary>
        /// <param name="propertyDataType">Enum PropertyDataType</param>
        /// <returns>Билдер для построения баунда</returns>
        BoundBuilder Register(PropertyDataType propertyDataType);

        /// <summary>
        /// Регистрация баунда по string.PropertyDataType колонки
        /// </summary>
        /// <param name="propertyDataType">string PropertyDataType</param>
        /// <returns>Билдер для построения баунда</returns>
        BoundBuilder Register(string propertyDataType);

        /// <summary>
        /// Регистрация по типу грида и для конкретного свойства (Если вы регистрируете свойство с типом, который не входит в базовые баунды, то желательно указать InitDefault). 
        /// Для регистрации любого BaseObject-свойства обязательно указывать InitBaseObjectBound-true
        /// Для регистрации любой BaseObject-коллекции обязательно указывать InitBaseObjectCollectionBound-true
        /// </summary>
        /// <typeparam name="T">Тип грида</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="property">Expression с свойством</param>
        /// <returns>Билдер для построения баунда</returns>
        BoundBuilder Register<T, TProperty>(Expression<Func<T, TProperty>> property);

        /// <summary>
        /// Регистрация по типу грида, мнемоники грида и для конкретного свойства (Если вы регистрируете свойство с типом, который не входит в базовые баунды, то желательно указать InitDefault). 
        /// Для регистрации любого BaseObject-свойства обязательно указывать InitBaseObjectBound-true
        /// Для регистрации любой BaseObject-коллекции обязательно указывать InitBaseObjectCollectionBound-true
        /// </summary>
        /// <typeparam name="T">Тип грида</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="mnemonic">Мнемоника грида</param>
        /// <param name="property">Expression с свойством</param>
        /// <returns>Билдер для построения баунда</returns>
        BoundBuilder Register<T, TProperty>(string mnemonic, Expression<Func<T, TProperty>> property);

        ColumnBoundConfig GetBoundByGridTypeAndProperty(Type gridType, string propertyName);

        ColumnBoundConfig GetBoundByGridTypeMnemonicAndProperty(Type gridType, string gridMnemonic, string propertyName);

        ColumnBoundConfig GetBoundByType(Type type);

        ColumnBoundConfig GetBoundByPropertyDataTypeEnum(PropertyDataType propertyDataType);

        ColumnBoundConfig GetBoundByPropertyDataTypeString(string propertyDataType);

        bool ExistsConfig(ColumnViewModel column, ViewModelConfig gridConfig);
    }
}