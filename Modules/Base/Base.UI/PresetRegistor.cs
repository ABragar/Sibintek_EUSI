using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.UI
{
    [JsonObject]
    [MemberType(nameof(PresetRegistor.Type), nameof(PresetRegistor.Preset))]
    public class PresetRegistor: BaseObject
    {
        [SystemProperty]
        public int? UserID { get; set; }

        [ListView]
        [MaxLength(255)]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [ListView("Тип")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string Type { get; set; }

        [ListView("Для")]
        [DetailView("Для", Required = true)]
        public string For { get; set; }

        [Column]
        private byte[] value_ { get; set; }

        [NotMapped]
        [DetailView]
        [PropertyDataType(PropertyDataType.BtnOpenVm)]
        public Preset Preset
        {
            get
            {
                if (value_ != null)
                {
                    using (var ms = new MemoryStream(value_))
                    {
                        var bin = new BinaryFormatter();

                        try
                        {
                            return bin.Deserialize(ms) as Preset;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    value_ = null;
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        var bin = new BinaryFormatter();

                        bin.Serialize(ms, value);

                        value_ = ms.ToArray();
                    }
                }
            }
        }

        //sib

        /// <summary>
        /// Получает или задает логин пользователя.
        /// </summary>
        public string UserLogin { get; set; }

        //end sib
    }
}
