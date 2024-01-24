﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.ViewModels.Stores
{
    public class StoreViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Status { get; set; } = default!;
        public TimeSpan OpenedTime { get; set; } = default!;
        public TimeSpan ClosedTime { get; set; } = default!;
        [Precision(18, 2)]
        public decimal Latitude { get; set; } = default!;
        [Precision(18, 2)]
        public decimal Longitude { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateTime? ActivationDate { get; set; } = null;
        public string ImageName { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public Guid UserId { get; set; } = default!;
        public string Email {  get; set; } = default!;
        public string Password { get; set; } = default!;
        public Guid WalletId { get; set; }
        public decimal WalletAmount { get; set; } = default!;
    }
}