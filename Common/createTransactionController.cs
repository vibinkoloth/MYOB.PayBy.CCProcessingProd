// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.createTransactionController
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

namespace MYOB.PayBy.CCProcessing.Common
{
  public class createTransactionController : ApiOperationBase<PayByHttpRequest, PaybyHttpResponse>
  {
    public createTransactionController(PayByHttpRequest apiRequest)
      : base(apiRequest)
    {
    }

    protected override void ValidateRequest() => this.GetApiRequest();
  }
}
