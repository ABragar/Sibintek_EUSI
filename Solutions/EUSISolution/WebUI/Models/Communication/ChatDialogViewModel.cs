using Base.Conference.Entities;
using Base.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WebUI.Models
{
    public class ChatDialogViewModel
    {
        public ChatDialogViewModel()
        {
        }

        public ChatDialogViewModel(Conference conference)
        {
            ID = conference.ID;
            ImageID = Guid.Empty;
            Title = conference.Title;
            DialogType = ChatDialogType.Conference;
            Members = conference.Members.Select(x => new ChatDialogViewModel(x.Object)).ToList();
        }

        public ChatDialogViewModel(User user)
        {
            ID = user.ID;
            ImageID = user.Image != null ? user.Image.FileID : Guid.Empty;
            Title = user.FullName;
            DialogType = ChatDialogType.Private;
        }

        // ToUserID или ToConferenceID
        public int ID { get; set; }

        public Guid ImageID { get; set; }

        // User.FullName или Conference.Title
        public string Title { get; set; }

        public ChatDialogType DialogType { get; set; }

        public List<ChatDialogViewModel> Members { get; set; }
    }

    public enum ChatDialogType
    {
        [Description("Личный диалог")]
        Private = 10,

        [Description("Конференция")]
        Conference = 20
    }
}