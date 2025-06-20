using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.DTO;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;


namespace Uniftec.PPW.Redesocial.FeedAPI.Aplicacao.Adapter
{
    public static class FeedItemAdapter
    {
        public static FeedItemDTO ToDTO(FeedItem entity)
        {
            return new FeedItemDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ContentId = entity.ContentId,
                PublishedAt = entity.PublishedAt,
                TextPreview = entity.TextPreview,
                LikeCount = entity.LikeCount,
                CommentCount = entity.CommentCount
            };
        }

        public static FeedItem ToEntity(FeedItemDTO dto)
        {
            return new FeedItem(
                dto.Id,
                dto.UserId,
                dto.ContentId,
                dto.PublishedAt,
                dto.TextPreview,
                dto.LikeCount,
                dto.CommentCount
            );
        }
    }
}
