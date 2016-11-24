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
    [RoutePrefix("api/material")]
    [Authorize]
    public class MaterialController : ApiControllerBase
    {
        #region Initialize

        private IMaterialService _materialService;

        public MaterialController(IErrorService errorService, IMaterialService materialService)
            : base(errorService)
        {
            this._materialService = materialService;
        }

        #endregion Initialize

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewMaterial")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _materialService.GetById(id);

                var responseData = Mapper.Map<Material, MaterialViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewMaterial")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _materialService.GetAll();
                IEnumerable<MaterialViewModel> modelVm = Mapper.Map<IEnumerable<Material>, IEnumerable<MaterialViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewMaterial")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _materialService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Material>, IEnumerable<MaterialViewModel>>(query);

                var paginationSet = new PaginationSet<MaterialViewModel>()
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
        [Authorize(Roles = "AddMaterial")]
        public HttpResponseMessage Create(HttpRequestMessage request, MaterialViewModel materialVm)
        {
            if (ModelState.IsValid)
            {
                var newMaterial = new Material();
                newMaterial.UpdateMaterial(materialVm);
                try
                {
                    _materialService.Add(newMaterial);
                    _materialService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, materialVm);
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
        [Authorize(Roles = "UpdateMaterial")]
        public HttpResponseMessage Update(HttpRequestMessage request, MaterialViewModel materialVm)
        {
            if (ModelState.IsValid)
            {
                var dbMaterial = _materialService.GetById(materialVm.ID);
                dbMaterial.UpdateMaterial(materialVm);
                try
                {
                    _materialService.Update(dbMaterial);
                    _materialService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, materialVm);
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
        [Authorize(Roles = "DeleteMaterial")]
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
                    var oldMaterial = _materialService.Delete(id);
                    _materialService.Save();

                    var responseData = Mapper.Map<Material, MaterialViewModel>(oldMaterial);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteMaterial")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedMaterials)
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
                    var listMaterial = new JavaScriptSerializer().Deserialize<List<int>>(checkedMaterials);
                    foreach (var item in listMaterial)
                    {
                        _materialService.Delete(item);
                    }

                    _materialService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listMaterial.Count);
                }

                return response;
            });
        }
    }
}