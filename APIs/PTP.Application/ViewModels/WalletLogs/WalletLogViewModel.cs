using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.ViewModels.WalletLogs
{
    public class WalletLogViewModel
    {
        public string Source { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Amount { get; set; } = default!;
        public string? Type { get; set; }
        public Guid WalletId { get; set; }
    }
}
