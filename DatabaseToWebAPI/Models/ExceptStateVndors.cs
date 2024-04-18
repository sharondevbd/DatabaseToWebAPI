using System.ComponentModel.DataAnnotations;

namespace DatabaseToWebAPI.Models
{
	public class ExceptStateVndors
	{
        public string VendorName { get; set; }
        public string VendorState { get; set; }
        public int InvoiceNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime InvoiceVDate { get; set; }
    }
}
