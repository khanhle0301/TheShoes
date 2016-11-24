using MyShop.Common;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using MyShop.Common.Exceptions;

namespace MyShop.Service
{
    public interface IProductService
    {
        bool SellProduct(int productId, int quantity);

        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetHomeHotSaleProduct(int top);

        IEnumerable<Product> GetHomeLastest(int top);

        IEnumerable<Product> GetHomeSale(int top);

        IEnumerable<Product> GetViewCount(int top);

        IEnumerable<Product> GetHomeHotProduct();

        IEnumerable<Product> GetReatedProducts(int id, int top);

        Product GetById(int id);

        Product GetAllById(int id);

        Product GetByAlias(string alias);

        IEnumerable<Tag> GetListTagByProductId(int id);

        IEnumerable<Size> GetListSizeByProductId(int id);

        IEnumerable<Color> GetListColorByProductId(int id);

        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductAllPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductOnSalePaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductHotPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductSaleHotPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductNewPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetListProductViewCountPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetAllByTagPaging(string tagid, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<Product> GetAllBySearch(string keyword, string filter, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type);

        IEnumerable<string> GetListProductByName(string name);

        IEnumerable<Product> GetProductColor(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductMaterial(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductHeel(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductHeight(IEnumerable<Product> Products, int id);

        IEnumerable<Product> GetProductType(IEnumerable<Product> Products, int id);

        void Save();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private ISizeRepository _sizeRepository;
        private IProductSizeRepository _productSizeRepository;
        private IColorRepository _colorRepository;
        private IProductColorRepository _productColorRepository;
        private IMaterialRepository _materialRepository;
        private IProductMaterialRepository _productMaterialRepository;
        private IProductCategoryRepository _productCategoryRepository;

        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository,
                                ITagRepository tagRepository, ISizeRepository sizeRepository,
                                IColorRepository colorRepository, IProductSizeRepository productSizeRepository,
                                IMaterialRepository materialRepository, IProductMaterialRepository productMaterialRepository,
                                IProductCategoryRepository productCategoryRepository,
        IProductColorRepository productColorRepository, IUnitOfWork unitOfWork)
        {
            this._productCategoryRepository = productCategoryRepository;
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
            this._colorRepository = colorRepository;
            this._productSizeRepository = productSizeRepository;
            this._productColorRepository = productColorRepository;
            this._sizeRepository = sizeRepository;
            this._materialRepository = materialRepository;
            this._productMaterialRepository = productMaterialRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool SellProduct(int productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if (product.Quantity < quantity)
                return false;
            product.Quantity -= quantity;
            return true;
        }

        public IEnumerable<Product> GetProductType(IEnumerable<Product> Products, int id)
        {
            return _productRepository.GetProductType(Products, id);
        }

        public IEnumerable<Product> GetProductColor(IEnumerable<Product> Products, int id)
        {
            return _productRepository.GetProductColor(Products, id);
        }

        public IEnumerable<Product> GetProductMaterial(IEnumerable<Product> Products, int id)
        {
            return _productRepository.GetProductMaterial(Products, id);
        }

        public IEnumerable<Product> GetProductHeel(IEnumerable<Product> Products, int id)
        {
            return _productRepository.GetProductHeel(Products, id);
        }

        public IEnumerable<Product> GetProductHeight(IEnumerable<Product> Products, int id)
        {
            return _productRepository.GetProductHeight(Products, id);
        }

        public Product Add(Product Product)
        {
            if (Product.OriginalPrice >= Product.Price)
                throw new NameDuplicatedException("Giá gốc không thể lớn hơn giá bán");
            else
               if (Product.PromotionPrice >= Product.Price)
                throw new NameDuplicatedException("Giá khuyến mãi không thể lớn hơn giá bán");
            else
              if (Product.OriginalPrice >= Product.PromotionPrice)
                throw new NameDuplicatedException("Giá khuyến mãi không thể nhỏ hơn giá gốc");
            var product = _productRepository.Add(Product);
            _unitOfWork.Commit();

            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }

            return product;
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll(new string[] { "ProductCategory" });
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Update(Product Product)
        {
            if (Product.OriginalPrice >= Product.Price)
                throw new NameDuplicatedException("Giá gốc không thể lớn hơn giá bán");
            else
               if (Product.PromotionPrice >= Product.Price)
                throw new NameDuplicatedException("Giá khuyến mãi không thể lớn hơn giá bán");
            else
               if (Product.OriginalPrice >= Product.PromotionPrice)
                throw new NameDuplicatedException("Giá khuyến mãi không thể nhỏ hơn giá gốc");
            _productRepository.Update(Product);

            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.DeleteMulti(x => x.ProductID == Product.ID);
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
        }

        public IEnumerable<Product> GetHomeLastest(int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.HomeFlag == true
                                                , new string[] { "ProductCategory" })
                                                .OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHomeHotSaleProduct(int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.HomeFlag == true
                                                && x.QuantitySold.HasValue, new string[] { "ProductCategory" })
                                                .OrderByDescending(x => x.QuantitySold).Take(top);
        }

        public IEnumerable<Product> GetHomeSale(int top)
        {
            return _productRepository.GetMulti(x => x.Status && x.HomeFlag == true
                                                && x.PromotionPrice.HasValue, new string[] { "ProductCategory" })
                                                .OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHomeHotProduct()
        {
            return _productRepository.GetMulti(x => x.Status && x.HomeFlag == true && x.HotFlag == true
                                                , new string[] { "ProductCategory" })
                                                .OrderByDescending(x => x.CreatedDate);
        }

        public Product GetByAlias(string alias)
        {
            return _productRepository.GetSingleByCondition(x => x.Alias == alias);
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public void IncreaseView(int id)
        {
            var post = _productRepository.GetSingleById(id);
            if (post.ViewCount.HasValue)
                post.ViewCount += 1;
            else
                post.ViewCount = 1;
        }

        public IEnumerable<Product> GetViewCount(int top)
        {
            return _productRepository.GetMulti(x => x.Status
                                                , new string[] { "ProductCategory" })
                                                .OrderByDescending(x => x.ViewCount).Take(top);
        }

        public Product GetAllById(int id)
        {
            return _productRepository.GetMulti(x => x.ID == id
                                              , new string[] { "ProductCategory" }).FirstOrDefault();
        }

        public IEnumerable<Size> GetListSizeByProductId(int id)
        {
            return _productSizeRepository.GetMulti(x => x.ProductID == id, new string[] { "Size" }).Select(y => y.Size);
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository.GetMulti(x => x.Status && x.ID != id && x.CategoryID == product.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Color> GetListColorByProductId(int id)
        {
            return _productColorRepository.GetMulti(x => x.ProductID == id, new string[] { "Color" }).Select(y => y.Color);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId, new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductAllPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status, new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductOnSalePaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.PromotionPrice.HasValue, new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductHotPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.HotFlag == true, new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductSaleHotPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.QuantitySold.HasValue, new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductNewPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status, new string[] { "ProductCategory" }).OrderByDescending(x => x.UpdatedDate).Take(12);

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetListProductViewCountPaging(string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status, new string[] { "ProductCategory" }).OrderByDescending(x => x.ViewCount).Take(12);

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<Product> GetAllByTagPaging(string tagid, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetAllByTag(tagid);

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(x => x.Status && x.Name.Contains(name)).Select(y => y.Name);
        }

        public IEnumerable<Product> GetAllBySearch(string keyword, string filter, string sort, string price, string provider, string color, string chatlieu, string heel, string height, string type)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.Name.Contains(keyword), new string[] { "ProductCategory" });

            IEnumerable<Product> resultType = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(type))
            {
                var typeArr = type.Split(',');
                foreach (var item in typeArr)
                {
                    resultType = resultType.Concat(this.GetProductType(query, int.Parse(item)));
                }
            }
            else
            {
                resultType = resultType.Concat(query);
            }

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(this.GetProductColor(resultType, int.Parse(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultType).Distinct();
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(this.GetProductMaterial(resultColor, int.Parse(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor).Distinct();
            }

            IEnumerable<Product> resultHeel = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(heel))
            {
                var heelArr = heel.Split(',');
                foreach (var item in heelArr)
                {
                    resultHeel = resultHeel.Concat(this.GetProductHeel(resultChatlieu, int.Parse(item)));
                }
            }
            else
            {
                resultHeel = resultHeel.Concat(resultChatlieu).Distinct();
            }

            IEnumerable<Product> resultHeight = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(height))
            {
                var heightArr = height.Split(',');
                foreach (var item in heightArr)
                {
                    resultHeight = resultHeight.Concat(this.GetProductHeight(resultHeel, int.Parse(item)));
                }
            }
            else
            {
                resultHeight = resultHeight.Concat(resultHeel).Distinct();
            }

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice < 100000 : x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 100000 && x.PromotionPrice <= 300000 : x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 300000 && x.PromotionPrice <= 500000 : x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice >= 500000 && x.PromotionPrice <= 1000000 : x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(resultHeight.Where(x => x.PromotionPrice.HasValue ? x.PromotionPrice > 1000000 : x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(resultHeight);
            }

            IEnumerable<Product> resultProvider = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(provider))
            {
                var providerArr = provider.Split(',');
                foreach (var item in providerArr)
                {
                    resultProvider = resultProvider.Concat(priceResult.Where(x => x.ProviderID == int.Parse(item.ToString())));
                }
            }
            else
            {
                resultProvider = resultProvider.Concat(priceResult);
            }

            var result = resultProvider.Distinct();
            switch (sort)
            {
                case "viewcount":
                    result = result.OrderByDescending(x => x.ViewCount);
                    break;
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price);
                    break;

                case "name_asc":
                    result = result.OrderBy(x => x.Name);
                    break;

                case "name_desc":
                    result = result.OrderByDescending(x => x.Name);
                    break;

                case "updated_asc":
                    result = result.OrderBy(x => x.UpdatedDate);
                    break;

                case "updated_desc":
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;

                case "sold_quantity":
                    result = result.OrderByDescending(x => x.QuantitySold);
                    break;

                default:
                    result = result.OrderByDescending(x => x.UpdatedDate);
                    break;
            }
            return result;
        }        

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}