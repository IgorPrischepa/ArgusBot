using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ArgusBot.DAL.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public List<GroupAdmin> GroupAdmins { get; set; }
    }
}
