using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades
{
    public class FeedItem
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ContentId { get; private set; }
        public ContentType ContentType { get; private set; } // Isso aqui precisa sair!
        public DateTime PublishedAt { get; private set; }
        public string TextPreview { get; private set; }
        public int LikeCount { get; private set; }
        public int CommentCount { get; private set; }

        public FeedItem(Guid id, Guid userId, Guid contentId, ContentType contentType, DateTime publishedAt, string textPreview, int likeCount, int commentCount)
        {
            Id = id;
            UserId = userId;
            ContentId = contentId;
            ContentType = contentType;
            PublishedAt = publishedAt;
            TextPreview = textPreview;
            LikeCount = likeCount;
            CommentCount = commentCount;
        }

        public FeedItem(Guid id, Guid userId, Guid contentId, DateTime publishedAt, string textPreview, int likeCount, int commentCount)
        {
            Id = id;
            UserId = userId;
            ContentId = contentId;
            PublishedAt = publishedAt;
            TextPreview = textPreview;
            LikeCount = likeCount;
            CommentCount = commentCount;
        }
    }

}
