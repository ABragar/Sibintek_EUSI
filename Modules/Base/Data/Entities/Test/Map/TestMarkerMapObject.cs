using Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Base.Enums;

namespace Data.Entities.Test.Map
{
    public class TestMarkerMapObject : TestBaseMapObject
    {
        [Image]
        [DetailView("Изображение", Order = 0), ListView(Width = 100, Height = 100)]
        public virtual FileData Image { get; set; }

        //[DetailView(Name = "Тестовый геометрический объект", Visible = false), ListView]
        //public virtual TestPathMapObject TestPath { get; set; }

        [DetailView(Name = "Тестовое перечисление", Order = 4), ListView]
        public TestMarkerEnum TestEnum { get; set; }

        [DetailView(Name = "Тестовое целое число (int)", Order = 5), ListView]
        public int TestInt { get; set; }

        [DetailView(Name = "Тестовое дробное число (double)", Order = 6), ListView]
        public double TestDouble { get; set; }

        [DetailView(Name = "Тестовое дробное число (float)", Order = 7), ListView]
        public float TestFloat { get; set; }

        [DetailView(Name = "Тестовое дробное число (decimal)", Order = 8), ListView]
        public decimal TestDecimal { get; set; }

        [DetailView(Name = "Тестовое булево значение", Order = 9), ListView]
        public bool TestBool { get; set; }

        [DetailView(Name = "Тестовое целое число (nullable int)", Order = 10), ListView]
        public int? TestNullableInt { get; set; }

        [DetailView(Name = "Тестовое дробное число (nullable double)", Order = 11), ListView]
        public double? TestNullableDouble { get; set; }

        [DetailView(Name = "Тестовое перечисление (nullable)", Order = 12), ListView]
        public TestMarkerEnum? TestNullableEnum { get; set; }

        [DetailView(Name = "Тестовая Дата", Order = 13), ListView]
        public DateTime? DateTest { get; set; }

        [DetailView(Name = "Тестовая коллекция (Aggregate)", TabName = "[1]Коллекция", Order = 14), ListView]
        public virtual ICollection<TestCollectionItem> TestCollectionItems { get; set; }

        [DetailView(Name = "Тестовые геометрические объекты (Many To Many)", TabName = "[1]Коллекция", Order = 15),
         ListView]
        public virtual ICollection<TestPathMapObject> TestPathItems { get; set; }

        [DetailView(Name = "Тестовые геометрические объекты (EasyCollection)", TabName = "[1]Коллекция", Order = 16),
         ListView]
        public virtual ICollection<TestMarkerPath> TestEasyPathItems { get; set; }

        [DetailView(Name = "Изображения", TabName = "[1]Галерея", HideLabel = true, Order = 16),
         ListView]
        [PropertyDataType(PropertyDataType.Gallery)]
        public virtual ICollection<TestMarkerFile> Images { get; set; }

    }

    public class TestMarkerFile : FileData
    {
        public int? TestMarkerMapObjectID { get; set; }

        [ForeignKey("TestMarkerMapObjectID")]
        public TestMarkerMapObject TestMarkerMapObject { get; set; }
    }
}