using Base.DAL;
using Base.Enums;
using Base.Security.Entities.Concrete;
using Base.Service;
using System;

namespace Base.Security.Service.Concrete
{
    /// <summary>
    /// Заполнить юзеров для тестов
    /// </summary>
    public class FeelUsers
    {
        public FeelUsers(IServiceLocator locator)
        {
            var d = locator.GetService<IBaseObjectService<User>>();
            var factory = locator.GetService<IUnitOfWorkFactory>();

            using (var uow = factory.CreateSystem())
            {
                InsertUsers(uow, d);
            }
        }

        private void InsertUsers(IUnitOfWork uow, IBaseObjectService<User> service)
        {

            string userName = "user";
            int cat = 2;

            for (int i = 0; i < 1000; i++)
            {
                var user = new User
                {
                    CategoryID = cat,
                    CustomStatus = 0,
                    IsActive = true,
                    Profile = new SimpleProfile
                    {
                        BirthDate = DateTime.Now,
                        FirstName = userName,
                        LastName = "Numer " + i,
                        Gender = Gender.Male
                    }
                };

                service.Create(uow, user);
                uow.SaveChanges();
            }
        }
    }
}
