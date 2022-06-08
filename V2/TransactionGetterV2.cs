// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.TransactionGetterV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.Common;
using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using PX.CCProcessingBase.Interfaces.V2;
using System;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class TransactionGetterV2 : PayByHostedFormHelperV2, ICCTransactionGetter
  {
    public TransactionGetterV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
    }

    public TransactionData GetTransaction(string transactionId)
    {
      PaymentCompleteResponse transaction = ProfileServer.GetTransaction(transactionId);
      if (transaction.transactionType == "TOKEN")
        transaction.responseCode = "00";
      return new TransactionData()
      {
        Amount = PayByPluginHelper.ToMYOBAmount(new Decimal?((Decimal) transaction.transactionAmount.paymentAmount)),
        AuthCode = transaction.responseText,
        CustomerId = transaction.clientRef,
        CcvVerificationStatus = CcvVerificationStatus.Match,
        DocNum = transaction.comment,
        ExpireAfterDays = PayByPluginHelper.AuthorizationValidPeriod,
        PaymentId = transactionId,
        ResponseReasonCode = 1,
        ResponseReasonText = transaction.responseText,
        SubmitTime = DateTime.Now,
        TranID = transaction.txnReference,
        TranStatus = PayByPluginHelper.TransactionStatus(transaction.responseCode),
        TranType = new CCTranType?(PayByPluginHelper.TransactionType(transaction.transactionType))
      };
    }

    public IEnumerable<TransactionData> GetTransactionsByCustomer(
      string customerProfileId,
      TransactionSearchParams searchParams = null)
    {
      return (IEnumerable<TransactionData>) new List<TransactionData>();
    }

    public IEnumerable<TransactionData> GetUnsettledTransactions(
      TransactionSearchParams searchParams = null)
    {
      return (IEnumerable<TransactionData>) new List<TransactionData>();
    }
  }
}
