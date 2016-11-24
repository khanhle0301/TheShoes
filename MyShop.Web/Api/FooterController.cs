using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Models;
using MyShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;
using AutoMapper;
using System.Net;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/footer")]
    [Authorize]
    public class FooterController : ApiControllerBase
    {
        #region Initialize
        private IFooterService _footerService;

        public FooterController(IErrorService errorService, IFooterService footerService)
            : base(errorService)
        {
            this._footerService = footerService;
        }

        #endregion
       
        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Footer")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _footerService.GetById(id);

                var responseData = Mapper.Map<Footer, FooterViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Footer")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _footerService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Footer>, IEnumerable<FooterViewModel>>(query);

                var paginationSet = new PaginationSet<FooterViewModel>()
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
        [Authorize(Roles = "Footer")]
        public HttpResponseMessage Create(HttpRequestMessage request, FooterViewModel footerVm)
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
                    var newFooter = new Footer();
                    newFooter.UpdateFooter(footerVm);                    
                    _footerService.Add(newFooter);
                    _footerService.Save();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(newFooter);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Footer")]
        public HttpResponseMessage Update(HttpRequestMessage request, FooterViewModel footerVm)
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
                    var dbFooter = _footerService.GetById(footerVm.ID);

                    dbFooter.UpdateFooter(footerVm);                 
                    _footerService.Update(dbFooter);
                    _footerService.Save();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(dbFooter);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "Footer")]
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
                    var oldFooter = _footerService.Delete(id);
                    _footerService.Save();

                    var responseData = Mapper.Map<Footer, FooterViewModel>(oldFooter);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "Footer")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedFooters)
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
                    var listFooter = new JavaScriptSerializer().Deserialize<List<int>>(checkedFooters);
                    foreach (var item in listFooter)
                    {
                        _footerService.Delete(item);
                    }

                    _footerService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listFooter.Count);
                }

                return response;
            });
        }
    }
}