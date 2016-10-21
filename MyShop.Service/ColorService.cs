using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IColorService
    {
        Color Add(Color color);

        void Update(Color Color);

        Color Delete(int id);

        IEnumerable<Color> GetAll();

        IEnumerable<Color> GetAll(string keyword);

        Color GetById(int id);

        IEnumerable<Color> GetListColorByProductId(int id);

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

        public Color Add(Color color)
        {
            return _colorRepository.Add(color);
        }

        public Color Delete(int id)
        {
            return _colorRepository.Delete(id);
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

        public Color GetById(int id)
        {
            return _colorRepository.GetSingleByCondition(x => x.ID == id);
        }

        public IEnumerable<Color> GetListColorByProductId(int id)
        {
            return _colorRepository.GetListColorByProductId(id);
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