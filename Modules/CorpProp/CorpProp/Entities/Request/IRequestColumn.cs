using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using CorpProp.Entities.NSI;

namespace CorpProp.Entities.Request
{
    public interface IRequestColumn : IBaseObject
    {
        /// <summary>
        /// Получает или задает наименование.
        /// </summary>

        string Name { get; set; }

        /// <summary>
        /// Получает или задает ИД типа данных. 
        /// </summary>         
        int? TypeDataID { get; set; }

        /// <summary>
        /// Получает или задает мин. длину текстового поля.
        /// </summary>

        int? MinLength { get; set; }

        /// <summary>
        /// Получает или задает макс. длину текстового поля.
        /// </summary>

        int? MaxLength { get; set; }


        /// <summary>
        /// Получает или задает мин. значение числового поля.
        /// </summary>

        int? MinValue { get; set; }

        /// <summary>
        /// Получает или задает макс. значение числового поля.
        /// </summary>

        int? MaxValue { get; set; }


        /// <summary>
        /// Получает или задает мин. дату.
        /// </summary>

        DateTime? MinDate { get; set; }

        /// <summary>
        /// Получает или задает макс. дату.
        /// </summary>

        DateTime? MaxDate { get; set; }

        /// <summary>
        /// Получает или задает признак обязательности заполнения.
        /// </summary>  
        bool Required { get; set; }

        /// <summary>
        /// Получает или задает инструкцию по заполнению.
        /// </summary>
        string Instruction { get; set; }

        /// <summary>
        /// Получает или задает ИД содержания запроса.
        /// </summary>
        int RequestContentID { get; set; }
        TypeData TypeData { get; set; }


    }
}
