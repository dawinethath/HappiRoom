using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("Demos")]
    public class Demo
    {
        public int id { get; set; }
        public int block_id { get; set; }
        public string name { get; set; }
        
    }
}