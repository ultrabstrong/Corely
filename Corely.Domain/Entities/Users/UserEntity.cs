﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Corely.Domain.Entities.Users
{
    [Table("Users")]
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedUtc { get; set; }

        public bool Enabled { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [ForeignKey(nameof(UserDetailsEntity.UserId))]
        public List<UserDetailsEntity>? Details { get; set; }
    }
}
