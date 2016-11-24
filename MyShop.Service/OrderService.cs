using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System;
using System.Collections.Generic;

namespace MyShop.Service
{
    public interface IOrderService
    {
        Order Add(Order Order);

        void Update(Order Order);

        Order Delete(int id);

        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetAll(string keyword);

        Order GetById(int id);

        Order Create(ref Order order, List<OrderDetail> orderDetails);

        IEnumerable<OrderDetail> GetByOrderId(int id);

        void ChangeStatus(int id);

        void UpdateStatus(int orderId);

        void Save();
    }

    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IUnitOfWork _unitOfWork;
        IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork,
             IOrderDetailRepository orderDetailRepository)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._orderRepository = orderRepository;
            this._unitOfWork = unitOfWork;
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void ChangeStatus(int id)
        {
            var feedback = _orderRepository.GetSingleById(id);
            feedback.Status = !feedback.Status;
            _orderRepository.Update(feedback);
        }

        public Order Add(Order Order)
        {
            return _orderRepository.Add(Order);
        }

        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Order Delete(int id)
        {
            return _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword) || x.CustomerEmail.Contains(keyword));
            else
                return _orderRepository.GetAll();
        }


        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Order Order)
        {
            _orderRepository.Update(Order);
        }

        public IEnumerable<OrderDetail> GetByOrderId(int id)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID == id, new string[] { "Product" });
        }
    }
}