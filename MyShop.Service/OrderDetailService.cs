using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IOrderDetailService
    {
        OrderDetail Add(OrderDetail OrderDetail);

        void Update(OrderDetail OrderDetail);

        OrderDetail Delete(int id);

        IEnumerable<OrderDetail> GetAll();

        IEnumerable<OrderDetail> GetAll(string keyword);

        OrderDetail GetById(int id);

        void Save();
    }
    public class OrderDetailService : IOrderDetailService
    {
        private IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public OrderDetail Add(OrderDetail OrderDetail)
        {
            return _orderDetailRepository.Add(OrderDetail);
        }

        public OrderDetail Delete(int id)
        {
            return _orderDetailRepository.Delete(id);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetailRepository.GetAll();
        }

        public IEnumerable<OrderDetail> GetAll(string keyword)
        {
            //if (!string.IsNullOrEmpty(keyword))
            //    return _orderDetailRepository.GetMulti(x => x.CustomerName.Contains(keyword));
            //else
            return _orderDetailRepository.GetAll();
        }

        public OrderDetail GetById(int id)
        {
            return _orderDetailRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(OrderDetail OrderDetail)
        {
            _orderDetailRepository.Update(OrderDetail);
        }
    }
}