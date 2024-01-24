﻿using Microsoft.EntityFrameworkCore;
using PTP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        [Precision(18, 10)]
        public decimal Amount { get; set; } = default!;
        public string TransactionType { get; set; } = nameof(TransactionTypeEnum.None);
        public Guid WalletId { get; set; }
        public Guid? PaymentId { get; set; }
    }
}
