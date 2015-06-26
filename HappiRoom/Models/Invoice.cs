using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    [Table("invoices")]
    public class Invoice
    {
        public int id { get; set; }
        public int room_id { get; set; }
        public int customer_id { get; set; }
        public int block_id { get; set; }
        public string number { get; set; }
        public string room_number { get; set; }
        public string customer_name { get; set; }        
        public int wfrom { get; set; }
        public int wto { get; set; }
        public int wmax { get; set; }
        public string wround { get; set; }
        public int wqty { get; set; }
        public int wadd_on { get; set; }
        public decimal wprice { get; set; }
        public decimal wamt { get; set; }
        public int efrom { get; set; }
        public int eto { get; set; }
        public int emax { get; set; }
        public string eround { get; set; }
        public int eqty { get; set; }
        public int eadd_on { get; set; }
        public decimal eprice { get; set; }
        public decimal eamt { get; set; }
        public DateTime date_in { get; set; }
        public DateTime month_of { get; set; }        
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public decimal rental { get; set; }
        public decimal fine { get; set; }
        public decimal discount { get; set; }
        public decimal deposit { get; set; }
        public decimal debt { get; set; }
        public decimal service { get; set; }
        public decimal trash { get; set; }
        public decimal total { get; set; }
        public decimal rate { get; set; }
        public int rate_id { get; set; }
        public DateTime billing_date { get; set; }
        public DateTime due_date { get; set; }
        public decimal paid_usd { get; set; }
        public decimal paid_khr { get; set; }
        public DateTime paid_date { get; set; }
        public string memo { get; set; }
        public string memo2 { get; set; }
        public int status { get; set; }
        public int biller { get; set; }
        public int cashier { get; set; }
        public int printed { get; set; }           
                
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_at { get; set; }
    }
}