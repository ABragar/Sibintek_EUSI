using Base.Attributes;
using Base.DAL;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет космический корабль.
    /// </summary>
    [Table("SpaceShip")]
    public class SpaceShip : NonCadastral
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SpaceShip.
        /// </summary>
        public SpaceShip() : base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SpaceShip из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public SpaceShip(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }

        /// <summary>
        /// Получает или задает регистрационный номер.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RegNumber { get; set; }


        /// <summary>
        /// Получает или задает обозначение.
        /// </summary>
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Marking { get; set; }


        /// <summary>
        /// Получает или задает средства выведения.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String LaunchVehicle { get; set; }


        /// <summary>
        /// Получает или задает место запуска.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public string LaunchPlace { get; set; }


        /// <summary>
        /// Получает или задает апогей.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? Apogee { get; set; }


        /// <summary>
        /// Получает или задает перигей.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? Perigee { get; set; }

        /// <summary>
        /// Получает или задает наклонение орбиты (град).
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? OrbitalInclination { get; set; }


        /// <summary>
        /// Получает или задает период обращения (мин).
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [DefaultValue(0)]
        public decimal? NodalPeriod { get; set; }


        /// <summary>
        /// Получает или задает дату запуска на орбиту.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public DateTime? LaunchDate { get; set; }


        /// <summary>
        /// Получает или задает дату вывода с орбиты.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public DateTime? DeOrbitingDate { get; set; }

        
    }
}
