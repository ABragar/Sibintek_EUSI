using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.UI;
using Common.Data.Entities.Test;

namespace Common.Data.Service.Concrete
{
    public class TestService : IQueryService<TestUnionEntry>
    {
        public IQueryable<TestUnionEntry> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            var q =
                unitOfWork.GetRepository<User>()
                    .All()
                    .Select(
                        x =>
                            new TestUnionEntry
                            {
                                Description = x.FullName,
                                ID = x.ID,
                                Name = default(string),
                            });


            q =
                unitOfWork.GetRepository<UiEnumValue>()
                    .All()
                    .Select(
                        x =>
                            new TestUnionEntry
                            {
                                Description = x.Title,
                                ID = x.ID,
                                Name = x.Value,
                            })
                    .Concat(q);


            return q.OrderBy(x => x.ID);
        }
    }
}