using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Services
{
    public class MnemonicExtensionService : IMnemonicExtensionService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IServiceLocator _serviceLocator;

        public MnemonicExtensionService(IUnitOfWorkFactory unitOfWorkFactory, IServiceLocator serviceLocator)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _serviceLocator = serviceLocator;
        }

        public void Accept(ConcurrentDictionary<string, ViewModelConfig> unionConfigs)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var mnemonicExGroup = GetMnemonicsEx(uofw);

                foreach (string mnemonic in mnemonicExGroup.System.Keys)
                {
                    ViewModelConfig config;

                    if (!unionConfigs.TryRemove(mnemonic, out config)) continue;

                    var copy = config.Copy();

                    foreach (var mnemonicEx in mnemonicExGroup.System[mnemonic])
                    {
                        copy.Accept(mnemonicEx);
                    }

                    unionConfigs.TryAdd(mnemonic, copy);
                }

                foreach (string key in mnemonicExGroup.Client.Keys)
                {
                    var arr = key.Split(':');

                    string baseMnemonic = arr[0];
                    string clientMnemonic = arr[1];

                    ViewModelConfig config;

                    if (!unionConfigs.TryGetValue(baseMnemonic, out config)) continue;

                    var copy = config.Copy();

                    copy.Mnemonic = clientMnemonic;

                    foreach (var mnemonicEx in mnemonicExGroup.Client[key])
                    {
                        copy.Accept(mnemonicEx);
                    }

                    unionConfigs.TryAdd(copy.Mnemonic, copy);
                }
            }
        }

        private class MnemonicExGroup
        {
            public IDictionary<string, ICollection<MnemonicEx>> System { get; set; }
            public IDictionary<string, ICollection<MnemonicEx>> Client { get; set; }
        }

        private IEnumerable<MnemonicEx> GetExtension(IUnitOfWork unitOfWork,
            Expression<Func<MnemonicEx, bool>> filter = null)
        {
            var mnemonicsEx = new List<MnemonicEx>();

            var implementations =
                typeof(MnemonicEx).Assembly.GetTypes()
                    .Where(
                        t =>
                            typeof(MnemonicEx).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass &&
                            typeof(MnemonicEx) != t);


            foreach (var type in implementations)
            {
                var typeservice = (typeof(IMnemonicExCrudService<>).MakeGenericType(type));

                var service =
                    _serviceLocator.GetService(typeservice) as IMnemonicExQueryService;

                if (service == null)
                    throw new Exception($"unregistered service -> {typeservice.FullName}");

                var q = service.GetAllMnemonicEx(unitOfWork);

                if (filter != null)
                    q = q.Where(filter);

                mnemonicsEx.AddRange(q.ToList());
            }

            return mnemonicsEx;
        }

        private MnemonicExGroup GetMnemonicsEx(IUnitOfWork unitOfWork)
        {
            var sysMnemonicIds =
                unitOfWork.GetRepository<SystemMnemonicItem>()
                    .All()
                    .Where(x => !x.Hidden)
                    .ToDictionary(x => x.ID, x => x.Mnemonic);

            var clientMnemonicIds =
                unitOfWork.GetRepository<ClientMnemonicItem>()
                    .All()
                    .Where(x => !x.Hidden)
                    .ToDictionary(x => x.ID, x => x.ParentMnemonic + ":" + x.Mnemonic);

            var mnemonicsEx = GetExtension(unitOfWork);

            var clientMnemonicEx =
                clientMnemonicIds.ToDictionary<KeyValuePair<int, string>, string, ICollection<MnemonicEx>>(
                    x => x.Value, x => new List<MnemonicEx>());

            var sysMnemonicEx =
                sysMnemonicIds.ToDictionary<KeyValuePair<int, string>, string, ICollection<MnemonicEx>>(x => x.Value,
                    x => new List<MnemonicEx>());

            foreach (var mnemonicEx in mnemonicsEx)
            {
                IDictionary<int, string> dicMnemonicIds;
                IDictionary<string, ICollection<MnemonicEx>> dicMnemonicEx;

                if (sysMnemonicIds.ContainsKey(mnemonicEx.MnemonicItemID))
                {
                    dicMnemonicIds = sysMnemonicIds;
                    dicMnemonicEx = sysMnemonicEx;
                }
                else if (clientMnemonicIds.ContainsKey(mnemonicEx.MnemonicItemID))
                {
                    dicMnemonicIds = clientMnemonicIds;
                    dicMnemonicEx = clientMnemonicEx;
                }
                else
                {
                    throw new NotImplementedException();
                }

                string mnemonic = dicMnemonicIds[mnemonicEx.MnemonicItemID];

                if (!dicMnemonicEx.ContainsKey(mnemonic))
                {
                    dicMnemonicEx[mnemonic] = new List<MnemonicEx>();
                }

                dicMnemonicEx[mnemonic].Add(mnemonicEx);
            }

            return new MnemonicExGroup()
            {
                System = sysMnemonicEx,
                Client = clientMnemonicEx
            };
        }

        public MnemonicItem GetMnemonicItem(IUnitOfWork unitOfWork, int mnemonicItemID)
        {
            var mnemonicItem =
                unitOfWork.GetRepository<MnemonicItem>()
                    .All()
                    .Where(x => x.ID == mnemonicItemID)
                    .Select(x => new {x.ID, x.ExtraID, x.Mnemonic}).Single();

            switch (mnemonicItem.ExtraID)
            {
                case nameof(SystemMnemonicItem):
                    return unitOfWork.GetRepository<SystemMnemonicItem>().All().Single(x => x.ID == mnemonicItemID);
                case nameof(ClientMnemonicItem):
                    return unitOfWork.GetRepository<ClientMnemonicItem>().All().Single(x => x.ID == mnemonicItemID);
                default:
                    throw new NotImplementedException();
            }
        }

        public void AcceptAllExtensions(IUnitOfWork unitOfWork, int mnemonicItemId, ViewModelConfig viewModelConfig)
        {
            var mnemonicsExs = GetExtension(unitOfWork, x => x.MnemonicItemID == mnemonicItemId);

            foreach (var mnemonicsEx in mnemonicsExs)
            {
                viewModelConfig.Accept(mnemonicsEx);
            }
        }
    }
}