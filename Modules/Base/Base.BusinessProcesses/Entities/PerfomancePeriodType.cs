using System.ComponentModel;
using Base.Attributes;

namespace Base.BusinessProcesses.Entities
{
    [UiEnum]
    public enum PerfomancePeriodType
    {
        [UiEnumValue("В календарных днях")]        
        Calendar = 0,
        [UiEnumValue("В рабочих днях")]
        Workday = 1,
    }
}