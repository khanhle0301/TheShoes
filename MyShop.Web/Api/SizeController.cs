using AutoMapper;
using MyShop.Common.Exceptions;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Infrastructure.Extensions;
using MyShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/size")]
    [Authorize]
    public class SizeController : ApiControllerBase
    {
        #region Initialize

        private ISizeService _sizeService;

        public SizeController(IErrorService errorService, ISizeService sizeService)
            : base(errorService)
        {
            this._sizeService = sizeService;
        }

        #endregion Initialize

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewSize")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _sizeService.GetById(id);

                var responseData = Mapper.Map<Size, SizeViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewSize")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _sizeService.GetAll();
                IEnumerable<SizeViewModel> modelVm = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewSize")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _sizeService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(query);

                var paginationSet = new PaginationSet<SizeViewModel>()
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
        [Authorize(Roles = "AddSize")]
        public HttpResponseMessage Create(HttpRequestMessage request, SizeViewModel sizeVm)
        {
            if (ModelState.IsValid)
            {
                var newSize = new Size();
                newSize.UpdateSize(sizeVm);
                try
                {
                    _sizeService.Add(newSize);
                    _sizeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, sizeVm);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "UpdateSize")]
        public HttpResponseMessage Update(HttpRequestMessage request, SizeViewModel sizeVm)
        {
            if (ModelState.IsValid)
            {
                var dbSize = _sizeService.GetById(sizeVm.ID);
                dbSize.UpdateSize(sizeVm);
                try
                {
                    _sizeService.Update(dbSize);
                    _sizeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, sizeVm);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "DeleteSize")]
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
                    var oldSize = _sizeService.Delete(id);
                    _sizeService.Save();

                    var responseData = Mapper.Map<Size, SizeViewModel>(oldSize);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteSize")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedSizes)
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
                    var listSize = new JavaScriptSerializer().Deserialize<List<int>>(checkedSizes);
                    foreach (var item in listSize)
                    {
                        _sizeService.Delete(item);
                    }

                    _sizeService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listSize.Count);
                }

                return response;
            });
        }
    }
}