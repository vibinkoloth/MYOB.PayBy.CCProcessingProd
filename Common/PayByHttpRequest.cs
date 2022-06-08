// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByHttpRequest
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PayByHttpRequest
  {
    public PayByClientConfig paybyClientConfig { get; set; }

    public operationEnum OperationType { get; set; }

    public PaymentInitRequest initRequest { get; set; }

    public PaymentCompleteRequest completeRequest { get; set; }

    public PaymentRealTimeRequest realTimeRequest { get; set; }

    public PayBySoapRequest soapRequest { get; set; }

    public transactionTypeEnum transactionType { get; set; }
  }
}
