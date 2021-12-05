using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgusBot.DAL.Models
{
    public class GroupAdmin
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
