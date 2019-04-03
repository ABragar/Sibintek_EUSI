namespace Base.UI.QueryFilter
{
    public enum OperatorKind
    {
        none,
        equal,// "равно",
        not_equal,// "не равно",

        @in,// "из указанных",
        not_in,// "не из указанных",

        less,// "меньше",
        less_or_equal,// "меньше или равно",

        greater,// "больше",
        greater_or_equal,// "больше или равно",

        between,// "между",
        not_between, // "не между"

        begins_with,// "начинается с",
        not_begins_with,// "не начинается с",

        contains,// "содержит",
        not_contains,// "не содержит",

        ends_with,// "оканчивается на",
        not_ends_with,// "не оканчивается на",

        is_empty,// "пустая строка",
        is_not_empty,// "не пустая строка",

        is_null,// "пусто",
        is_not_null,// "не пусто" 

        quantity_1,// "количество равно 1"
        quantity_less,// "количество меньше"
        quantity_equal,// "количество равно"
        quantity_greater,// "количество больше"
        quantity_from_to,// "количество от до"

        aggregate_sum, // "сумма"
        aggregate_min, // "минимальное"
        aggregate_max, // "максимальное"
        aggregate_average, // "среднее"

        collection_in, // "из указанных для коллекции"
        collection_not_in // "не из указанных для коллекции"
    }
}