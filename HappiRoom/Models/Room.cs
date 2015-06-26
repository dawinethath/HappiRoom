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
        public int block_id { get; set; }
        public int customer_id { get; set; }
        public int rental_id { get; set; }
        public int electricity_fee_id { get; set; }
        public int water_fee_id { get; set; }
        public int wadd_on { get; set; }
        public int eadd_on { get; set; }
        public int status { get; set; }
                
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}