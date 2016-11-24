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
    [RoutePrefix("api/banner")]
    [Authorize]
    public class BannerController : ApiControllerBase
    {
        #region Initialize
        private IBannerService _bannerService;

        public BannerController(IErrorService errorService, IBannerService bannerService)
            : base(errorService)
        {
            this._bannerService = bannerService;
        }

        #endregion

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewBanner")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _bannerService.GetById(id);

                var responseData = Mapper.Map<Banner, BannerViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewBanner")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _bannerService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(query);

                var paginationSet = new PaginationSet<BannerViewModel>()
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
        [Authorize(Roles = "AddBanner")]
        public HttpResponseMessage Create(HttpRequestMessage request, BannerViewModel bannerVm)
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
                    var newBanner = new Banner();
                    newBanner.UpdateBanner(bannerVm);
                    _bannerService.Add(newBanner);
                    _bannerService.Save();

                    var responseData = Mapper.Map<Banner, BannerViewModel>(newBanner);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "UpdateBanner")]
        public HttpResponseMessage Update(HttpRequestMessage request, BannerViewModel bannerVm)
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
                    var dbBanner = _bannerService.GetById(bannerVm.ID);
                    dbBanner.UpdateBanner(bannerVm);
                    _bannerService.Update(dbBanner);
                    _bannerService.Save();

                    var responseData = Mapper.Map<Banner, BannerViewModel>(dbBanner);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "DeleteBanner")]
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
                    var oldBanner = _bannerService.Delete(id);
                    _bannerService.Save();

                    var responseData = Mapper.Map<Banner, BannerViewModel>(oldBanner);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteBanner")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedBanners)
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
                    var listBanner = new JavaScriptSerializer().Deserialize<List<int>>(checkedBanners);
                    foreach (var item in listBanner)
                    {
                        _bannerService.Delete(item);
                    }

                    _bannerService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listBanner.Count);
                }

                return response;
            });
        }
    }
}