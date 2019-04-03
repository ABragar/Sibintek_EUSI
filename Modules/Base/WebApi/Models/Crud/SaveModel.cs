namespace WebApi.Models.Crud
{
    public class SaveModel<T>
    {
        public T model { get; set; }

        public SaveLinkSourceModel link { get; set; }
    }
}