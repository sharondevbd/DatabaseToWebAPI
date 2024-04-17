﻿using System;
using System.Collections.Generic;

namespace DatabaseToWebAPI.Models
{
    public partial class Invoicecopy
    {
        public int InvoiceId { get; set; }
        public int VendorId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal CreditTotal { get; set; }
        public int TermsId { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
