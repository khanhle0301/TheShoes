using System;
using System.Collections.Generic;
using TheShoes.Data.Infrastructure;
using TheShoes.Data.Repositories;
using TheShoes.Model.Models;
using System.Linq;
using TheShoes.Common;

namespace TheShoes.Service
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

        IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow);

        void Save();

        IEnumerable<Tag> GetListTagByPostId(int id);

        Tag GetTag(string tagId);

        IEnumerable<Post> GetListPostByTag(string tagId, int page, int pagesize, out int totalRow);

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

        public IEnumerable<Post> GetListPostByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var model = _postRepository.GetListPostByTag(tagId, page, pageSize, out totalRow);
            return model;
        }

        public Tag GetTag(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public IEnumerable<Tag> GetListTagByPostId(int id)
        {
            return _postTagRepository.GetMulti(x => x.PostID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public Post Add(Post Post)
        {
            var post = _postRepository.Add(Post);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(Post.Tags))
            {
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

                    PostTag PostTag = new PostTag();
                    PostTag.PostID = Post.ID;
                    PostTag.TagID = tagId;
                    _postTagRepository.Add(PostTag);
                }
                //_unitOfWork.Commit();       
            }
            return Post;
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
            return _postRepository.GetMultiPaging(x => x.Status && x.CategoryID == categoryId, out totalRow, page, pageSize, new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totalRow)
        {
            //TODO: Select all post by tag
            return _postRepository.GetAllByTag(tag, page, pageSize, out totalRow);

        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(x => x.Status, out totalRow, page, pageSize);
        }

        public Post GetById(int id)
        {
            return _postRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Post post)
        {
            _postRepository.Update(post);
        }
    }
}