// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByDDTransactionProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByDDTransactionProcessorV2 : PayByProcessorV2, ICCTransactionProcessor
  {
    private IEnumerable<SettingsValue> _SettingValues;

    public PayByDDTransactionProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
      this._SettingValues = settingValues;
    }

    public ProcessingResult Processor(PayByHttpRequest transactionRequest2) => PayByPluginHelper.ProcessTransactionResponseV2(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createTransactionController>(transactionRequest2, new createTransactionController(transactionRequest2)));

    public ProcessingResult DoTransaction(ProcessingInput inputData)
    {
      ProcessingResult processingResult1 = new ProcessingResult();
      if (inputData.Amount <= 0M)
        PayByValidatorV2.AmountShouldNotbeZero();
      PayByHttpRequest transactionRequest2 = (PayByHttpRequest) new PayByRealtimePaymentRequest();
      transactionRequest2.soapRequest = new PayBySoapRequest();
      transactionRequest2.OperationType = operationEnum.PAYMENT_BATCH;
      transactionRequest2.paybyClientConfig = this.ClientConfigAuthentication;
      PayByBatchProcessorV2 batchProcessorV2 = new PayByBatchProcessorV2(this._SettingValues);
      string str;
      switch (inputData.TranType)
      {
        case CCTranType.AuthorizeOnly:
          transactionRequest2.transactionType = transactionTypeEnum.PURCHASE;
          transactionRequest2.soapRequest.soapEnvelopeXml = this.GetTransactionRequest(inputData, transactionTypeEnum.PURCHASE);
          str = "Request accepted.";
          break;
        case CCTranType.Credit:
          transactionRequest2.transactionType = transactionTypeEnum.REFUND;
          transactionRequest2.soapRequest.soapEnvelopeXml = this.GetTransactionRequest(inputData, transactionTypeEnum.REFUND);
          str = "Refund request accepted.";
          break;
        default:
          throw new NotImplementedException("Use Authorize CC Payment for direct debit payment.");
      }
      ProcessingResult processingResult2 = batchProcessorV2.Processor(transactionRequest2);
      ProcessingResult processingResult3;
      if (processingResult2 != null && !string.IsNullOrEmpty(processingResult2.TransactionNumber))
      {
        processingResult2.ResponseReasonText = str;
        processingResult2.ResponseCode = "HFR";
        processingResult2.ResponseReasonCode = processingResult2.TransactionNumber;
        processingResult3 = processingResult2;
      }
      else
      {
        processingResult2.ResponseReasonCode = "ERR";
        processingResult3 = processingResult2;
      }
      return processingResult3;
    }

    protected string ValidateCreateTransactionResponse(PaybyHttpResponse response)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!response.IsSuccess)
        stringBuilder.Append(response.HttpResponseCode.ToString() + ": " + response.Message + ". ");
      return stringBuilder.ToString();
    }

    public int? AuthorizationValidPeriod => new int?(30);

    public XmlDocument GetTransactionRequest(
      ProcessingInput aInputData,
      transactionTypeEnum transactionType)
    {
      string str1;
      string str2;
      string str3;
      if (transactionType != transactionTypeEnum.PURCHASE)
      {
        if (transactionType != transactionTypeEnum.REFUND)
          throw new PXException("Only PURCHASE and REFUND supported here, but got " + transactionType.ToString());
        str1 = this.ClientConfigAuthentication.recurClientId;
        str2 = this.ClientConfigAuthentication.refundTranCode;
        str3 = "Debit";
      }
      else
      {
        str1 = this.ClientConfigAuthentication.clientId;
        str2 = this.ClientConfigAuthentication.tranCode;
        str3 = "Debit";
      }
      XmlDocument transactionRequest = new XmlDocument();
      StringBuilder stringBuilder = new StringBuilder();
      ARPayment arPayment = (ARPayment) PXSelectBase<ARPayment, PXSelect<ARPayment, Where<ARPayment.docType, Equal<Required<ARPayment.docType>>, And<ARPayment.refNbr, Equal<Required<ARPayment.refNbr>>>>>.Config>.Select(new PXGraph(), (object) aInputData.DocumentData.DocType, (object) aInputData.DocumentData.DocRefNbr);
      if (arPayment == null)
        return (XmlDocument) null;
      PXResultset<CustomerPaymentMethodDetail> pxResultset = PXSelectBase<CustomerPaymentMethodDetail, PXSelectJoin<CustomerPaymentMethodDetail, InnerJoin<CustomerPaymentMethod, On<CustomerPaymentMethodDetail.pMInstanceID, Equal<CustomerPaymentMethod.pMInstanceID>>>, Where<CustomerPaymentMethod.isActive, Equal<Required<CustomerPaymentMethod.isActive>>, And<CustomerPaymentMethod.pMInstanceID, Equal<Required<CustomerPaymentMethod.pMInstanceID>>>>>.Config>.Select(new PXGraph(), (object) true, (object) arPayment.PMInstanceID);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      foreach (PXResult<CustomerPaymentMethodDetail, CustomerPaymentMethod> pxResult in pxResultset)
      {
        CustomerPaymentMethodDetail paymentMethodDetail = (CustomerPaymentMethodDetail) pxResult;
        if (paymentMethodDetail.DetailID == "ACCOUNT" || paymentMethodDetail.DetailID == "1")
          empty1 = paymentMethodDetail.Value;
        else if (paymentMethodDetail.DetailID == "TITLE" || paymentMethodDetail.DetailID == "2")
          empty3 = paymentMethodDetail.Value;
        else if (paymentMethodDetail.DetailID == "BSB" || paymentMethodDetail.DetailID == "3")
          empty2 = paymentMethodDetail.Value;
      }
      stringBuilder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ");
      stringBuilder.Append("xmlns:pay=\"http://paycorp.com.au/ns/payments1.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"> ");
      stringBuilder.Append("<soapenv:Header/>");
      stringBuilder.Append("<soapenv:Body>");
      stringBuilder.Append("<pay:Group Batch=\"true\">");
      stringBuilder.Append("<pay:Comment>TEST TRANSACTIONS</pay:Comment>");
      stringBuilder.Append("<pay:Network>live</pay:Network>");
      stringBuilder.Append("<pay:TimeoutSeconds>75</pay:TimeoutSeconds>");
      stringBuilder.Append("<pay:Transaction AccountTitle=\"" + empty3 + "\" Bsb=\"" + empty2 + "\" Account=\"" + empty1 + "\" LodgementRef=\"" + aInputData.DocumentData.DocRefNbr + "\" Amount=\"" + aInputData.Amount.ToString() + "\" ");
      stringBuilder.Append("xsi:type=\"ns1:" + str3 + "\" TransactionCode=\"" + str2 + "\" ClientId=\"" + str1 + "\" xmlns:ns1=\"http://paycorp.com.au/ns/payments1.0\">");
      stringBuilder.Append("</pay:Transaction>");
      stringBuilder.Append("</pay:Group>");
      stringBuilder.Append("</soapenv:Body>");
      stringBuilder.Append("</soapenv:Envelope>");
      transactionRequest.LoadXml(stringBuilder.ToString());
      return transactionRequest;
    }
  }
}
