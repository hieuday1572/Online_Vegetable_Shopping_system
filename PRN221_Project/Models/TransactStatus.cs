﻿using System;
using System.Collections.Generic;

namespace PRN221_Project.Models
{
    public partial class TransactStatus
    {
        public TransactStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int TransactStatusId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
