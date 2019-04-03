using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SibAssemblyInfo")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("SibAssemblyInfo")]
[assembly: ComVisible(false)]
[assembly: Guid("8cfb2f34-5e45-4c5c-9e7e-7f5d2781bb1f")]
[assembly: AssemblyVersion(SibiAssemblyInfo.Version)]
[assembly: AssemblyFileVersionAttribute(SibiAssemblyInfo.FileVersion)]
[assembly: NeutralResourcesLanguageAttribute(SibiAssemblyInfo.NeutralResourcesLanguageAttribute)]
[assembly: AssemblyCompany(SibiAssemblyInfo.Company)]
[assembly: AssemblyCopyright(SibiAssemblyInfo.Copyright)]
[assembly: AssemblyCulture(SibiAssemblyInfo.Culture)]
[assembly: AssemblyTrademark(SibiAssemblyInfo.Trademark)]


/// <summary>
/// Класс для глобальной установки версий.
/// </summary>
public class SibiAssemblyInfo
{
    public const string VersionBase = "1.12.18.";
    public const string Version = VersionBase + "8";
    public const string FileVersionBase = VersionBase;
    public const string FileVersion = Version;
    public const string NeutralResourcesLanguageAttribute = "ru-RU";
    public const string Company = "ООО ИК «СИБИНТЕК»";
    public const string Copyright = "© ООО ИК «СИБИНТЕК», 2018";
    public const string Culture = "";
    public const string Trademark = "СИБИ";
    public const string URL = @"http://www.sibintek.ru/";

    public const string AppName = @"АИС «Корпоративная собственность»";
    public const string AppShortName = "АИС КС";
    public const string WelcomeMessage = "Добро пожаловать!";
    
}
