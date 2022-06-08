// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByProfileProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByProfileProcessorV2 : PayByProcessorV2, ICCProfileProcessor
  {
    private IEnumerable<SettingsValue> _settingsValues;

    public PayByProfileProcessorV2(IEnumerable<SettingsValue> settingsValues)
      : base(settingsValues)
    {
      this._settingsValues = settingsValues;
    }

    public string CreateCustomerProfile(CustomerData customerData) => customerData.CustomerCD;

    public string CreatePaymentProfile(string customerProfileId, CreditCardData cardData) => cardData.PaymentProfileID;

    public void DeleteCustomerProfile(string customerProfileId)
    {
    }

    public void DeletePaymentProfile(string customerProfileId, string paymentProfileId)
    {
    }

    public IEnumerable<CustomerData> GetAllCustomerProfiles() => (IEnumerable<CustomerData>) new List<CustomerData>();

    public IEnumerable<CreditCardData> GetAllPaymentProfiles(
      string customerProfileId)
    {
      return ProfileServer.GetAllPaymentProfiles(customerProfileId);
    }

    public CustomerData GetCustomerProfile(string customerProfileId) => new CustomerData();

    public CreditCardData GetPaymentProfile(
      string customerProfileId,
      string paymentProfileId)
    {
      return ProfileServer.GetPaymentProfile(paymentProfileId);
    }

    public void UpdateCustomerProfile(CustomerData customerData)
    {
    }

    public void UpdatePaymentProfile(string customerProfileId, CreditCardData cardData)
    {
    }
  }
}
