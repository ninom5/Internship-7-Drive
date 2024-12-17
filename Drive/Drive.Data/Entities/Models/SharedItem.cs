using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Data.Entities.Models
{
    public class SharedItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DataType ItemType { get; set; }
        public int SharedWithId { get; set; }
        public int SharedById { get; set; }
        public DateTime SharedAt { get; set; }

        public User SharedWith { get; set; }
        public User SharedBy { get; set; } 
    }

}
