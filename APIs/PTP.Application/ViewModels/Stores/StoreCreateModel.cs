using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.ViewModels.Stores
{
    public class StoreCreateModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string OpenedTime { get; set; } = default!;
        public string ClosedTime { get; set; } = default!;
        [Precision(18, 10)]
        public decimal Latitude { get; set; } = default!;
        [Precision(18, 10)]
        public decimal Longitude { get; set; } = default!;
        public string Zone { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string AddressNo { get; set; } = default!;
        public string Street { get; set; } = default!;
        public IFormFile? File { get; set; } = default!;
        public DateTime? ActivationDate { get; set; } = null;
        public string Email { get; set; } = default!;
        public string ManagerName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now!;
        public string ManagerPhone { get; set; } = default!;

        public List<Guid>? StationIds { get; set; }

    }
}
