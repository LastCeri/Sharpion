using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpion.IonUtils.PacksOps.SendTransaction
{
    public class TransactionInteraction
    {
        public string ReceiptAddress { get; set; }
        public string ContractAddress { get; set; }
        public string Amount { get; set; }
    }
}
