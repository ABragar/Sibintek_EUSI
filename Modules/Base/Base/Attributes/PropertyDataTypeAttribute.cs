using System;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyDataTypeAttribute : Attribute
    {
        public PropertyDataTypeAttribute() { }

        public PropertyDataTypeAttribute(PropertyDataType dataType)
        {
            DataType = dataType;
        }
        public PropertyDataTypeAttribute(string dataType)
        {
            CustomDataType = dataType;
            DataType = PropertyDataType.Custom;
        }
        public PropertyDataType DataType { get; private set; }
        public string CustomDataType { get; private set; }
        public string Params { get; set; }
    }

    public enum PropertyDataType
    {
        Custom,
        DateTime,
        Date,
        Time,
        Month,
        Duration,
        PhoneNumber,
        Currency,
        Text,
        Html,
        SimpleHtml,
        MultilineText,
        EmailAddress,
        Password,
        Url,
        CreditCard,
        PostalCode,
        Upload,
        File,
        FileSelector,
        Files,
        Number,
        Color,
        Icon,
        Percent,
        ListBaseObjects,
        ListWFObjects,
        Mnemonic,
        ObjectType,
        ObjectTypeName,
        Rules,
        Inn,
        Kpp,
        Okpo,
        Oktmo,
        Ogrn,
        Bik,
        Year,
        Location,
        LocationPoint,
        LocationPolygon,
        SelectFromBoundedList,
        MultiSelectFromBoundedList,
        BtnOpenVm,
        DetailViewSetting,
        BPObjectEditButton,
        ConditionalBranch,
        ExtraId,
        Gallery,
        Masked,
        PartialEditor,
        Label
    }
}
