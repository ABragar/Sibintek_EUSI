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
        /// �������� ��� ������ ������������.
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// �������� ��� ������ ���. ����� ���������� ����.
        /// </summary>

        public int? MinLength { get; set; }

        /// <summary>
        /// �������� ��� ������ ����. ����� ���������� ����.
        /// </summary>

        public int? MaxLength { get; set; }


        /// <summary>
        /// �������� ��� ������ ���. �������� ��������� ����.
        /// </summary>

        public int? MinValue { get; set; }

        /// <summary>
        /// �������� ��� ������ ����. �������� ��������� ����.
        /// </summary>

        public int? MaxValue { get; set; }


        /// <summary>
        /// �������� ��� ������ ���. ����.
        /// </summary>

        public DateTime? MinDate { get; set; }

        /// <summary>
        /// �������� ��� ������ ����. ����.
        /// </summary>

        public DateTime? MaxDate { get; set; }


        /// <summary>
        /// �������� ��� ������ ������ ��������.
        /// </summary>
        /// <remarks>
        /// ������ ��������� ��������, ���������� ������� � �������.
        /// </remarks>
        public bool HasItems { get; set; }


        /// <summary>
        /// �������� ��� ������ ������� �������������� ����������.
        /// </summary>  
        public bool Required { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� �� ����������.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// ������ ��������� ��������
        /// </summary>
        public bool HasColumnItems { get; set; }

    }
}