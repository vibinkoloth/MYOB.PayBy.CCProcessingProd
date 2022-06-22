using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYOB.PayBy.CCProcessing.V2
{
    public class CreditCardUrlProcessorV2 : CreditCardUrlHelper
    {
        private readonly IEnumerable<SettingsValue> _settingsValues;

        public CreditCardUrlProcessorV2(IEnumerable<SettingsValue> settingValues)
          : base(settingValues)
        {
            this._settingsValues = settingValues;
        }

        public CreditCardUrlResponse GetUrlForCreditCard(CustomerData customerData)
        {
            ProcessingInput input = new ProcessingInput()
            {
                CustomerData = new CustomerData()
            };
            input.CustomerData = customerData;
            string curyid = this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>)(x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>)(v => v.Value)).FirstOrDefault<string>();
            return this.GetUrlForCreditCard(input, this._settingsValues, curyid).response;
        }

        public CreditCardUrlResponse GetUrlForCreditCardTest(CustomerData customerData)
        {
            ProcessingInput input = new ProcessingInput()
            {
                CustomerData = new CustomerData()
            };
            input.CustomerData = customerData;
            string curyid = this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>)(x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>)(v => v.Value)).FirstOrDefault<string>();
            return this.GetUrlForCreditCard(input, this._settingsValues, curyid).response;
        }

        public IEnumerable<CreditCardData> GetAllPaymentProfiles(string customerProfileId,string requestedId)
        {
            return this.GetAllPaymentProfiles(customerProfileId, this._settingsValues, requestedId);
        }
    }
}
