using System;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.DAL;
using Base.Extensions;
using Base.Security.Entities.Concrete;


namespace Base.Security
{
    [EnableFullTextSearch]
    [JsonObject]
    public sealed class User : BaseObject, IUser
    {
        private static readonly CompiledExpression<User, string> _title =
            DefaultTranslationOf<User>.Property(x => x.FullName).Is(x => x.Profile != null ? x.Profile.FullName : x.SysName);

        private static readonly CompiledExpression<User, FileData> _img =
            DefaultTranslationOf<User>.Property(x => x.Image).Is(x => x.Profile != null ? x.Profile.Image : null);

        private static readonly CompiledExpression<User, int?> _profileID =
           DefaultTranslationOf<User>.Property(x => x.ProfileId).Is(x => x.BaseProfileID);

        public User()
        {
            IsActive = true;
        }

        public string SysName { get; set; }

        [SystemProperty]
        [Image(Circle = true)]
        [DetailView("Фотография", Order = -100), ListView(Width = 100, Height = 100)]
        public FileData Image => _img.Evaluate(this);

        [FullTextSearchProperty]
        [DetailView("Ф.И.О.", Order = 1), ListView]
        [SystemProperty]
        public string FullName => _title.Evaluate(this);

        [SystemProperty]
        public CustomStatus CustomStatus { get; set; }

        [DetailView(Visible = false)]
        public int? ProfileId => _profileID.Evaluate(this);

        public int? BaseProfileID { get; set; }

        
        public BaseProfile Profile { get; set; }

        [SystemProperty]
        public bool IsActive { get; set; }

        #region ICategorizedItem
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public UserCategory Category_ { get; set; }
        HCategory ICategorizedItem.Category => Category_;
        #endregion ICategorizedItem

        public User Copy(IUnitOfWork uow)
        {
            var user = new User() { ID = ID };

            if (BaseProfileID != null)
            {
                var profile = uow.GetRepository<BaseProfile>()
                    .All()
                    .Where(x => x.ID == BaseProfileID.Value)
                    .Select(x => new
                    {
                        x.ID,
                        ImageId = x.Image != null ? x.Image.FileID : Guid.Empty,
                        x.FirstName,
                        x.MiddleName,
                        x.LastName
                    }).Single();

                user.Profile = new SimpleProfile()
                {
                    ID = profile.ID,
                    Image = new FileData() { FileID = profile.ImageId },
                    FirstName = profile.FirstName,
                    MiddleName = profile.MiddleName,
                    LastName = profile.LastName
                };
            }

            return user;
        }
    }
}