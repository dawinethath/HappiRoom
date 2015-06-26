using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("Bills")]
    public class Bill
    {
        public int id { get; set; }
        public string type { get; set; }
        public string number { get; set; }
        public decimal amount { get; set; }
        public decimal discount { get; set; }
        public decimal deposit { get; set; }
        public decimal rate { get; set; }
        public string sub_code { get; set; }
        public DateTime date { get; set; }
        public DateTime due_date { get; set; }
        public int status { get; set; }
        public int biller { get; set; }
        public int cashier { get; set; }
        public decimal paid_usd { get; set; }
        public decimal paid_khr { get; set; }
        public DateTime paid_date { get; set; }
        public string memo { get; set; }
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}