namespace Uiftec.PPW.Redesocial.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime PublishedAt { get; set; }
        public string TextPreview { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime date {  get; set; }
    }

}
