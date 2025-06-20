using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO
{
    public class FeedItemDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime PublishedAt { get; set; }
        public string TextPreview { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
