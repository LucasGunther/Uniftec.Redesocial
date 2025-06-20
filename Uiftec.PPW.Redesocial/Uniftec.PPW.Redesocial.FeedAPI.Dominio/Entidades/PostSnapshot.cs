using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades
{
    public class PostSnapshot
    {
        public Guid PostId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Content { get; private set; }
        public string MediaUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public PostSnapshot(Guid postId, Guid authorId, string content, string mediaUrl, DateTime createdAt)
        {
            PostId = postId;
            AuthorId = authorId;
            Content = content;
            MediaUrl = mediaUrl;
            CreatedAt = createdAt;
        }
    }
}
