//using Base.Attributes;
//using Base.UI.ViewModal;
//using Data.Enums;
//using System;

//namespace Data.Entities
//{
//    public class Aircraft : InventoryObject
//    {
//        [DetailView("Вид", Required = true)]
//        [ListView]
//        public KindOfVessel KindOfVessel { get; set; }

//        [DetailView("Тип", Required = true)]
//        [ListView]
//        public TypeOfVessel TypeOfVessel { get; set; }

//        [DetailView("Идентификационный номер")]
//        [ListView]
//        public string IdentificationNumber { get; set; }

//        [DetailView("Обозначение")]
//        [ListView]
//        public string ADesignation { get; set; }

//        [DetailView("Номер аппарата")]
//        [ListView]
//        public string AirframeNumber { get; set; }

//        [DetailView("Номер двигателя")]
//        [ListView]
//        public string EngineNumbers { get; set; }

//        [DetailView("Количество вспомогательных двигателей")]
//        [ListView]
//        public string NumbersOfAuxiliaryEngine { get; set; }

//        [DetailView("Дата изготовления")]
//        [ListView]
//        [PropertyDataType(PropertyDataType.Date)]
//        public DateTime? DateOfManufacture { get; set; }

//        [DetailView("Изготовитель")]
//        [ListView]
//        public string ManufacturersName { get; set; }
//    }
//}
