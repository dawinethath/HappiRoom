using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("Blocks")]
    public class Block
    {
        public int id { get; set; }
        public string name { get; set; }
        
    }
}