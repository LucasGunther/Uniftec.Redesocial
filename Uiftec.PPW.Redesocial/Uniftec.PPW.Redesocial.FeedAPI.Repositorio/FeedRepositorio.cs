using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Entidades;
using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Repositorio;

namespace Uniftec.PPW.Redesocial.FeedAPI.Repositorio
{
    public class FeedRepository : IFeedRepositorio
    {
        //private string strConexao = "Server=localhost;Port=5432;Database=projetosweb;User Id=postgres;Password=Guntherzinho00";
        private string strConexao = "Server=localhost;Port=5432;Database=redesocial;User Id=postgres;Password=tatubola;";
        public void AddFeedItem(FeedItem feedItem)
        {
            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        var cmd = new NpgsqlCommand();
                        cmd.Connection = conexao;
                        cmd.Transaction = transacao;

                        cmd.CommandText = @"INSERT INTO public.feed
                            (id, userid, contentid, publishedat, textpreview, likecount, commentcount)
                            VALUES (@id, @userid, @contentid, @publishedat, @textpreview, @likecount, @commentcount);";

                        cmd.Parameters.AddWithValue("id", feedItem.Id);
                        cmd.Parameters.AddWithValue("userid", feedItem.UserId);
                        cmd.Parameters.AddWithValue("contentid", feedItem.ContentId);
                        cmd.Parameters.AddWithValue("publishedat", feedItem.PublishedAt);
                        cmd.Parameters.AddWithValue("textpreview", feedItem.TextPreview ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("likecount", feedItem.LikeCount);
                        cmd.Parameters.AddWithValue("commentcount", feedItem.CommentCount);

                        cmd.ExecuteNonQuery();

                        transacao.Commit();
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void AddFeedItems(IEnumerable<FeedItem> feedItems)
        {
            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        foreach (var feedItem in feedItems)
                        {
                            var cmd = new NpgsqlCommand();
                            cmd.Connection = conexao;
                            cmd.Transaction = transacao;

                            cmd.CommandText = @"INSERT INTO public.feed
                                (id, userid, contentid, publishedat, textpreview, likecount, commentcount)
                                VALUES (@id, @userid, @contentid, @publishedat, @textpreview, @likecount, @commentcount);";

                            cmd.Parameters.AddWithValue("id", feedItem.Id);
                            cmd.Parameters.AddWithValue("userid", feedItem.UserId);
                            cmd.Parameters.AddWithValue("contentid", feedItem.ContentId);
                            cmd.Parameters.AddWithValue("publishedat", feedItem.PublishedAt);
                            cmd.Parameters.AddWithValue("textpreview", feedItem.TextPreview ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("likecount", feedItem.LikeCount);
                            cmd.Parameters.AddWithValue("commentcount", feedItem.CommentCount);

                            cmd.ExecuteNonQuery();
                        }

                        transacao.Commit();
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public List<FeedItem> GetFeedByUserId(Guid userId)
        {
            var feedItems = new List<FeedItem>();

            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = @"SELECT id, userid, contentid, publishedat, textpreview, likecount, commentcount
                                    FROM public.feed
                                    WHERE userid = @userid
                                    ORDER BY publishedat DESC;";

                cmd.Parameters.AddWithValue("userid", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var feedItem = new FeedItem(
                            reader.GetGuid(0),                     // Id
                            reader.GetGuid(1),                     // UserId
                            reader.GetGuid(2),                     // ContentId
                            reader.GetDateTime(3),                 // PublishedAt
                            reader.IsDBNull(4) ? null : reader.GetString(4), // TextPreview
                            reader.GetInt32(5),                    // LikeCount
                            reader.GetInt32(6)                     // CommentCount
                        );

                        feedItems.Add(feedItem);
                    }
                }
            }

            return feedItems;
        }

        public void RemoveFeedItem(Guid feedItemId)
        {
            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        var cmd = new NpgsqlCommand();
                        cmd.Connection = conexao;
                        cmd.Transaction = transacao;

                        cmd.CommandText = @"DELETE FROM public.feed WHERE id = @id;";
                        cmd.Parameters.AddWithValue("id", feedItemId);

                        cmd.ExecuteNonQuery();

                        transacao.Commit();
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void UpdateFeedItem(FeedItem feedItem)
        {
            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        var cmd = new NpgsqlCommand();
                        cmd.Connection = conexao;
                        cmd.Transaction = transacao;

                        cmd.CommandText = @"UPDATE public.feed 
                                            SET textpreview = @textpreview, 
                                                likecount = @likecount, 
                                                commentcount = @commentcount
                                            WHERE id = @id;";

                        cmd.Parameters.AddWithValue("id", feedItem.Id);
                        cmd.Parameters.AddWithValue("textpreview", feedItem.TextPreview ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("likecount", feedItem.LikeCount);
                        cmd.Parameters.AddWithValue("commentcount", feedItem.CommentCount);

                        cmd.ExecuteNonQuery();

                        transacao.Commit();
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public List<FeedItem> GetAllFeedItems()
        {
            var feedItems = new List<FeedItem>();

            using (var conexao = new NpgsqlConnection(strConexao))
            {
                conexao.Open();

                var cmd = new NpgsqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = @"SELECT id, userid, contentid, publishedat, textpreview, likecount, commentcount
                            FROM public.feed
                            ORDER BY publishedat DESC;";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var feedItem = new FeedItem(
                            reader.GetGuid(0),
                            reader.GetGuid(1),
                            reader.GetGuid(2),
                            reader.GetDateTime(3),
                            reader.IsDBNull(4) ? null : reader.GetString(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6)
                        );
                        feedItems.Add(feedItem);
                    }
                }
            }

            return feedItems;
        }
    }
}
