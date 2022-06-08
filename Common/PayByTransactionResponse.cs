// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByTransactionResponse
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.component;
using System;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PayByTransactionResponse : PaybyHttpResponse
  {
    public InitResponse initResponse { get; set; }

    public DateTime settlementDate { get; set; }

    public string tokenResponseText { get; set; }

    public bool tokenized { get; set; }

    public string token { get; set; }

    public string responseText { get; set; }

    public string responseCode { get; set; }

    public string feeReference { get; set; }

    public string authCode { get; set; }

    public string txnReference { get; set; }

    public string clientRef { get; set; }

    public TransactionAmount transactionAmount { get; set; }

    public CreditCard creditCard { get; set; }

    public string transactionType { get; set; }

    public string clientIdHash { get; set; }

    public int clientId { get; set; }

    public string comment { get; set; }

    public string cvcResponse { get; set; }
  }
}
