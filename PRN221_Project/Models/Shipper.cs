﻿using System;
using System.Collections.Generic;

namespace PRN221_Project.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }

        public int ShipperId { get; set; }
        public string? ShipperName { get; set; }
        public string? Company { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
