// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByTransactionProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using PX.CS;
using System;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByTransactionProcessorV2 : PayByProcessorV2, ICCTransactionProcessor
  {
    private PayByHttpRequest httpRequest;
    private IEnumerable<SettingsValue> _SettingValues;

    public PayByTransactionProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
      this._SettingValues = settingValues;
    }

    public ProcessingResult Processor(PayByHttpRequest transactionRequest2) => PayByPluginHelper.ProcessTransactionResponseV2(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createTransactionController>(transactionRequest2, new createTransactionController(transactionRequest2)));

    public ProcessingResult DoTransaction(ProcessingInput inputData)
    {
      this.httpRequest = new PayByHttpRequest();
      RMColumn rmColumn = new RMColumn();
      string error = PayByValidatorV2.ValidateForTransaction(inputData);
      if (!string.IsNullOrEmpty(error))
        throw new CCProcessingException(error);
      this.httpRequest.realTimeRequest = new PaymentRealTimeRequest();
      switch (inputData.TranType)
      {
        case CCTranType.AuthorizeAndCapture:
          this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.PURCHASE.ToString();
          break;
        case CCTranType.AuthorizeOnly:
          this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.AUTHORISATION.ToString();
          break;
        case CCTranType.PriorAuthorizedCapture:
        case CCTranType.CaptureOnly:
          this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.COMPLETION.ToString();
          break;
        case CCTranType.Credit:
          this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.REFUND.ToString();
          break;
        case CCTranType.Void:
          this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.REVERSAL.ToString();
          break;
        default:
          throw new NotImplementedException();
      }
      if (this.httpRequest.realTimeRequest.transactionType == transactionTypeEnum.COMPLETION.ToString() && string.IsNullOrEmpty(inputData.OrigTranID))
        this.httpRequest.realTimeRequest.transactionType = transactionTypeEnum.PURCHASE.ToString();
      ProcessingResult processingResult1 = new ProcessingResult();
      this.httpRequest.OperationType = operationEnum.PAYMENT_REAL_TIME;
      this.httpRequest.paybyClientConfig = this.ClientConfigAuthentication;
      PayByHttpRequest httpRequest = this.httpRequest;
      httpRequest.realTimeRequest = PayByPluginHelper.GetTransactionRequest(inputData, this.httpRequest.paybyClientConfig.clientId, this.httpRequest.realTimeRequest.transactionType, this.httpRequest.paybyClientConfig.curyID);
      ProcessingResult processingResult2 = PayByPluginHelper.ProcessTransactionResponseV2(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createTransactionController>(httpRequest, new createTransactionController(httpRequest)));
      if (inputData.TranType == CCTranType.AuthorizeOnly)
        processingResult2.ExpireAfterDays = PayByPluginHelper.AuthorizationValidPeriod;
      return processingResult2;
    }
  }
}
