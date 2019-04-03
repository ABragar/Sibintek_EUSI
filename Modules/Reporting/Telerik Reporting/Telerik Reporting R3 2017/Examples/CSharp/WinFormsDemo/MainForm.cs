namespace Telerik.Reporting.Examples.CSharp.WinFormsDemo
{
    using System.Windows.Forms;

    public partial class MainForm : Form
	{
        public MainForm()
		{
			InitializeComponent();
		}

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
	}
}