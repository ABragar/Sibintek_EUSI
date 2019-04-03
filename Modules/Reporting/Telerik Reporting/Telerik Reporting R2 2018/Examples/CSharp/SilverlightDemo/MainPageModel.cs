namespace CSharp.SilverlightDemo
{
    using CSharp.ReportLibrary.Silverlight;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Telerik.Reporting;

    public class MainPageModel : INotifyPropertyChanged
    {
        ReportInfo selectedReportInfo;
        IList<ReportInfo> availableReports;

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<ReportInfo> AvailableReports
        {
            get
            {
                return this.availableReports;
            }
            set
            {
                this.availableReports = value;
                this.NotifyPropertyChanged("AvailableReports");
            }
        }

        public ReportInfo SelectedReport
        {
            get { return this.selectedReportInfo; }
            set
            {
                this.selectedReportInfo = value;
                this.NotifyPropertyChanged("SelectedReport");
            }
        }

        void NotifyPropertyChanged(string propertyName)
        {
            if (null != this.PropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainPageModel()
        {
            var list = new List<ReportInfo>();
            foreach (var report in ReportManager.GetReports())
            {
                var reportName = report.GetType().Name;
                list.Add(new ReportInfo()
                {
                    Name = reportName,
                    Description = reportName,
                    Report = report
                });
            }
            this.AvailableReports = list;
        }
    }

    public class ReportInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Report Report { get; set; }
    }
}
