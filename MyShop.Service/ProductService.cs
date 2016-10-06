using MyShop.Common;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MyShop.Service
{
    public interface IProductService
    {
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

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, string price, out int totalRow);

        Product GetById(int id);

        Product GetAllById(int id);

        Product GetByAlias(string alias);

        IEnumerable<Tag> GetListTagByProductId(int id);

        IEnumerable<Size> GetListSizeByProductId(int id);

        IEnumerable<Color> GetListColorByProductId(int id);

        void IncreaseView(int id);


        IEnumerable<Product> Demo(int categoryId, string sort, string price, string provider, string color,string chatlieu);


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

        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository,
                                ITagRepository tagRepository, ISizeRepository sizeRepository,
                                IColorRepository colorRepository, IProductSizeRepository productSizeRepository,
                                IMaterialRepository materialRepository, IProductMaterialRepository productMaterialRepository,
        IProductColorRepository productColorRepository, IUnitOfWork unitOfWork)
        {
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

        public Product Add(Product Product)
        {
            var product = _productRepository.Add(Product);
            _unitOfWork.Commit();

            if (!string.IsNullOrEmpty(Product.Materials))
            {
                string[] materials = Product.Materials.Split(',');
                for (var i = 0; i < materials.Length; i++)
                {
                    var materialId = StringHelper.ToUnsignString(materials[i]);
                    if (_materialRepository.Count(x => x.ID == materialId) == 0)
                    {
                        Material material = new Material();
                        material.ID = materialId;
                        material.Name = materials[i];
                        _materialRepository.Add(material);
                    }

                    ProductMaterial productMaterial = new ProductMaterial();
                    productMaterial.ProductID = Product.ID;
                    productMaterial.MaterialID = materialId;
                    _productMaterialRepository.Add(productMaterial);
                }
            }

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

            if (!string.IsNullOrEmpty(Product.Sizes))
            {
                string[] sizes = Product.Sizes.Split(',');
                for (var i = 0; i < sizes.Length; i++)
                {
                    var sizeId = StringHelper.ToUnsignString(sizes[i]);
                    if (_sizeRepository.Count(x => x.ID == sizeId) == 0)
                    {
                        Size size = new Size();
                        size.ID = sizeId;
                        size.Name = sizes[i];
                        _sizeRepository.Add(size);
                    }

                    ProductSize productSize = new ProductSize();
                    productSize.ProductID = Product.ID;
                    productSize.SizeID = sizeId;
                    _productSizeRepository.Add(productSize);
                }
            }

            if (!string.IsNullOrEmpty(Product.Colors))
            {
                string[] colors = Product.Colors.Split(',');
                for (var i = 0; i < colors.Length; i++)
                {
                    var colorId = StringHelper.ToUnsignString(colors[i]);
                    if (_colorRepository.Count(x => x.ID == colorId) == 0)
                    {
                        Color color = new Color();
                        color.ID = colorId;
                        color.Name = colors[i];
                        color.Background = null;
                        _colorRepository.Add(color);
                    }

                    ProductColor productColor = new ProductColor();
                    productColor.ProductID = Product.ID;
                    productColor.ColorID = colorId;
                    _productColorRepository.Add(productColor);
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

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product Product)
        {
            _productRepository.Update(Product);

            if (!string.IsNullOrEmpty(Product.Materials))
            {
                string[] materials = Product.Materials.Split(',');
                for (var i = 0; i < materials.Length; i++)
                {
                    var materialId = StringHelper.ToUnsignString(materials[i]);
                    if (_materialRepository.Count(x => x.ID == materialId) == 0)
                    {
                        Material material = new Material();
                        material.ID = materialId;
                        material.Name = materials[i];
                        _materialRepository.Add(material);
                    }
                    _productMaterialRepository.DeleteMulti(x => x.ProductID == Product.ID);
                    ProductMaterial productMaterial = new ProductMaterial();
                    productMaterial.ProductID = Product.ID;
                    productMaterial.MaterialID = materialId;
                    _productMaterialRepository.Add(productMaterial);
                }
            }

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

            if (!string.IsNullOrEmpty(Product.Sizes))
            {
                string[] sizes = Product.Sizes.Split(',');
                for (var i = 0; i < sizes.Length; i++)
                {
                    var sizeId = StringHelper.ToUnsignString(sizes[i]);
                    if (_sizeRepository.Count(x => x.ID == sizeId) == 0)
                    {
                        Size size = new Size();
                        size.ID = sizeId;
                        size.Name = sizes[i];
                        _sizeRepository.Add(size);
                    }
                    _productSizeRepository.DeleteMulti(x => x.ProductID == Product.ID);
                    ProductSize productSize = new ProductSize();
                    productSize.ProductID = Product.ID;
                    productSize.SizeID = sizeId;
                    _productSizeRepository.Add(productSize);
                }
            }

            if (!string.IsNullOrEmpty(Product.Colors))
            {
                string[] colors = Product.Colors.Split(',');
                for (var i = 0; i < colors.Length; i++)
                {
                    var colorId = StringHelper.ToUnsignString(colors[i]);
                    if (_colorRepository.Count(x => x.ID == colorId) == 0)
                    {
                        Color color = new Color();
                        color.ID = colorId;
                        color.Name = colors[i];
                        color.Background = null;
                        _colorRepository.Add(color);
                    }
                    _productColorRepository.DeleteMulti(x => x.ProductID == Product.ID);
                    ProductColor productColor = new ProductColor();
                    productColor.ProductID = Product.ID;
                    productColor.ColorID = colorId;
                    _productColorRepository.Add(productColor);
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

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, string price, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);
            IEnumerable<Product> result = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        result = result.Concat(query.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        result = result.Concat(query.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        result = result.Concat(query.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        result = result.Concat(query.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        result = result.Concat(query.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                result = result.Concat(query); ;
            }

            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
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

            totalRow = result.Count();

            return result.Skip((page - 1) * pageSize).Take(pageSize);
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

        public IEnumerable<Product> Demo(int categoryId, string sort, string price, string provider, string color,string chatlieu)
        {
            var query = _productRepository.GetMulti(x => x.Status && x.CategoryID == categoryId, new string[] { "ProductCategory" });

            IEnumerable<Product> priceResult = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(price))
            {
                var priceArr = price.Split(',');
                for (int i = 0; i < priceArr.Length; i++)
                {
                    if (priceArr[i] == "-100")
                        priceResult = priceResult.Concat(query.Where(x => x.Price < 100000));
                    else if (priceArr[i] == "100-300")
                        priceResult = priceResult.Concat(query.Where(x => x.Price >= 100000 && x.Price <= 300000));
                    else if (priceArr[i] == "300-500")
                        priceResult = priceResult.Concat(query.Where(x => x.Price >= 300000 && x.Price <= 500000));
                    else if (priceArr[i] == "500-1000")
                        priceResult = priceResult.Concat(query.Where(x => x.Price >= 500000 && x.Price <= 1000000));
                    else if (priceArr[i] == "1000")
                        priceResult = priceResult.Concat(query.Where(x => x.Price > 1000000));
                }
            }
            else
            {
                priceResult = priceResult.Concat(query);
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

            IEnumerable<Product> resultColor = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(color))
            {
                var colorArr = color.Split(',');
                foreach (var item in colorArr)
                {
                    resultColor = resultColor.Concat(resultProvider.Where(x => x.Colors != null && x.Colors.Contains(item)));
                }
            }
            else
            {
                resultColor = resultColor.Concat(resultProvider);
            }

            IEnumerable<Product> resultChatlieu = Enumerable.Empty<Product>();

            if (!string.IsNullOrEmpty(chatlieu))
            {
                var chatlieuArr = chatlieu.Split(',');
                foreach (var item in chatlieuArr)
                {
                    resultChatlieu = resultChatlieu.Concat(resultColor.Where(x => x.Materials != null && x.Materials.Contains(item)));
                }
            }
            else
            {
                resultChatlieu = resultChatlieu.Concat(resultColor);
            }

            var result = resultChatlieu.Distinct();
            switch (sort)
            {
                case "manual":
                    result = result.OrderByDescending(x => x.HotFlag);
                    break;

                case "price_asc":
                    result = result.OrderBy(x => x.Price);
                    break;

                case "price_desc":
                    result = result.OrderByDescending(x => x.Price);
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
    }
}