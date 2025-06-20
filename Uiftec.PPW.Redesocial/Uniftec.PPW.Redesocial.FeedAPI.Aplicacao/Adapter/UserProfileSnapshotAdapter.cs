using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;

namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.Adapter
{
    public static class UserProfileSnapshotAdapter
    {
        public static UserProfileSnapshotDTO ToDTO(UserProfileSnapshot entity)
        {
            return new UserProfileSnapshotDTO
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                ProfileImageUrl = entity.ProfileImageUrl
            };
        }

        public static UserProfileSnapshot ToEntity(UserProfileSnapshotDTO dto)
        {
            return new UserProfileSnapshot(
                dto.UserId,
                dto.UserName,
                dto.ProfileImageUrl
            );
        }
    }
}
