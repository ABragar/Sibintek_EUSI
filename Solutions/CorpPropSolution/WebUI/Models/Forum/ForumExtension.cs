using Base.Forum.Entities;
using System;
using System.Linq;
using Base.Utils.Common;
using Base.Utils.Common.Caching;

namespace WebUI.Models.Forum
{
    public static class ForumExtension
    {
        public static IQueryable<SectionViewModel> ToViewModel(this IQueryable<ForumSection> sections)
        {
            return sections.Select(x => new SectionViewModel()
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                CreateRecord = new RecordUserInfo()
                {
                    UserID = x.CreateUser.ID,
                    Image = x.CreateUser.Image != null ? x.CreateUser.Image.FileID : default(Nullable<Guid>),
                    Date = x.CreateDate
                },
                TopicsCount = x.Topics.Where(z => z.Status == ForumTopicStatus.Normal).Count(),
                PostsCount = x.Topics.SelectMany(z => z.Posts).Where(z => z.Status == ForumPostStatus.Normal).Count(),
                PremodPostsCount = x.Topics.SelectMany(z => z.Posts).Count(z => z.Status == ForumPostStatus.Premoderate)
            });
        }

        public static IQueryable<TopicViewModel> ToViewModel(this IQueryable<ForumTopic> topics)
        {
            return topics.Select(x => new
            {
                ID = x.ID,
                SectionID = x.SectionID,
                Title = x.Title,
                Description = x.Description,
                CreateRecord = new RecordUserInfo()
                {
                    UserID = x.CreateUser.ID,
                    Title = x.CreateUser.FullName,
                    Image = x.CreateUser.Image != null ? x.CreateUser.Image.FileID : default(Nullable<Guid>),
                    Date = x.CreateDate
                },
                LastRecord = x.Posts.OrderByDescending(z => z.ID).FirstOrDefault(z => z.Status == ForumPostStatus.Normal),
                PostsCount = x.Posts.Where(z => z.Status == ForumPostStatus.Normal).Count(),
                ViewsCount = x.ViewsCount,
                Status = x.Status,
                PremodPostsCount = x.Posts.Where(z => z.Status == ForumPostStatus.Premoderate).Count()
            })
            .Select(x => new TopicViewModel()
            {
                ID = x.ID,
                SectionID = x.SectionID,
                Title = x.Title,
                Description = x.Description,
                CreateRecord = x.CreateRecord,
                LastRecord = x.LastRecord != null ? new RecordUserInfo()
                {
                    UserID = x.LastRecord.CreateUserID,
                    Title = x.LastRecord.CreateUser.FullName,
                    Image = x.LastRecord.CreateUser.Image != null ? x.LastRecord.CreateUser.Image.FileID : default(Nullable<Guid>),
                    Date = x.LastRecord.CreateDate
                } : x.CreateRecord,
                PostsCount = x.PostsCount,
                ViewsCount = x.ViewsCount,
                Status = x.Status,
                PremodPostsCount = x.PremodPostsCount
            });
        }

        public static IQueryable<PostViewModel> ToViewModel(this IQueryable<ForumPost> posts)
        {
            return posts.Select(x => new PostViewModel()
            {
                ID = x.ID,
                TopicID = x.TopicID,
                Message = x.Message,
                CreateRecord = new RecordUserInfo()
                {
                    UserID = x.CreateUser.ID,
                    Title = x.CreateUser.FullName,
                    Image = x.CreateUser.Image != null ? x.CreateUser.Image.FileID : default(Nullable<Guid>),
                    Date = x.CreateDate
                },
                Status = x.Status,
                IsFirst = x.isFirst
            });
        }

        public static IQueryable<TopicViewModel> FullTextSearch(this IQueryable<ForumSection> sections, string searchStr, ISimpleCacheWrapper cache)
        {
            return sections.SelectMany(x => x.Topics).FullTextSearch(searchStr, cache).Where(x => x.Status == ForumTopicStatus.Normal).ToViewModel().OrderBy(x => x.LastRecord.Date);
        }
    }
}