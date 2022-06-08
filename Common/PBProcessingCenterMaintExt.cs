// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PBProcessingCenterMaintExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Objects.AR.CCPaymentProcessing.Common;
using PX.Objects.AR.CCPaymentProcessing.Helpers;
using PX.Objects.CA;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PBProcessingCenterMaintExt : PXGraphExtension<CCProcessingCenterMaint>
  {
    protected void CCProcessingCenter_RowSelected(
      PXCache sender,
      PXRowSelectedEventArgs e,
      PXRowSelected baseevent)
    {
      baseevent(sender, e);
      if (!(e.Row is CCProcessingCenter row))
        return;
      bool flag1 = CCProcessingFeatureHelper.IsFeatureSupported(row, CCProcessingFeature.ProfileManagement);
      bool flag2 = CCProcessingFeatureHelper.IsFeatureSupported(row, CCProcessingFeature.HostedForm);
      bool flag3 = row.ProcessingTypeName == "MYOB.PayBy.CCProcessing.V2.PayByProcessingPluginV2" || row.ProcessingTypeName == "MYOB.PayBy.CCProcessing.V2.PayByProcessingPluginV2";
      PXUIFieldAttribute.SetVisible<CCProcessingCenter.useAcceptPaymentForm>(sender, (object) row, flag1 & flag2 & flag3);
      if (!(row.ProcessingTypeName == "MYOB.PayBy.CCProcessing.V2.PayByDirectDebitProcessingPlugin"))
        return;
      this.Base.testCredentials.SetEnabled(false);
    }
  }
}
