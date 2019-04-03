using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public class EmployeeService: BaseEmployeeService<Employee>, IEmployeeService
    {
        public EmployeeService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<Employee> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Employee> objectSaver)
        {
            var obj = objectSaver.Dest;
            obj.Title = $"{obj.LastName} {(obj.FirstName ?? "").Trim()} {(obj.MiddleName ?? "").Trim()}";

            return base.GetForSave(unitOfWork, objectSaver);
        }
    }
}
