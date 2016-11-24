using MyShop.Common.Exceptions;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
namespace MyShop.Service
{
    public interface IHeightService
    {
        Height Add(Height Height);

        void Update(Height Height);

        Height Delete(int id);

        IEnumerable<Height> GetAll();

        IEnumerable<Height> GetAll(string keyword);

        Height GetById(int id);

        IEnumerable<Height> GetListHeightByProductId(int id);

        void Save();
    }
    public class HeightService : IHeightService
    {
        private IHeightRepository _heightRepository;
        private IUnitOfWork _unitOfWork;

        public HeightService(IHeightRepository heightRepository, IUnitOfWork unitOfWork)
        {
            this._heightRepository = heightRepository;
            this._unitOfWork = unitOfWork;
        }

        public Height Add(Height height)
        {
            if (_heightRepository.CheckContains(x => x.Name == height.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _heightRepository.Add(height);
        }

        public Height Delete(int id)
        {
            return _heightRepository.Delete(id);
        }

        public IEnumerable<Height> GetAll()
        {
            return _heightRepository.GetAll();
        }

        public IEnumerable<Height> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _heightRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _heightRepository.GetAll();
        }

        public Height GetById(int id)
        {
            return _heightRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Height> GetListHeightByProductId(int id)
        {
            return _heightRepository.GetListHeightByProductId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Height height)
        {
            if (_heightRepository.CheckContains(x => x.Name == height.Name && x.ID != height.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _heightRepository.Update(height);
        }
    }
}