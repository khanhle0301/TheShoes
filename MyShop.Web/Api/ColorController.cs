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
    [RoutePrefix("api/color")]
    [Authorize]
    public class ColorController : ApiControllerBase
    {
        #region Initialize

        private IColorService _colorService;

        public ColorController(IErrorService errorService, IColorService colorService)
            : base(errorService)
        {
            this._colorService = colorService;
        }

        #endregion Initialize

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewColor")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _colorService.GetById(id);

                var responseData = Mapper.Map<Color, ColorViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewColor")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _colorService.GetAll();
                IEnumerable<ColorViewModel> modelVm = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewColor")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _colorService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(query);

                var paginationSet = new PaginationSet<ColorViewModel>()
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
        [Authorize(Roles = "AddColor")]
        public HttpResponseMessage Create(HttpRequestMessage request, ColorViewModel colorVm)
        {
            if (ModelState.IsValid)
            {
                var newColor = new Color();
                newColor.UpdateColor(colorVm);
                try
                {                   
                    _colorService.Add(newColor);
                    _colorService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, colorVm);
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
        [Authorize(Roles = "UpdateColor")]
        public HttpResponseMessage Update(HttpRequestMessage request, ColorViewModel colorVm)
        {
            if (ModelState.IsValid)
            {
                var dbColor = _colorService.GetById(colorVm.ID);
                dbColor.UpdateColor(colorVm);
                try
                {                                    
                    _colorService.Update(dbColor);
                    _colorService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, colorVm);
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
        [Authorize(Roles = "DeleteColor")]
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
                    var oldColor = _colorService.Delete(id);
                    _colorService.Save();

                    var responseData = Mapper.Map<Color, ColorViewModel>(oldColor);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteColor")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedColors)
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
                    var listColor = new JavaScriptSerializer().Deserialize<List<int>>(checkedColors);
                    foreach (var item in listColor)
                    {
                        _colorService.Delete(item);
                    }

                    _colorService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listColor.Count);
                }

                return response;
            });
        }
    }
}