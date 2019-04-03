using System;
using Base.Attributes;

namespace Base.Security
{
    public interface IUserStatus
    {
        int UserId { get; set; }
        string ConnectionId { get; set; }
        bool Online { get; set; }
        bool Updated { get; set; }
        DateTime LastDate { get; set; }
        DateTime? LastPublicDate { get; set; }
        CustomStatus CustomStatus { get; set; }
        CustomStatus? LastCustomStatus { get; set; }
        CustomStatus? LastPublicCustomStatus { get; set; }
        IUserStatus GetPublicVersion();
    }

    [UiEnum]
    public enum CustomStatus
    {
        [UiEnumValue("Готов к контакту", Icon = "glyphicon glyphicon-ok-2", Color = "rgba(145, 218, 0, 0.9)")]
        Ready = 0,

        [UiEnumValue("Отсутствует", Icon = "glyphicon glyphicon-ok", Color = "rgba(249, 206, 21, 0.93)")]
        Away = 1,

        [UiEnumValue("Не беспокоить", Icon = "glyphicon glyphicon-minus", Color = "rgba(235, 6, 21, 0.87)")]
        DontDisturb = 2,

        [UiEnumValue("Отключен", Icon = "halfling halfling-off", Color = "rgba(255,255,255, 0.93)")]
        Disconnected = 3,

        [UiEnumValue("Ведет разговор", Icon = "glyphicon glyphicon-headphones", Color = "rgba(91, 192, 222, 0.93)")]
        InConversation = 10,
    }
}