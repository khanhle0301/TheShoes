using AutoMapper;
using MyShop.Model.Models;
using MyShop.Web.Models;

namespace MyShop.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Post, PostViewModel>();
            Mapper.CreateMap<PostCategory, PostCategoryViewModel>();
            Mapper.CreateMap<Tag, TagViewModel>();
            Mapper.CreateMap<ProductCategory, ProductCategoryViewModel>();
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ProductTag, ProductTagViewModel>();
            Mapper.CreateMap<Slide, SlideViewModel>();
            Mapper.CreateMap<Page, PageViewModel>();
            Mapper.CreateMap<Footer, FooterViewModel>();
            Mapper.CreateMap<ContactDetail, ContactDetailViewModel>();
            Mapper.CreateMap<Feedback, FeedbackViewModel>();
            Mapper.CreateMap<Order, OrderViewModel>();
            Mapper.CreateMap<OrderDetail, OrderDetailViewModel>();
            Mapper.CreateMap<Banner, BannerViewModel>();
            Mapper.CreateMap<Size, SizeViewModel>();
            Mapper.CreateMap<Provider, ProviderViewModel>();
            Mapper.CreateMap<Color, ColorViewModel>();
            Mapper.CreateMap<Material, MaterialViewModel>();

            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
            Mapper.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
        }
    }
}