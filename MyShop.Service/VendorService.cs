using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IVendorService
    {
        Vendor Add(Vendor Vendor);

        void Update(Vendor Vendor);

        Vendor Delete(int id);

        IEnumerable<Vendor> GetAll();

        IEnumerable<Vendor> GetAll(string keyword);

        Vendor GetById(int id);

        void Save();
    }
   public class VendorService : IVendorService
    {
        private IVendorRepository _vendorRepository;
        private IUnitOfWork _unitOfWork;

        public VendorService(IVendorRepository vendorRepository, IUnitOfWork unitOfWork)
        {
            this._vendorRepository = vendorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Vendor Add(Vendor Vendor)
        {
            return _vendorRepository.Add(Vendor);
        }

        public Vendor Delete(int id)
        {
            return _vendorRepository.Delete(id);
        }

        public IEnumerable<Vendor> GetAll()
        {
            return _vendorRepository.GetAll();
        }

        public IEnumerable<Vendor> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _vendorRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _vendorRepository.GetAll();
        }


        public Vendor GetById(int id)
        {
            return _vendorRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Vendor Vendor)
        {
            _vendorRepository.Update(Vendor);
        }
    }
}
