// Decompiled with JetBrains decompiler
// Type: PX.CCProcessing.V2.ICCPaymentProfileProcessor
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace PX.CCProcessing.V2
{
  internal interface ICCPaymentProfileProcessor
  {
    string CreatePaymentProfile(string customerProfileId, CreditCardData cardData);

    void DeletePaymentProfile(string customerProfileId, string paymentProfileId);

    IEnumerable<CreditCardData> GetAllPaymentProfiles(
      string customerProfileId);

    CreditCardData GetPaymentProfile(
      string customerProfileId,
      string paymentProfileId);

    void UpdatePaymentProfile(string customerProfileId, CreditCardData cardData);

        void test();
  }
}
