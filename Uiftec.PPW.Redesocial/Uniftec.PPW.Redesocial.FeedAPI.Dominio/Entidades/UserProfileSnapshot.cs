using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades
{
    public class UserProfileSnapshot
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }
        public string ProfileImageUrl { get; private set; }

        public UserProfileSnapshot(Guid userId, string userName, string profileImageUrl)
        {
            UserId = userId;
            UserName = userName;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
