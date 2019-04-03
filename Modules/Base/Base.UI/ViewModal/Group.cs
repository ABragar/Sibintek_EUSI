namespace Base.UI.ViewModal
{
    public class Group
    {
        public string Field { get; set; }

        public Group Copy()
        {
            return new Group()
            {
                Field = this.Field
            };
        }
    }
}