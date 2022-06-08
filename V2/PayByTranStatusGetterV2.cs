// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByTranStatusGetterV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByTranStatusGetterV2 : ICCTranStatusGetter
  {
    public CCTranStatus GetTranStatus(ProcessingResult result) => !int.TryParse(result.ResponseReasonCode, out int _) ? CCTranStatus.Approved : CCTranStatus.Approved;
  }
}
