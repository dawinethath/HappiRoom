using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("lottery_records")]
    public class LotteryRecord
    {
        public int id { get; set; }
        public string types { get; set; }
        public DateTime dates { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int a3 { get; set; }
        public int a4 { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int d { get; set; }

        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
        
    }
}