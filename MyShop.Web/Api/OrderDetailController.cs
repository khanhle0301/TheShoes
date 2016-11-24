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
    [RoutePrefix("api/orderdetail")]
    [Authorize]
    public class OrderDetailController : ApiControllerBase
    {
        #region Initialize
        private IOrderDetailService _orderDetailService;

        public OrderDetailController(IErrorService errorService, IOrderDetailService orderDetailService)
            : base(errorService)
        {
            this._orderDetailService = orderDetailService;
        }

        #endregion

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "OrderDetail")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderDetailService.GetById(id);

                var responseData = Mapper.Map<OrderDetail, OrderDetailViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "OrderDetail")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderDetailService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.OrderID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(query);

                var paginationSet = new PaginationSet<OrderDetailViewModel>()
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
        [Authorize(Roles = "OrderDetail")]
        public HttpResponseMessage Create(HttpRequestMessage request, OrderDetailViewModel orderDetailVm)
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
                    var newOrderDetail = new OrderDetail();
                    newOrderDetail.UpdateOrderDetail(orderDetailVm);
                    _orderDetailService.Add(newOrderDetail);
                    _orderDetailService.Save();

                    var responseData = Mapper.Map<OrderDetail, OrderDetailViewModel>(newOrderDetail);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "OrderDetail")]
        public HttpResponseMessage Update(HttpRequestMessage request, OrderDetailViewModel orderDetailVm)
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
                    var dbOrderDetail = _orderDetailService.GetById(orderDetailVm.OrderID);
                    dbOrderDetail.UpdateOrderDetail(orderDetailVm);
                    _orderDetailService.Update(dbOrderDetail);
                    _orderDetailService.Save();

                    var responseData = Mapper.Map<OrderDetail, OrderDetailViewModel>(dbOrderDetail);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "OrderDetail")]
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
                    var oldOrderDetail = _orderDetailService.Delete(id);
                    _orderDetailService.Save();

                    var responseData = Mapper.Map<OrderDetail, OrderDetailViewModel>(oldOrderDetail);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "OrderDetail")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedOrderDetails)
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
                    var listOrderDetail = new JavaScriptSerializer().Deserialize<List<int>>(checkedOrderDetails);
                    foreach (var item in listOrderDetail)
                    {
                        _orderDetailService.Delete(item);
                    }

                    _orderDetailService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listOrderDetail.Count);
                }

                return response;
            });
        }
    }
}