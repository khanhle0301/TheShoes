using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;
using MyShop.Common.Exceptions;

namespace MyShop.Service
{
    public interface IMaterialService
    {
        Material Add(Material Material);

        void Update(Material Material);

        Material Delete(int id);

        IEnumerable<Material> GetAll();

        IEnumerable<Material> GetAll(string keyword);

        Material GetById(int id);

        IEnumerable<Material> GetListMaterialByProductId(int id);

        void Save();
    }
    public class MaterialService : IMaterialService
    {
        private IMaterialRepository _materialRepository;
        private IUnitOfWork _unitOfWork;

        public MaterialService(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            this._materialRepository = materialRepository;
            this._unitOfWork = unitOfWork;
        }

        public Material Add(Material material)
        {
            if (_materialRepository.CheckContains(x => x.Name == material.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _materialRepository.Add(material);
        }

        public Material Delete(int id)
        {
            return _materialRepository.Delete(id);
        }

        public IEnumerable<Material> GetAll()
        {
            return _materialRepository.GetAll();
        }

        public IEnumerable<Material> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _materialRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _materialRepository.GetAll();
        }


        public Material GetById(int id)
        {
            return _materialRepository.GetSingleById(id);
        }

        public IEnumerable<Material> GetListMaterialByProductId(int id)
        {
            return _materialRepository.GetListMaterialByProductId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Material material)
        {
            if (_materialRepository.CheckContains(x => x.Name == material.Name && x.ID != material.ID))
                throw new NameDuplicatedException("Tên không được trùng");
            _materialRepository.Update(material);
        }
    }
}