using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO
{
    public class UserProfileSnapshotDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
