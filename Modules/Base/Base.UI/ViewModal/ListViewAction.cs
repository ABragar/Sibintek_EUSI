namespace Base.UI.ViewModal
{
    public class ListViewAction
    {   
        public LvAction Value { get; set; }

        public ListViewAction Copy()
        {
            return new ListViewAction()
            {
                Value = Value
            };
        }
    }
}