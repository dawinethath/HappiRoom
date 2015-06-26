using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HappiRoom.Models
{
    public class HappiRoomContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Company> Companies  { get; set; }
        public DbSet<Demo> Demos { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<LotteryRecord> LotteryRecords { get; set; }
    }
}