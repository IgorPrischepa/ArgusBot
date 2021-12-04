using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgusBot.DAL.Models
{
    [Index(nameof(GroupId), nameof(UserId))]
    public class CheckList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string QuestionMessageId { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        public StatusTypes Status { get; set; }

        public DateTime SendingTime { get; set; }
    }
}
