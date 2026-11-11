using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shared.Common
{
    public class PaymentGatewaySettings
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = default!;
        public string WebhookSecret { get; set; } = default!;
    }
}
