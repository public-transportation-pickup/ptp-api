using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.ViewModels.Orders
{
    public class OrderUpdateModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = default!;
    }
}
