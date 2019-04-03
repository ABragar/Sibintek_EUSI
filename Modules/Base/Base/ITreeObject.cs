namespace Base
{
    /// <summary>
    /// Иерархичный объект
    /// </summary>
    public interface ITreeObject
    {
        int ID { get; }
        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        int? ParentID { get; }
    }
}
