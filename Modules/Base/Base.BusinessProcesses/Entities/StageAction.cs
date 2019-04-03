using Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Security;
using Newtonsoft.Json;

namespace Base.BusinessProcesses.Entities
{
    [JsonObject]
    public class StageAction : Output
    {
        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        public StageAction()
        {
            Color = "#6f5499";
            ShowComment = false;
            RequiredComment = false;
            CloseDV = false;
        }
        

        [ListView]
        [DetailView(Name = "Действие по-умолчанию")]
        public bool IsDefaultAction { get; set; }

        #region sib
        [DetailView(Name = "Закрыть окно детального просмотра по окончании действия")]
        public bool CloseDV { get; set; }

        [DetailView(Name = "Предлагать ввод комментария")]
        public bool ShowComment { get; set; }       
        #endregion

        [DetailView(Name = "Коментарий обязателен")]
        public bool RequiredComment { get; set; }

        [DetailView(Name = "Разрешить загрузку файла")]
        public bool AllowLoadFile { get; set; }


        [DetailView(Name = "Роли")]
        public virtual ICollection<ActionRole> Roles { get; set; }       
        
    }

    public class ActionRole : EasyCollectionEntry<Role>
    {

    }
}
