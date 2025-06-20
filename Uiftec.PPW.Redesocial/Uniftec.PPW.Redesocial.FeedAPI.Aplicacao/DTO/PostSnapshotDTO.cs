using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO
{
    public class PostSnapshotDTO
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
}
