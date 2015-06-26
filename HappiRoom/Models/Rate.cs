using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("rates")]
    public class Rate
    {
        public int id { get; set; }
        public decimal rate { get; set; }
        public DateTime date { get; set; }
        public int status { get; set; }

        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}