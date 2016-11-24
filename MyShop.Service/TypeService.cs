using MyShop.Common.Exceptions;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface ITypeService
    {
        Type Add(Type Type);

        void Update(Type Type);

        Type Delete(int id);

        IEnumerable<Type> GetAll();

        IEnumerable<Type> GetAll(string keyword);

        Type GetById(int id);

        IEnumerable<Type> GetListTypeByProductId(int id);

        void Save();
    }
    public class TypeService : ITypeService
    {
        private ITypeRepository _typeRepository;
        private IUnitOfWork _unitOfWork;

        public TypeService(ITypeRepository typeRepository, IUnitOfWork unitOfWork)
        {
            this._typeRepository = typeRepository;
            this._unitOfWork = unitOfWork;
        }

        public Type Add(Type type)
        {
            if (_typeRepository.CheckContains(x => x.Name == type.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _typeRepository.Add(type);
        }

        public Type Delete(int id)
        {
            return _typeRepository.Delete(id);
        }

        public IEnumerable<Type> GetAll()
        {
            return _typeRepository.GetAll();
        }

        public IEnumerable<Type> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _typeRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _typeRepository.GetAll();
        }

        public Type GetById(int id)
        {
            return _typeRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Type> GetListTypeByProductId(int id)
        {
            return _typeRepository.GetListTypeByProductId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Type type)
        {
            if (_typeRepository.CheckContains(x => x.Name == type.Name && x.ID != type.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _typeRepository.Update(type);
        }
    }
}
