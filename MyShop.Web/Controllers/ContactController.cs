using AutoMapper;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Extensions;
using MyShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MyShop.Common;

namespace MyShop.Web.Controllers
{
    public class ContactController : Controller
    {
        IContactDetailService _contactDetailService;
        IFeedbackService _feedbackService;
        public ContactController(IContactDetailService contactDetailService, IFeedbackService feedbackService)
        {
            this._contactDetailService = contactDetailService;
            this._feedbackService = feedbackService;
        }
        // GET: Contact
        public ActionResult Index()
        {
            FeedbackViewModel viewModel = new FeedbackViewModel();
            viewModel.ContactDetail = GetDetail();
            return View(viewModel);
        }

        public JsonResult SendFeedback(string contactViewModel)
        {
            var contact = new JavaScriptSerializer().Deserialize<FeedbackViewModel>(contactViewModel);

            Feedback newFeedback = new Feedback();
            FeedbackViewModel newFeedbackVm = new FeedbackViewModel();
            newFeedbackVm.CreatedDate = DateTime.Now;
            newFeedbackVm.Phone = contact.Phone;
            newFeedbackVm.Name = contact.Name;
            newFeedbackVm.Email = contact.Email;
            newFeedbackVm.Message = contact.Message;
            newFeedbackVm.Status = contact.Status;

            newFeedback.UpdateFeedback(newFeedbackVm);
            _feedbackService.Add(newFeedback);
            _feedbackService.Save();

            string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/feedback.html"));
            content = content.Replace("{{Name}}", contact.Name);
            content = content.Replace("{{Email}}", contact.Email);
            content = content.Replace("{{Message}}", contact.Message);
            var adminEmail = ConfigHelper.GetByKey("AdminEmail");
            MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

            return Json(new
            {
                status = true
            });
        }

        private ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return contactViewModel;
        }
    }
}