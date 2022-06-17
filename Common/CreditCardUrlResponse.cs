using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYOB.PayBy.CCProcessing.Common
{
    public class CreditCardUrlResponse
    {
        public string RequestID { get; set; }
        public DateTime URLExpiryDate { get; set; }
        public string URL { get; set; }
    }
}
