using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades
{
    public class Feed
    {
        public Guid UserId { get; private set; }
        public List<FeedItem> Items { get; private set; }

        public Feed(Guid userId)
        {
            UserId = userId;
            Items = new List<FeedItem>();
        }

        public void AddItem(FeedItem item)
        {
            Items.Add(item);
        }
    }
}
