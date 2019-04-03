using System;
using CorpProp.Entities.Request;
using Newtonsoft.Json;

namespace WebUI.Models.CorpProp.Response
{
    public class RequestColumnConstrain
    {

        public static TOut DeepCopy<TIn, TOut>(TIn self)
        {
            var serialized = JsonConvert.SerializeObject(self, new JsonSerializerSettings()
                                                               {
                                                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                   StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                                                                   MaxDepth = 1
                                                               });
            return JsonConvert.DeserializeObject<TOut>(serialized);
        }

        public static RequestColumnConstrain CreateFrom(IRequestColumn column)
        {
            return DeepCopy<IRequestColumn, RequestColumnConstrain>(column);
        }
        /// <summary>
        /// ѕолучает или задает наименование.
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// ѕолучает или задает мин. длину текстового пол€.
        /// </summary>

        public int? MinLength { get; set; }

        /// <summary>
        /// ѕолучает или задает макс. длину текстового пол€.
        /// </summary>

        public int? MaxLength { get; set; }


        /// <summary>
        /// ѕолучает или задает мин. значение числового пол€.
        /// </summary>

        public int? MinValue { get; set; }

        /// <summary>
        /// ѕолучает или задает макс. значение числового пол€.
        /// </summary>

        public int? MaxValue { get; set; }


        /// <summary>
        /// ѕолучает или задает мин. дату.
        /// </summary>

        public DateTime? MinDate { get; set; }

        /// <summary>
        /// ѕолучает или задает макс. дату.
        /// </summary>

        public DateTime? MaxDate { get; set; }


        /// <summary>
        /// ѕолучает или задает список значений.
        /// </summary>
        /// <remarks>
        /// —писок возможных значений, разделЄнных точками с зап€той.
        /// </remarks>
        public bool HasItems { get; set; }


        /// <summary>
        /// ѕолучает или задает признак об€зательности заполнени€.
        /// </summary>  
        public bool Required { get; set; }

        /// <summary>
        /// ѕолучает или задает инструкцию по заполнению.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// —писок возможных значений
        /// </summary>
        public bool HasColumnItems { get; set; }

    }
}