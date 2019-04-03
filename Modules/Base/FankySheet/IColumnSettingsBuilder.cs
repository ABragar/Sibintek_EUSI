namespace FankySheet
{
    public interface IColumnSettingsBuilder
    {
        IColumnSettingsBuilder Width(double? width);
        IColumnSettingsBuilder Caption(string caption);
    }
}