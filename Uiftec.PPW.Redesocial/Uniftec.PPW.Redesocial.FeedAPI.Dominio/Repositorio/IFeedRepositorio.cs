using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;

namespace Uniftec.PPW.Redesocial.FeedAPI.Dominio.Repositorio
{
    public interface IFeedRepositorio
    {
        List<FeedItem> GetFeedByUserId(Guid userId);
        void AddFeedItem(FeedItem feedItem);
        void AddFeedItems(IEnumerable<FeedItem> feedItems);
        void RemoveFeedItem(Guid feedItemId);
        void UpdateFeedItem(FeedItem feedItem);
        List<FeedItem> GetAllFeedItems();
    }
}
