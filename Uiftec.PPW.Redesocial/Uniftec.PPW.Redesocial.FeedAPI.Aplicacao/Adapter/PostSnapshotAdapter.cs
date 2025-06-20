using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;

namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.Adapter
{
    public static class PostSnapshotAdapter
    {
        public static PostSnapshotDTO ToDTO(PostSnapshot entity)
        {
            return new PostSnapshotDTO
            {
                PostId = entity.PostId,
                AuthorId = entity.AuthorId,
                Content = entity.Content,
                MediaUrl = entity.MediaUrl,
                CreatedAt = entity.CreatedAt
            };
        }

        public static PostSnapshot ToEntity(PostSnapshotDTO dto)
        {
            return new PostSnapshot(
                dto.PostId,
                dto.AuthorId,
                dto.Content,
                dto.MediaUrl,
                dto.CreatedAt
            );
        }
    }
}
