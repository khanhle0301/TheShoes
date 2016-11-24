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
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        #region Initialize
        private IOrderService _orderService;

        public OrderController(IErrorService errorService, IOrderService orderService)
            : base(errorService)
        {
            this._orderService = orderService;
        }

        #endregion


        [Route("getbyorderid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage GetByOrderId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetByOrderId(id);

                var responseData = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }


        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetById(id);

                var responseData = Mapper.Map<Order, OrderViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);

                var paginationSet = new PaginationSet<OrderViewModel>()
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
        [Authorize(Roles = "Order")]
        public HttpResponseMessage Create(HttpRequestMessage request, OrderViewModel orderVm)
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
                    var newOrder = new Order();
                    newOrder.UpdateOrder(orderVm);
                    _orderService.Add(newOrder);
                    _orderService.Save();

                    var responseData = Mapper.Map<Order, OrderViewModel>(newOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage Update(HttpRequestMessage request, OrderViewModel orderVm)
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
                    var dbOrder = _orderService.GetById(orderVm.ID);
                    dbOrder.UpdateOrder(orderVm);
                    _orderService.Update(dbOrder);
                    _orderService.Save();

                    var responseData = Mapper.Map<Order, OrderViewModel>(dbOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "Order")]
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
                    var oldOrder = _orderService.Delete(id);
                    _orderService.Save();

                    var responseData = Mapper.Map<Order, OrderViewModel>(oldOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedOrders)
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
                    var listOrder = new JavaScriptSerializer().Deserialize<List<int>>(checkedOrders);
                    foreach (var item in listOrder)
                    {
                        _orderService.Delete(item);
                    }

                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listOrder.Count);
                }

                return response;
            });
        }

        [Route("changestatus")]
        [HttpDelete]
        [Authorize(Roles = "Order")]
        public HttpResponseMessage ChangeStatus(HttpRequestMessage request, int id)
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
                    _orderService.ChangeStatus(id);
                    _orderService.Save();
                    var oldOrder = _orderService.GetById(id);
                    var responseData = Mapper.Map<Order, OrderViewModel>(oldOrder);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
    }
}