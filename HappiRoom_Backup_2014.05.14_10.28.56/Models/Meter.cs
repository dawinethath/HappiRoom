using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("meters")]
    public class Meter
    {
        public int id { get; set; }
        public int room_id { get; set; }
        public string number { get; set; }
        public string type { get; set; }
        public int status { get; set; }
        public int max_digit { get; set; }
        public DateTime used_date { get; set; }

        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}