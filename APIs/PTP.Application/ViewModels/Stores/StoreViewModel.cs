﻿using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

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
        public string OpenedTime { get; set; } = default!;
        public string ClosedTime { get; set; } = default!;
        [Precision(18, 2)]
        public decimal Latitude { get; set; } = default!;
        [Precision(18, 2)]
        public decimal Longitude { get; set; } = default!;
        public string AddressNo { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Zone { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public DateTime? ActivationDate { get; set; } = null;
        public string ImageName { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public Guid UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public List<Guid> StationIds { get; set; } = new();
        public List<string> StationName { get; set; } = new();
        public string ManagerName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ManagerPhone { get; set; } = string.Empty;
        // public Guid WalletId { get; set; }
        // public decimal WalletAmount { get; set; } = default!;
    }
}
