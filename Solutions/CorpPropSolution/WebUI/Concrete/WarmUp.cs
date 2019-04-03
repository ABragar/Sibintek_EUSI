using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Threading;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Task.Entities;
using CorpProp.Entities.Request;
using CorpProp.Services.Request;

namespace WebUI.Concrete
{
    /// <summary>
    /// Разогреватель инфраструктуры репозиториев
    /// </summary>
    internal class WarmUp: WarmUpActions
    {
        private readonly IServiceLocator _locator;

        public WarmUp(IServiceLocator locator)
        {
            _locator = locator;
        }

        [Flags]
        public enum WarmUpType
        {
            Find,
            Create,
            Update,
            Delete
        }

        public void WarmUpRepoByTypes(IEnumerable<Type> repoTypes, WarmUpType warmUpType)
        {
            using (var uow = _locator.GetService<IUnitOfWorkFactory>().Create())
            {

                var methodInfo = typeof(IUnitOfWork).GetMethod(nameof(uow.GetRepository), Array.Empty<Type>());
                if (methodInfo == null)
                    throw new NotSupportedException(
                                        $"Не удалось получить метод {nameof(uow.GetRepository)}");
                repoTypes.AsParallel().ForEach(
                                          repoType =>
                                          {
                                              var genericmethod = methodInfo.MakeGenericMethod(repoType);
                                              var repo = genericmethod.Invoke(uow, Array.Empty<object>());
                                              InvokeAction(repo, warmUpType);
                                          });
            }
        }
    }

    internal class WarmUpActions
    {
        public void InvokeAction(object repo, WarmUp.WarmUpType warmUpType)
        {
            var typeValues = Enum.GetValues(typeof(WarmUp.WarmUpType));

            foreach (WarmUp.WarmUpType value in typeValues)
            {
            if ((warmUpType & value) == value)
                switch (warmUpType)
                {
                    case WarmUp.WarmUpType.Find:
                        Find(repo);
                        break;
                    default:
                        throw new NotImplementedException($"{warmUpType}");
                }
            }
        }

        public void Find(object repo)
        {
            var memberInfo = repo.GetType().GetMethod(nameof(IRepository<BaseObject>.Find), new Type[] { typeof(object[]) });
            if (memberInfo != null)
            {
                memberInfo.Invoke(repo, new object[] { new object[] { 0 } });
            }
        }
    }
}