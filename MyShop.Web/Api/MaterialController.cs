using System.Web.Http;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Models;
using MyShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;
using System.Net.Http;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System;

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

        #endregion

        [Route("getbyid/{id:int}")]
        [HttpGet]
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
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, MaterialViewModel materialVm)
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
                    var newMaterial = new Material();
                    newMaterial.UpdateMaterial(materialVm);
                    _materialService.Add(newMaterial);
                    _materialService.Save();

                    var responseData = Mapper.Map<Material, MaterialViewModel>(newMaterial);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, MaterialViewModel materialVm)
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
                    var dbMaterial = _materialService.GetById(materialVm.ID);
                    dbMaterial.UpdateMaterial(materialVm);
                    _materialService.Update(dbMaterial);
                    _materialService.Save();

                    var responseData = Mapper.Map<Material, MaterialViewModel>(dbMaterial);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
