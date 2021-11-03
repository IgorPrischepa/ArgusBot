﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgusBot.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserGuid { get; set; }

        [Required]
        [MaxLength(40)]
        public string Login { get; set; }

        [Required]
        public string NormalizedLogin { get; set; }

        [Required]
        public string Password { get; set; }

        public string TelegramId { get; set; }
    }
}