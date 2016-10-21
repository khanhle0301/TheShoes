using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IHeelService
    {
        Heel Add(Heel Heel);

        void Update(Heel Heel);

        Heel Delete(int id);

        IEnumerable<Heel> GetAll();

        IEnumerable<Heel> GetAll(string keyword);

        Heel GetById(int id);

        IEnumerable<Heel> GetListHeelByProductId(int id);

        void Save();
    }
    public class HeelService : IHeelService
    {
        private IHeelRepository _heelRepository;
        private IUnitOfWork _unitOfWork;

        public HeelService(IHeelRepository heelRepository, IUnitOfWork unitOfWork)
        {
            this._heelRepository = heelRepository;
            this._unitOfWork = unitOfWork;
        }

        public Heel Add(Heel heel)
        {
            return _heelRepository.Add(heel);
        }

        public Heel Delete(int id)
        {
            return _heelRepository.Delete(id);
        }

        public IEnumerable<Heel> GetAll()
        {
            return _heelRepository.GetAll();
        }

        public IEnumerable<Heel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _heelRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _heelRepository.GetAll();
        }

        public Heel GetById(int id)
        {
            return _heelRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Heel> GetListHeelByProductId(int id)
        {
            return _heelRepository.GetListHeelByProductId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Heel heel)
        {
            _heelRepository.Update(heel);
        }
    }
}

