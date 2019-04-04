using Base;
using Base.Attributes;
using Base.Enums;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Common.Data.Entities.Test
{
    public class TestObject3: BaseObject
    {
        private const string TabSortable = "[04]Сортируемые";
        private const string TabGroups = "[05]Группируемые";
        private const string TabVisability = "[06]Изменение видимости";
        private const string TabRadio = "[07]Переключатели";
        private const string TabDataTypes = "[08]Типы данных";


        private const string Group_1 = "Group_1_with_order";
        private const string Group_2 = "Group_2_with_2_equal_order_and_2_diferent_order";
        private const string Group_3 = "Group_3_without_order";
        private const string Group_radio_enum = "Group_radio_test_enum";
        private const string Group_radio_bool = "Group_radio_bool";

        [ListView]
        [DetailView]
        public string Name { get; set; }
        
        
        [ListView()]
        [DetailView]
        public DateTime Age { get; set; }

//        [ListView(Hidden = true)]
//        [DetailView(Name = "Земельный участок",
//            Description = "Земельный участок, на котором расположен объект")]
//        public virtual Land Land { get; set; }

        #region Image fields

        [DetailView, ListView]
        public TestObject TestObject { get; set; }

        public int? TestObjectID { get; set; }

        [DetailView(Name = "NoPhoto Image")]
        [Image(DefaultImage = DefaultImage.NoPhoto, Circle = true)]
        public virtual FileData ImageNoPhoto { get; set; }

        [DetailView(Name= "NoImage Image")]
        [Image(DefaultImage = DefaultImage.NoImage, Circle = true)]
        public virtual FileData ImageNoImage { get; set; }
        
        [DetailView(Name= "Gallery Image"), ListView]
        [PropertyDataType(PropertyDataType.Gallery)]
        public virtual ICollection<TestObject3File> ImageGallery { get; set; }

        [DetailView(TabName = "[01] Tab_00", Order = 20)]
        [PropertyDataType(PropertyDataType.Icon)]
        public virtual FileData Image2 { get; set; }

        #endregion

        #region Numberic fields
        
        private Double _money;

        [DetailView(TabName = "[02]Money")]
        public Double Money_double { get => _money;
            set => _money = value;
        }

        [DetailView(TabName = "[02]Money")]
        [ListView(Name = "Money")]
        public Int32 Money_int { get => Convert.ToInt32(_money);
            set => _money = value;
        }

        #endregion

        #region Sortable fields

        [DetailView(TabName = TabSortable, Order = 1, Name = "SortableEl_1")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl1 { get; set; }

        [DetailView(TabName = TabSortable, Order = 1, Name = "SortableEl_4")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl4 { get; set; }


        [DetailView(TabName = TabSortable, Order = 2, Name = "SortableEl_2")]
        [PropertyDataType(PropertyDataType.Currency)]
        public string SortableEl2 { get; set; }

        [DetailView(TabName = TabSortable, Order = 3,  Name = "SortableEl_3")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl3 { get; set; }
        #endregion

        #region Groupable fields
        
        [DetailView(TabName = TabGroups, Order = 1, Group = Group_1, Name = "SortableEl_5")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl5 { get; set; }

        [DetailView(TabName = TabGroups, Order = 3, Group = Group_1, Name = "SortableEl_6")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl6 { get; set; }

        [DetailView(TabName = TabGroups, Order = 2, Group = Group_1, Name = "SortableEl_7")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl7 { get; set; }

        [DetailView(TabName = TabGroups, Order = 1, Group = Group_2, Name = "SortableEl_8")]
        [PropertyDataType(PropertyDataType.Currency)]
        public string SortableEl8 { get; set; }

        [DetailView(TabName = TabGroups, Order = 1, Group = Group_2, Name = "SortableEl_9")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl9 { get; set; }

        [DetailView(TabName = TabGroups, Order = 2, Group = Group_2, Name = "SortableEl_10")]
        [PropertyDataType(PropertyDataType.Currency)]
        public string SortableEl10 { get; set; }

        [DetailView(TabName = TabGroups, Order = 3, Group = Group_2, Name = "SortableEl_11")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl11 { get; set; }

        [DetailView(TabName = TabGroups, Group = Group_3, Name = "SortableEl_12")]
        [PropertyDataType(PropertyDataType.Currency)]
        public string SortableEl12 { get; set; }

        [DetailView(TabName = TabGroups,Group = Group_3, Name = "SortableEl_13")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SortableEl13 { get; set; }

        [ListView(Groupable = true)]
        public int List_view_group { get; set; }

        #endregion

        #region RadioButton fields

        [DetailView(Name = "Radio_1", Group = Group_radio_enum,TabName = TabRadio)]
        public MyRadio Radio { get; set; }

        [DetailView(Name = "Radio_bool_1", Group = Group_radio_bool, TabName = TabRadio)]
        public bool Bool_1 { get; set; }

        [DetailView(Name = "Radio_bool_2", Group = Group_radio_bool, TabName = TabRadio)]
        public bool Bool_2 { get; set; }

        [DetailView(Name = "Radio_bool_3", Group = Group_radio_bool, TabName = TabRadio)]
        public bool Bool_3 { get; set; }

        [DetailView(Name = "bool_4",  TabName = TabRadio)]
        public bool Bool_4 { get; set; }

        [DetailView(Name = "bool_5",TabName = TabRadio)]
        public bool Bool_5 { get; set; }


        #endregion

        #region DataTypes fields

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Currency_int")]
        [PropertyDataType(PropertyDataType.Currency)]
        public Double DT_Val_Currency_int { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Bik_str")]
        [PropertyDataType(PropertyDataType.Bik)]
        public string DT_Val_Bik_str { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Color_int")]
        [PropertyDataType(PropertyDataType.Color)]
        public int DT_Val_Color_int { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Color_str")]
        [PropertyDataType(PropertyDataType.Color)]
        public string DT_Val_Color_str { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Date_str")]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime DT_Val_Date { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_DateTime_int")]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime DT_Val_DateTime { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Duration_int")]
        [PropertyDataType(PropertyDataType.Duration)]
        public int DT_Val_Duration_int { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_EmailAddress_str")]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string DT_Val_EmailAddress_str { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Url_str")]
        [PropertyDataType(PropertyDataType.Url)]
        public string DT_Val_Url_str { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Percent_int")]
        [PropertyDataType(PropertyDataType.Percent)]
        public int DT_Val_Percent_int { get; set; }

        [ListView(Visible = true), DetailView(TabName = TabDataTypes, Name = "DT_Val_Percent_str")]
        [PropertyDataType(PropertyDataType.Percent)]
        public string DT_Val_Percent_str { get; set; }


        #endregion

        #region Visability fields

        [DetailView(TabName = TabVisability, Visible = true, Name = "Val_1_visib_false")]
        public int Val_1 { get; set; }

        [DetailView(TabName = TabVisability, Visible = true, Name = "Val_2_visib_true")]
        public int Val_2 { get; set; }

        [DetailView(TabName = TabVisability, HideLabel = false, Name = "Val_3_hideLab_false")]
        public int Val_3 { get; set; }

        [DetailView(TabName = TabVisability, HideLabel = true, Name = "Val_4_hideLab_true")]
        public int Val_4 { get; set; }

        #endregion
    }
    public class TestObject3File : FileCollectionItem
    {
        public int? TestObject3ObjectID { get; set; }

        [ForeignKey("TestObject3ObjectID")]
        public TestObject3 TestObject3Object { get; set; }
    }
    public enum MyRadio{
        YES,
        NO
    }
    
}