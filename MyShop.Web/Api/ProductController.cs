using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Infrastructure.Extensions;
using MyShop.Web.Models;
using MyShop.Common.Exceptions;

namespace MyShop.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private IProductService _productService;
        private IProductMaterialService _productMaterialService;
        private IMaterialService _materialService;
        private IProductColorService _productColorService;
        private IColorService _colorService;
        private IProductSizeService _productSizeService;
        private ISizeService _sizeService;
        private IProductHeightService _productHeightService;
        private IHeightService _heightService;
        private IProductTypeService _productTypeService;
        private ITypeService _typeService;
        private IProductHeelService _productHeelService;
        private IHeelService _heelService;

        public ProductController(IErrorService errorService, IProductService productService,
            IProductMaterialService productMaterialService, IMaterialService materialService,
            IProductColorService productColorService, IColorService colorService,
             IProductSizeService productSizeService, ISizeService sizeService,
             IProductHeightService productHeightService, IHeightService heightService,
             IProductTypeService productTypeService, ITypeService typeService,
             IProductHeelService productHeelService, IHeelService heelService)
            : base(errorService)
        {
            this._productHeelService = productHeelService;
            this._heelService = heelService;
            this._productTypeService = productTypeService;
            this._typeService = typeService;
            this._productHeightService = productHeightService;
            this._heightService = heightService;
            this._productSizeService = productSizeService;
            this._sizeService = sizeService;
            this._productService = productService;
            this._materialService = materialService;
            this._productColorService = productColorService;
            this._colorService = colorService;
            this._productMaterialService = productMaterialService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        [Authorize(Roles = "ViewProduct")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "ViewProduct")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                var responseData = Mapper.Map<Product, ProductViewModel>(model);

                var listMaterial = _materialService.GetListMaterialByProductId(id);
                responseData.Materials = Mapper.Map<IEnumerable<Material>, IEnumerable<MaterialViewModel>>(listMaterial);

                var listColor = _colorService.GetListColorByProductId(id);
                responseData.Colors = Mapper.Map<IEnumerable<Color>, IEnumerable<ColorViewModel>>(listColor);

                var listSize = _sizeService.GetListSizeByProductId(id);
                responseData.Sizes = Mapper.Map<IEnumerable<Size>, IEnumerable<SizeViewModel>>(listSize);

                var listHeight = _heightService.GetListHeightByProductId(id);
                responseData.Heights = Mapper.Map<IEnumerable<Height>, IEnumerable<HeightViewModel>>(listHeight);

                var listType = _typeService.GetListTypeByProductId(id);
                responseData.Types = Mapper.Map<IEnumerable<Model.Models.Type>, IEnumerable<TypeViewModel>>(listType);

                var listHeel = _heelService.GetListHeelByProductId(id);
                responseData.Heels = Mapper.Map<IEnumerable<Heel>, IEnumerable<HeelViewModel>>(listHeel);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "ViewProduct")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
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
        [Authorize(Roles = "AddProduct")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product();
                newProduct.UpdateProduct(productVm);
                newProduct.CreatedDate = DateTime.Now;
                newProduct.CreatedBy = User.Identity.Name;
                newProduct.UpdatedDate = DateTime.Now;
                newProduct.UpdatedBy = User.Identity.Name;
                try
                {
                    _productService.Add(newProduct);
                    _productService.Save();

                    //Insert ProductMaterial
                    foreach (var material in productVm.Materials)
                    {
                        _productMaterialService.Add(new ProductMaterial()
                        {
                            MaterialID = material.ID,
                            ProductID = newProduct.ID
                        });
                    }
                    _productMaterialService.Save();

                    //Insert ProductColor
                    foreach (var color in productVm.Colors)
                    {
                        _productColorService.Add(new ProductColor()
                        {
                            ColorID = color.ID,
                            ProductID = newProduct.ID
                        });
                    }
                    _productColorService.Save();

                    //Insert ProductSize
                    foreach (var size in productVm.Sizes)
                    {
                        _productSizeService.Add(new ProductSize()
                        {
                            SizeID = size.ID,
                            ProductID = newProduct.ID
                        });
                    }
                    _productSizeService.Save();

                    //Insert ProductHeight
                    foreach (var height in productVm.Heights)
                    {
                        _productHeightService.Add(new ProductHeight()
                        {
                            HeightId = height.ID,
                            ProductId = newProduct.ID
                        });
                    }
                    _productHeightService.Save();

                    //Insert ProductType                   
                    foreach (var type in productVm.Types)
                    {
                        _productTypeService.Add(new ProductType()
                        {
                            TypeId = type.ID,
                            ProductId = newProduct.ID
                        });
                    }
                    _productTypeService.Save();

                    //Insert ProductHeel                  
                    foreach (var heel in productVm.Heels)
                    {
                        _productHeelService.Add(new ProductHeel()
                        {
                            HeelId = heel.ID,
                            ProductId = newProduct.ID
                        });
                    }
                    _productHeelService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
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
        [Authorize(Roles = "UpdateProduct")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
        {
            if (ModelState.IsValid)
            {
                var dbProduct = _productService.GetById(productVm.ID);
                dbProduct.UpdateProduct(productVm);
                dbProduct.UpdatedDate = DateTime.Now;
                dbProduct.UpdatedBy = User.Identity.Name;
                try
                {
                    _productService.Update(dbProduct);
                    _productService.Save();

                    //Insert ProductMaterial
                    _productMaterialService.DeleteMulti(dbProduct.ID);
                    foreach (var material in productVm.Materials)
                    {
                        _productMaterialService.Add(new ProductMaterial()
                        {
                            MaterialID = material.ID,
                            ProductID = dbProduct.ID
                        });
                    }
                    _productMaterialService.Save();

                    //Insert ProductColor
                    _productColorService.DeleteMulti(dbProduct.ID);
                    foreach (var color in productVm.Colors)
                    {
                        _productColorService.Add(new ProductColor()
                        {
                            ColorID = color.ID,
                            ProductID = dbProduct.ID
                        });
                    }
                    _productColorService.Save();

                    //Insert ProductSize
                    _productSizeService.DeleteMulti(dbProduct.ID);
                    foreach (var size in productVm.Sizes)
                    {
                        _productSizeService.Add(new ProductSize()
                        {
                            SizeID = size.ID,
                            ProductID = dbProduct.ID
                        });
                    }
                    _productSizeService.Save();

                    //Insert ProductHeight
                    _productHeightService.DeleteMulti(dbProduct.ID);
                    foreach (var height in productVm.Heights)
                    {
                        _productHeightService.Add(new ProductHeight()
                        {
                            HeightId = height.ID,
                            ProductId = dbProduct.ID
                        });
                    }
                    _productHeightService.Save();

                    //Insert ProductType
                    _productTypeService.DeleteMulti(dbProduct.ID);
                    foreach (var type in productVm.Types)
                    {
                        _productTypeService.Add(new ProductType()
                        {
                            TypeId = type.ID,
                            ProductId = dbProduct.ID
                        });
                    }
                    _productTypeService.Save();

                    //Insert ProductHeel      
                    _productHeelService.DeleteMulti(dbProduct.ID);
                    foreach (var heel in productVm.Heels)
                    {
                        _productHeelService.Add(new ProductHeel()
                        {
                            HeelId = heel.ID,
                            ProductId = dbProduct.ID
                        });
                    }
                    _productHeelService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
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
        [Authorize(Roles = "DeleteProduct")]
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
                    var oldProductCategory = _productService.Delete(id);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "DeleteProduct")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
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
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listProductCategory)
                    {
                        _productService.Delete(item);
                    }

                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
    }
}
