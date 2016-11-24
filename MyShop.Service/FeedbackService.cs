using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System;

namespace MyShop.Service
{
    public interface IFeedbackService
    {
        Feedback Add(Feedback Feedback);

        void Update(Feedback Feedback);

        Feedback Delete(int id);

        IEnumerable<Feedback> GetAll();

        IEnumerable<Feedback> GetAll(string keyword);

        Feedback GetById(int id);

        void ChangeStatus(int id);

        void Save();
    }
    class FeedbackService : IFeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        private IUnitOfWork _unitOfWork;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            this._feedbackRepository = feedbackRepository;
            this._unitOfWork = unitOfWork;
        }

        public Feedback Add(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }

        public void ChangeStatus(int id)
        {
            var feedback = _feedbackRepository.GetSingleById(id);
            feedback.Status = !feedback.Status;
            _feedbackRepository.Update(feedback);
        }

        public Feedback Delete(int id)
        {
            return _feedbackRepository.Delete(id);
        }

        public IEnumerable<Feedback> GetAll()
        {
            return _feedbackRepository.GetAll();
        }

        public IEnumerable<Feedback> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _feedbackRepository.GetMulti(x => x.Name.Contains(keyword) || x.Email.Contains(keyword));
            else
                return _feedbackRepository.GetAll();
        }


        public Feedback GetById(int id)
        {
            return _feedbackRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Feedback Feedback)
        {
            _feedbackRepository.Update(Feedback);
        }
    }
}