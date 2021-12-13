using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArgusBot.DAL.Models
{
    public class GroupAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long TelegramUserId { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
