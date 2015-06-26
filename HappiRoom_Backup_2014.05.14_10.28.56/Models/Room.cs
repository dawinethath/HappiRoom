using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("rooms")]
    public class Room
    {
        public int id { get; set; }
        public string number { get; set; }
        public string description { get; set; }
        public byte status { get; set; }    
                
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}