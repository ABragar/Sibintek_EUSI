Progress® Telerik® Reporting Examples 

I. Database Setup 
	During installation, the Telerik Reporting installer will search for a running instance of SQL Server, 
	since it needs to install the AdventureWorks database required by our demos. Supported SQL Server instances 
	are SQL Server 2005 Standard/Express editions or later. In case the installer does not find running SQL instance, 
	the AdventureWorks database will not be installed and the connection string in the application configuration file 
	for all demo projects will be left invalid (connectionString="TELERIK_CONNECTION_STRING").

II. Demo projects dependencies
	The MvcDemo and Html5Demo projects depends on the ASP.NET MVC 4 framework (http://www.asp.net/mvc/mvc4), which is included in VS2012.
	Still, if you are using VS2010, ASP.NET MVC 4 for Visual Studio 2010 should be installed in order to load the projects
	(http://www.microsoft.com/en-us/download/details.aspx?id=30683).
	