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
using MyShop.Common.Exceptions;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/productcategory")]
    [Authorize]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        [Authorize(Roles = "ViewProductCategory")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewProductCategory")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetById(id);

                var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewProductCategory")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
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
        [Authorize(Roles = "AddProductCategory")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            if (ModelState.IsValid)
            {
                var newProductCategory = new ProductCategory();
                newProductCategory.UpdateProductCategory(productCategoryVm);
                newProductCategory.CreatedDate = DateTime.Now;
                newProductCategory.CreatedBy = User.Identity.Name;
                newProductCategory.UpdatedDate = DateTime.Now;
                newProductCategory.UpdatedBy = User.Identity.Name;
                try
                {
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    return request.CreateResponse(HttpStatusCode.OK, responseData);
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
        [Authorize(Roles = "UpdateProductCategory")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            if (ModelState.IsValid)
            {
                var dbProductCategory = _productCategoryService.GetById(productCategoryVm.ID);
                dbProductCategory.UpdateProductCategory(productCategoryVm);
                dbProductCategory.UpdatedDate = DateTime.Now;
                dbProductCategory.UpdatedBy = User.Identity.Name;
                try
                {
                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                    return request.CreateResponse(HttpStatusCode.OK, responseData);
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
        [Authorize(Roles = "DeleteProductCategory")]
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
                    var oldProductCategory = _productCategoryService.Delete(id);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteProductCategory")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProductCategories)
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
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach (var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                    }

                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
    }
}