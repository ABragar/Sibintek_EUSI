namespace Telerik.Reporting.Examples.CSharp
{
    using System.Collections;

    public class Car
    {
        string manufacturer;
        string model;
        int year;
        string imageUrl;
		ArrayList availableColor;

        public Car(string manufacturer, string model, int year, string imageUrl, string[] availableColor)
        {
            this.manufacturer = manufacturer;
            this.model = model;
            this.year = year;
            this.imageUrl = imageUrl;
			this.AvailableColor = new ArrayList(availableColor);
        }

        public string Model
        {
            get { return this.model; }
            set { this.model = value; }
        }

        public string Manufacturer
        {
            get { return this.manufacturer; }
            set { this.manufacturer = value; }
        }

        public int Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public string ImageUrl
        {
            get { return this.imageUrl; }
            set { this.imageUrl = value; }
        }

		public ArrayList AvailableColor
		{
			get { return this.availableColor; }
			set { this.availableColor = value; }
		}
    }
}
