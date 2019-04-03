using Base.Attributes;

namespace Base.UI.ViewModal
{
    [UiEnum]
    public enum LvAction
    {
        Create,
        Delete,
        ChangeCategory,
        AllCategorizedItems,
        Settings,
        Export,
        Search,
        Edit,
        MultiEdit,
        Import,
        Link,
        Unlink
    }
}