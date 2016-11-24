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
    [RoutePrefix("api/heel")]
    [Authorize]
    public class HeelController : ApiControllerBase
    {
        #region Initialize

        private IHeelService _heelService;

        public HeelController(IErrorService errorService, IHeelService heelService)
            : base(errorService)
        {
            this._heelService = heelService;
        }

        #endregion Initialize

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewHeel")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _heelService.GetById(id);

                var responseData = Mapper.Map<Heel, HeelViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        [Authorize(Roles = "ViewHeel")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _heelService.GetAll();
                IEnumerable<HeelViewModel> modelVm = Mapper.Map<IEnumerable<Heel>, IEnumerable<HeelViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewHeel")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageHeel = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _heelService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageHeel).Take(pageHeel);

                var responseData = Mapper.Map<IEnumerable<Heel>, IEnumerable<HeelViewModel>>(query);

                var paginationSet = new PaginationSet<HeelViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageHeel)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "AddHeel")]
        public HttpResponseMessage Create(HttpRequestMessage request, HeelViewModel heelVm)
        {
            if (ModelState.IsValid)
            {
                var newHeel = new Heel();
                newHeel.UpdateHeel(heelVm);
                try
                {
                    _heelService.Add(newHeel);
                    _heelService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, heelVm);
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
        [Authorize(Roles = "UpdateHeel")]
        public HttpResponseMessage Update(HttpRequestMessage request, HeelViewModel heelVm)
        {
            if (ModelState.IsValid)
            {
                var dbHeel = _heelService.GetById(heelVm.ID);
                dbHeel.UpdateHeel(heelVm);
                try
                {
                    _heelService.Update(dbHeel);
                    _heelService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, heelVm);
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
        [Authorize(Roles = "DeleteHeel")]
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
                    var oldHeel = _heelService.Delete(id);
                    _heelService.Save();

                    var responseData = Mapper.Map<Heel, HeelViewModel>(oldHeel);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteHeel")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedHeels)
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
                    var listHeel = new JavaScriptSerializer().Deserialize<List<int>>(checkedHeels);
                    foreach (var item in listHeel)
                    {
                        _heelService.Delete(item);
                    }

                    _heelService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listHeel.Count);
                }

                return response;
            });
        }
    }
}
