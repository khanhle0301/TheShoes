using MyShop.Common;
using MyShop.Data.Infrastructure;
using MyShop.Data.Repositories;
using MyShop.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using MyShop.Common.ViewModel;

namespace MyShop.Service
{
    public interface IPostService
    {
        Post Add(Post post);

        void Update(Post post);

        Post Delete(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetAll(string keyword);

        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Post GetById(int id);

        IEnumerable<PostViewModel> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);
      
        IEnumerable<Post> GetHot(int top);

        IEnumerable<Tag> GetListTagByPostId(int id);

        IEnumerable<Post> GetReatedPosts(int id, int top);

        IEnumerable<Post> GetReatedTakePosts(int id, int top);

        Post GetByAlias(string alias);

        void IncreaseView(int id);

        void Save();
    }

    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private IPostTagRepository _postTagRepository;

        private IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository, IPostTagRepository postTagRepository,
            ITagRepository _tagRepository, IUnitOfWork unitOfWork)
        {
            this._postRepository = postRepository;
            this._postTagRepository = postTagRepository;
            this._tagRepository = _tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Post Add(Post Post)
        {
            var post = _postRepository.Add(Post);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(Post.Tags))
            {
                string[] tags = post.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.PostTag;
                        _tagRepository.Add(tag);
                    }

                    PostTag postTag = new PostTag();
                    postTag.PostID = post.ID;
                    postTag.TagID = tagId;
                    _postTagRepository.Add(postTag);
                }
            }
            return post;
        }

        public Post Delete(int id)
        {
            return _postRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll(new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _postRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _postRepository.GetAll();
        }

        public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            var model = _postRepository.GetMulti(x => x.Status && x.CategoryID == categoryId
                       , new string[] { "PostCategory" });
            totalRow = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);           
        }

        public IEnumerable<PostViewModel> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {          
            return _postRepository.GetAllByTag(tag, page, pageSize, out totalRow);
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            var model = _postRepository.GetMulti(x => x.Status
                        , new string[] { "PostCategory" });
            totalRow = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Post GetById(int id)
        {
            return _postRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Post Post)
        {
            _postRepository.Update(Post);        
            if (!string.IsNullOrEmpty(Post.Tags))
            {
                _postTagRepository.DeleteMulti(x => x.PostID == Post.ID);
                string[] tags = Post.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.PostTag;
                        _tagRepository.Add(tag);
                    }
                    PostTag postTag = new PostTag();
                    postTag.PostID = Post.ID;
                    postTag.TagID = tagId;
                    _postTagRepository.Add(postTag);
                }
            }
        }

        public IEnumerable<Post> GetHot(int top)
        {
            return _postRepository.GetMulti(x => x.Status && x.HotFlag == true, new string[] { "PostCategory" }).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Tag> GetListTagByPostId(int id)
        {
            return _postTagRepository.GetMulti(x => x.PostID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public Post GetByAlias(string alias)
        {
            return _postRepository.GetSingleByCondition(x => x.Alias == alias);
        }
      
        public void IncreaseView(int id)
        {
            var post = _postRepository.GetSingleById(id);
            if (post.ViewCount.HasValue)
                post.ViewCount += 1;
            else
                post.ViewCount = 1;
        }

        public IEnumerable<Post> GetReatedPosts(int id, int top)
        {
            var post = _postRepository.GetSingleById(id);
            return _postRepository.GetMulti(x => x.Status && x.ID != id && x.CategoryID == post.CategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Post> GetReatedTakePosts(int id, int top)
        {
            var post = _postRepository.GetSingleById(id);
            return _postRepository.GetMulti(x => x.Status && x.ID != id && x.CategoryID == post.CategoryID).OrderByDescending(x => x.CreatedDate).Skip(2).Take(top);
        }
    }
}