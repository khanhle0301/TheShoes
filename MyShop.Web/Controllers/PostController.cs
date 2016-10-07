using AutoMapper;
using MyShop.Common;
using MyShop.Model.Models;
using MyShop.Service;
using MyShop.Web.Infrastructure.Core;
using MyShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class PostController : Controller
    {
        private IPostService _postService;
        private IPostCategoryService _postCategoryService;
        private ICommonService _commonService;

        public PostController(IPostService postService, IPostCategoryService postCategoryService, ICommonService commonService)
        {
            this._postService = postService;
            this._postCategoryService = postCategoryService;
            this._commonService = commonService;
        }

        public ActionResult Index(int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postService.GetAllPaging(page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        public ActionResult Category(string alias, int page = 1)
        {
            var category = _postCategoryService.GetByAlias(alias);
            ViewBag.Category = Mapper.Map<PostCategory, PostCategoryViewModel>(category);
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postService.GetAllByCategoryPaging(category.ID, page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        public ActionResult ListByTag(string tagId, int page = 1)
        {
            ViewBag.Tags = _commonService.GetById(tagId);
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postService.GetAllByTagPaging(tagId, page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        [ChildActionOnly]
        public ActionResult TopNewPost()
        {
            var model = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(_postService.GetNewPost(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostTag()
        {
            var model = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_commonService.GetPostTags());
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostCategory()
        {
            var model = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(_postCategoryService.GetPostCategory());
            return PartialView(model);
        }

        public ActionResult Detail(int id)
        {
            var post = _postService.GetById(id);
            var viewModel = Mapper.Map<Post, PostViewModel>(post);
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_postService.GetListTagByPostId(id));
            ViewBag.Category = Mapper.Map<PostCategory, PostCategoryViewModel>(_postCategoryService.GetById(post.CategoryID));
            var relatedPost = _postService.GetReatedPosts(id, 2);
            ViewBag.RelatedPosts = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(relatedPost);
            var relatedTakePost = _postService.GetReatedTakePosts(id, 2);
            ViewBag.RelatedTakePosts = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(relatedTakePost);
            _postService.IncreaseView(id);
            _postService.Save();
            return View(viewModel);
        }
    }
}