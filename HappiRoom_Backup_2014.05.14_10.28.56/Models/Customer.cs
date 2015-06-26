using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{  
    [Table("customers")]
    public class Customer
    {
        public int id { get; set; }
        public int room_id { get; set; }
        public string number { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public DateTime date_of_birth { get; set; }
        public string place_of_birth { get; set; }
        public string national_card_no { get; set; }
        public decimal balance { get; set; }
        public decimal deposit { get; set; }
        public string memo { get; set; }
        public DateTime registered_date { get; set; }
        public DateTime date_in { get; set; }
        public DateTime date_going_out { get; set; }
        public DateTime date_out { get; set; }

        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }        
        public DateTime updated_at { get; set; }
    }
}