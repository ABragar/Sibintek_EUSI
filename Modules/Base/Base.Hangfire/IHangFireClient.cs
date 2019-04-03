namespace Base.Hangfire
{
    public interface IHangFireClient
    {
        void Process(int id);
    }
}