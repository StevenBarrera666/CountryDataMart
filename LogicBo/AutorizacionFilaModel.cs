using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicBo
{
    public class AutorizacionFilaModel
    {
        public string FileId { get; set; }
        public string CustomerId { get; set; }
        public string FileDetailID { get; set; }
        public string PaymentMethodId { get; set; }
        public string PaymentCardId { get; set; }
        public decimal FareTA { get; set; }
        public decimal IVATA { get; set; }
        public decimal FareWithTaxes { get; set; }

        
    }
}