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
    [RoutePrefix("api/provider")]
    [Authorize]
    public class ProviderController : ApiControllerBase
    {
        #region Initialize
        private IProviderService _providerService;

        public ProviderController(IErrorService errorService, IProviderService providerService)
            : base(errorService)
        {
            this._providerService = providerService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        [Authorize(Roles = "ViewProvider")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _providerService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewProvider")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _providerService.GetById(id);

                var responseData = Mapper.Map<Provider, ProviderViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewProvider")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _providerService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Provider>, IEnumerable<ProviderViewModel>>(query);

                var paginationSet = new PaginationSet<ProviderViewModel>()
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
        [Authorize(Roles = "AddProvider")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProviderViewModel providerVm)
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
                    var newProvider = new Provider();
                    newProvider.UpdateProvider(providerVm);                   
                    newProvider.CreatedDate = DateTime.Now;
                    newProvider.CreatedBy = User.Identity.Name;
                    newProvider.UpdatedDate = DateTime.Now;
                    newProvider.UpdatedBy = User.Identity.Name;
                    _providerService.Add(newProvider);
                    _providerService.Save();

                    var responseData = Mapper.Map<Provider, ProviderViewModel>(newProvider);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "UpdateProvider")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProviderViewModel providerVm)
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
                    var dbProvider = _providerService.GetById(providerVm.ID);
                    dbProvider.UpdateProvider(providerVm);
                    dbProvider.UpdatedDate = DateTime.Now;
                    dbProvider.UpdatedBy = User.Identity.Name;
                    _providerService.Update(dbProvider);
                    _providerService.Save();

                    var responseData = Mapper.Map<Provider, ProviderViewModel>(dbProvider);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "DeleteProvider")]
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
                    var oldProvider = _providerService.Delete(id);
                    _providerService.Save();

                    var responseData = Mapper.Map<Provider, ProviderViewModel>(oldProvider);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteProvider")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProviders)
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
                    var listProvider = new JavaScriptSerializer().Deserialize<List<int>>(checkedProviders);
                    foreach (var item in listProvider)
                    {
                        _providerService.Delete(item);
                    }

                    _providerService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProvider.Count);
                }

                return response;
            });
        }
    }
}