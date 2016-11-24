using AutoMapper;
using MyShop.Common.Exceptions;
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
    [RoutePrefix("api/type")]
    [Authorize]
    public class TypeController : ApiControllerBase
    {
        #region Initialize

        private ITypeService _typeService;

        public TypeController(IErrorService errorService, ITypeService typeService)
            : base(errorService)
        {
            this._typeService = typeService;
        }

        #endregion Initialize

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewType")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _typeService.GetById(id);

                var responseData = Mapper.Map<Model.Models.Type, TypeViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }


        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewType")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _typeService.GetAll();
                IEnumerable<TypeViewModel> modelVm = Mapper.Map<IEnumerable<Model.Models.Type>, IEnumerable<TypeViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }


        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewType")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageType = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _typeService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageType).Take(pageType);

                var responseData = Mapper.Map<IEnumerable<Model.Models.Type>, IEnumerable<TypeViewModel>>(query);

                var paginationSet = new PaginationSet<TypeViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageType)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "AddType")]
        public HttpResponseMessage Create(HttpRequestMessage request, TypeViewModel typeVm)
        {
            if (ModelState.IsValid)
            {
                var newType = new Model.Models.Type();
                newType.UpdateType(typeVm);
                try
                {
                    _typeService.Add(newType);
                    _typeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, typeVm);
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
        [Authorize(Roles = "UpdateType")]
        public HttpResponseMessage Update(HttpRequestMessage request, TypeViewModel typeVm)
        {
            if (ModelState.IsValid)
            {
                var dbType = _typeService.GetById(typeVm.ID);
                dbType.UpdateType(typeVm);
                try
                {
                    _typeService.Update(dbType);
                    _typeService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, typeVm);
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
        [Authorize(Roles = "DeleteType")]
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
                    var oldType = _typeService.Delete(id);
                    _typeService.Save();

                    var responseData = Mapper.Map<Model.Models.Type, TypeViewModel>(oldType);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }


        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteType")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedTypes)
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
                    var listType = new JavaScriptSerializer().Deserialize<List<int>>(checkedTypes);
                    foreach (var item in listType)
                    {
                        _typeService.Delete(item);
                    }

                    _typeService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listType.Count);
                }

                return response;
            });
        }
    }
}