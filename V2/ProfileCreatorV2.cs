// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.ProfileCreatorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class ProfileCreatorV2 : PayByHostedFormHelperV2, ICCProfileCreator
  {
    public ProfileCreatorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
    }

    public TranProfile GetOrCreatePaymentProfileFromTransaction(
      string transactionId,
      CreateTranPaymentProfileParams cParams)
    {
      return new TranProfile()
      {
        CustomerProfileId = cParams.PCCustomerId,
        PaymentProfileId = transactionId
      };
    }
  }
}
