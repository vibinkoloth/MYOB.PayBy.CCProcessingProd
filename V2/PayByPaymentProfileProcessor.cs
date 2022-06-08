// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByPaymentProfileProcessor
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessing.V2;
using PX.CCProcessingBase.Interfaces.V2;
using System;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByPaymentProfileProcessor : PayByProcessorV2, ICCPaymentProfileProcessor
  {
    public PayByPaymentProfileProcessor(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
    }

    private bool CheckRequiredFieldsMissedError(string message) => message.Contains("E00027");

    string ICCPaymentProfileProcessor.CreatePaymentProfile(
      string customerProfileId,
      CreditCardData cardData)
    {
      throw new NotImplementedException();
    }

    void ICCPaymentProfileProcessor.DeletePaymentProfile(
      string customerProfileId,
      string paymentProfileId)
    {
      throw new NotImplementedException();
    }

    IEnumerable<CreditCardData> ICCPaymentProfileProcessor.GetAllPaymentProfiles(
      string customerProfileId)
    {
      throw new NotImplementedException();
    }

    CreditCardData ICCPaymentProfileProcessor.GetPaymentProfile(
      string customerProfileId,
      string paymentProfileId)
    {
      throw new NotImplementedException();
    }

    void ICCPaymentProfileProcessor.UpdatePaymentProfile(
      string customerProfileId,
      CreditCardData cardData)
    {
      throw new NotImplementedException();
    }
  }
}
