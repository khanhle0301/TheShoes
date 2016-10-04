using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IColorService
    {
        void Update(Color Color);

        IEnumerable<Color> GetAll();

        IEnumerable<Color> GetAll(string keyword);

        Color GetById(string id);

        void Save();
    }
    public class ColorService : IColorService
    {
        private IColorRepository _colorRepository;
        private IUnitOfWork _unitOfWork;

        public ColorService(IColorRepository colorRepository, IUnitOfWork unitOfWork)
        {
            this._colorRepository = colorRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Color> GetAll()
        {
            return _colorRepository.GetAll();
        }

        public IEnumerable<Color> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _colorRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _colorRepository.GetAll();
        }


        public Color GetById(string id)
        {
            return _colorRepository.GetSingleByCondition(x => x.ID == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Color Color)
        {
            _colorRepository.Update(Color);
        }
    }
}