using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
        
        public string Text { get; set; }


        
        [ForeignKey("UserId")]
        public User Author {  get; set; }

        public Guid UserId { get; set; }

        
        
        public Guid TopicId { get; set; }
        
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
    }
} 