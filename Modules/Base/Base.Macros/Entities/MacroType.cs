namespace Base.Macros.Entities
{
    public enum MacroType
    {
        String = 0,
        Number,
        DateTime,
        TimeSpan,
        Operator,
        Boolean,
        InitObject,
        BaseObject,
        Function,
        CollectionItem,
        NotNull, 
        EqualNull,
    }
}