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
using MyShop.Common.Exceptions;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/height")]
    [Authorize]
    public class HeightController : ApiControllerBase
    {
        #region Initialize
        private IHeightService _heightService;

        public HeightController(IErrorService errorService, IHeightService heightService)
            : base(errorService)
        {
            this._heightService = heightService;
        }

        #endregion

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewHeight")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _heightService.GetById(id);

                var responseData = Mapper.Map<Height, HeightViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewHeight")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _heightService.GetAll();
                IEnumerable<HeightViewModel> modelVm = Mapper.Map<IEnumerable<Height>, IEnumerable<HeightViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewHeight")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageHeight = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _heightService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageHeight).Take(pageHeight);

                var responseData = Mapper.Map<IEnumerable<Height>, IEnumerable<HeightViewModel>>(query);

                var paginationSet = new PaginationSet<HeightViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageHeight)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "AddHeight")]
        public HttpResponseMessage Create(HttpRequestMessage request, HeightViewModel heightVm)
        {
            if (ModelState.IsValid)
            {
                var newHeight = new Height();
                newHeight.UpdateHeight(heightVm);
                try
                {
                    _heightService.Add(newHeight);
                    _heightService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, heightVm);
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
        [Authorize(Roles = "UpdateHeight")]
        public HttpResponseMessage Update(HttpRequestMessage request, HeightViewModel heightVm)
        {
            if (ModelState.IsValid)
            {
                var dbHeight = _heightService.GetById(heightVm.ID);
                dbHeight.UpdateHeight(heightVm);
                try
                {
                    _heightService.Update(dbHeight);
                    _heightService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, heightVm);
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
        [Authorize(Roles = "DeleteHeight")]
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
                    var oldHeight = _heightService.Delete(id);
                    _heightService.Save();

                    var responseData = Mapper.Map<Height, HeightViewModel>(oldHeight);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteHeight")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedHeights)
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
                    var listHeight = new JavaScriptSerializer().Deserialize<List<int>>(checkedHeights);
                    foreach (var item in listHeight)
                    {
                        _heightService.Delete(item);
                    }

                    _heightService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listHeight.Count);
                }

                return response;
            });
        }
    }
}