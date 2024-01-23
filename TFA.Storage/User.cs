﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MaxLength(20)]
        public string Loign { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Topic.Author))]
        public ICollection<Topic> Topics { get; set; }
    
    }
}