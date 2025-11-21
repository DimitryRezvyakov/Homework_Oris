using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BudgetDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
    }
}
