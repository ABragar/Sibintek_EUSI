using Base.Censorship.Entities;
using Base.Censorship.Service;
using Base.Settings;
using Base.UI;

namespace Base.Censorship
{

    public class Initializer : IModuleInitializer
    {
        private readonly ICensorshipService _settingService;

        public Initializer(ICensorshipService settingService)
        {
            _settingService = settingService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<CensorshipSetting>()
                .Service< ICensorshipService>()
                .Title("Настройки цензуры")
                .ListView(l => l.Title("Настройки цензуры"))
                .DetailView(d => d.Title("Настройки цензуры"));

            context.DataInitializer("Censorship", "0.1", () =>
            {
                _settingService.Create(context.UnitOfWork, new CensorshipSetting()
                {
                    Title = "Цензура",
                    TurnOn = true,
                    Regex = @"\w{0,5}[хx]([хx\s\!@#\$%\^&*+-\|\/]{0,6})[уy]([уy\s\!@#\$%\^&*+-\|\/]{0,6})[ёiлeеюийя]\w{0,5}|\w{0,5}[пp]([пp\s\!@#\$%\^&*+-\|\/]{0,6})[iие]([iие\s\!@#\$%\^&*+-\|\/]{0,6})[3зс]([3зс\s\!@#\$%\^&*+-\|\/]{0,6})[дd]\w{0,5}|\w{0,5}[сcs][уy]([уy\!@#\$%\^&*+-\|\/]{0,6})[4чkк]\w{0,5}|\w{0,5}[bб]([bб\s\!@#\$%\^&*+-\|\/]{0,6})[lл]([lл\s\!@#\$%\^&*+-\|\/]{0,6})[yя]\w{0,5}|\w{0,5}[её][bб][лске@eыиаa][наи@йвл]\w{0,5}|\w{0,5}[еe]([еe\s\!@#\$%\^&*+-\|\/]{0,6})[бb]([бb\s\!@#\$%\^&*+-\|\/]{0,6})[uу]([uу\s\!@#\$%\^&*+-\|\/]{0,6})[н4ч]\w{0,5}|\w{0,5}[еeё]([еeё\s\!@#\$%\^&*+-\|\/]{0,6})[бb]([бb\s\!@#\$%\^&*+-\|\/]{0,6})[нn]([нn\s\!@#\$%\^&*+-\|\/]{0,6})[уy]\w{0,5}|\w{0,5}[еe]([еe\s\!@#\$%\^&*+-\|\/]{0,6})[бb]([бb\s\!@#\$%\^&*+-\|\/]{0,6})[оoаa@]([оoаa@\s\!@#\$%\^&*+-\|\/]{0,6})[тnнt]\w{0,5}|\w{0,5}[ё]([ё\!@#\$%\^&*+-\|\/]{0,6})[б]\w{0,5}|\w{0,5}[pп]([pп\s\!@#\$%\^&*+-\|\/]{0,6})[иeеi]([иeеi\s\!@#\$%\^&*+-\|\/]{0,6})[дd]([дd\s\!@#\$%\^&*+-\|\/]{0,6})[oоаa@еeиi]([oоаa@еeиi\s\!@#\$%\^&*+-\|\/]{0,6})[рr]\w{0,5}|\w{0,5}[оoеe][еёe][бb][аaоoыy]?\w{0,5}",
                    WhiteList = "корабл рубл страху"
                });
            });
        }
    }
}
