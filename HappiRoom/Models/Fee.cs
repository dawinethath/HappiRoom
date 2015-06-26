using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("Fees")]
    public class Fee
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal fee { get; set; }

        public string type { get; set; }
        public int status { get; set; }

        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}