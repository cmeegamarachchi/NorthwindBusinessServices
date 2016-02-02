using System.Collections.Generic;
using System.Linq;
using NorthwindDataAccess.Core;
using NorthwindDataAccess.Model;

namespace NorthwindBusinessServices.Suppliers
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IUnitOfWork _uow;

        public SuppliersService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ICollection<SupplierDescription> GetSupplierDescriptions()
        {
            return _uow.GetRepository<Supplier>().GetAll().Select(s => new SupplierDescription
            {
                Id = s.SupplierID,
                Name = s.CompanyName
            }).ToList();
        }
    }
}