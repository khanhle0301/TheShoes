using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Models;
using MyShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/feedback")]
    [Authorize]
    public class FeedbackController : ApiControllerBase
    {
        #region Initialize
        private IFeedbackService _feedbackService;

        public FeedbackController(IErrorService errorService, IFeedbackService feedbackService)
            : base(errorService)
        {
            this._feedbackService = feedbackService;
        }

        #endregion

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _feedbackService.GetById(id);

                var responseData = Mapper.Map<Feedback, FeedbackViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _feedbackService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(query);

                var paginationSet = new PaginationSet<FeedbackViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage Create(HttpRequestMessage request, FeedbackViewModel slideVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newFeedback = new Feedback();
                    newFeedback.UpdateFeedback(slideVm);
                    _feedbackService.Add(newFeedback);
                    _feedbackService.Save();

                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(newFeedback);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage Update(HttpRequestMessage request, FeedbackViewModel slideVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbFeedback = _feedbackService.GetById(slideVm.ID);
                    dbFeedback.UpdateFeedback(slideVm);
                    _feedbackService.Update(dbFeedback);
                    _feedbackService.Save();

                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(dbFeedback);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldFeedback = _feedbackService.Delete(id);
                    _feedbackService.Save();

                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(oldFeedback);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedFeedbacks)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listFeedback = new JavaScriptSerializer().Deserialize<List<int>>(checkedFeedbacks);
                    foreach (var item in listFeedback)
                    {
                        _feedbackService.Delete(item);
                    }

                    _feedbackService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listFeedback.Count);
                }

                return response;
            });
        }

        [Route("changestatus")]
        [HttpDelete]
        [Authorize(Roles = "Feedback")]
        public HttpResponseMessage ChangeStatus(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _feedbackService.ChangeStatus(id);
                    _feedbackService.Save();
                    var oldFeedback = _feedbackService.GetById(id);
                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(oldFeedback);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
    }
}