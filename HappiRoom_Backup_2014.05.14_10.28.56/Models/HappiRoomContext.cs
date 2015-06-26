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
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}