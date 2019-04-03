using System.Collections.Generic;
using Base.Entities.Complex;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;

namespace Base.Support.Entities
{
    public class SupportRequest : BaseSupport
    {
        [ListView, DetailView(Name = "Тип запроса", Order = 1)]
        public SupportRequestType SupportRequestType { get; set; }

        //[DetailView(ReadOnly = true, Order = 900, Name = "История сообщении")]
        //[PropertyDataType("BpHistory")]
        //public string History { get; set; }
    }

    [UiEnum]
    public enum SupportRequestType
    {
        [UiEnumValue("Вопросы по работе с системой", Color = "#6f5499", Icon = "glyphicon glyphicon-circle-plus")]
        System = 0,
        [UiEnumValue("Предложения", Color = "#5dd95d", Icon = "glyphicon glyphicon-circle-plus")]
        Proposal = 1,
        [UiEnumValue("Сообщения об ошибках", Color = "#d9534f", Icon = "glyphicon glyphicon-circle-plus")]
        Error = 2,
    }
}