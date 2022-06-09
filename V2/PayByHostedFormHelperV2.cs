// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByHostedFormHelperV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.component;
using gateway_client_csharp.au.com.gateway.client.payment;
using Microsoft.CSharp.RuntimeBinder;
using MYOB.PayBy.CCProcessing.Common;
using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;

namespace MYOB.PayBy.CCProcessing.V2
{
  public abstract class PayByHostedFormHelperV2 : PayByProcessorV2
  {
    private string _callbackUrl;

    protected PayByHostedFormHelperV2(IEnumerable<SettingsValue> setting)
      : base(setting)
    {
      this._settingValues = setting;
    }

    public string CallbackUrl
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(this._callbackUrl))
          return this._callbackUrl;
        string callbackUrl = string.Empty;
        Page currentHandler = HttpContext.Current.CurrentHandler as Page;
        if (HttpContext.Current != null && HttpContext.Current.Request.UrlReferrer != (Uri) null && currentHandler != null)
        {
          string str = currentHandler.ResolveUrl("~/Frames/PayByConnector.html");
          callbackUrl = HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Authority) + str;
        }
        return callbackUrl;
      }
      set => this._callbackUrl = value;
    }

    public (HostedFormData FormData, bool IsSuccess) GetHostedFormData(
      ProcessingInput input,
      IEnumerable<SettingsValue> settingsValues,
      int IframeType = 0,
      string curyid = "AUD")
    {
      PayByHttpRequest transactionRequest2 = new PayByHttpRequest();
      transactionRequest2.OperationType = operationEnum.PAYMENT_INIT;
      transactionRequest2.paybyClientConfig = this.ClientConfigAuthentication;
      int num = 0;
      int int32 = Convert.ToInt32(this.ClientConfigAuthentication.recurClientId);
      HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
      string leftPart = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
      string absolute = VirtualPathUtility.ToAbsolute("~/");
      string str1 = leftPart + absolute + "Content/paybyAmountHide.css";
      string str2 = leftPart + absolute + "Content/paybyMarginTop.css";
      string str3 = str1.Replace("http://", "https://");
      string str4 = str2.Replace("http://", "https://");
      string str5 = IframeType < 2 ? transactionTypeEnum.TOKEN.ToString() : (input.TranType == CCTranType.AuthorizeOnly ? transactionTypeEnum.TOKEN.ToString() : transactionTypeEnum.PURCHASE.ToString());
      string customerCd = input?.CustomerData?.CustomerCD;
      PayByHttpRequest payByHttpRequest = transactionRequest2;
      PaymentInitRequest paymentInitRequest = new PaymentInitRequest();
      paymentInitRequest.clientId = int32;
      paymentInitRequest.transactionType = str5;
      paymentInitRequest.tokenReference = customerCd;
      paymentInitRequest.transactionAmount = new TransactionAmount()
      {
        currency = input?.CuryID,
        paymentAmount = num
      };
      paymentInitRequest.redirect = new Redirect()
      {
        returnUrl = this.CallbackUrl,
        returnMethod = "GET"
      };
      paymentInitRequest.clientRef = customerCd;
      paymentInitRequest.tokenize = true;
      string str6;
      if (input != null)
      {
        switch (input.DocumentData?.DocRefNbr)
        {
          case null:
            break;
          default:
            str6 = input?.DocumentData?.DocRefNbr;
            goto label_4;
        }
      }
      str6 = customerCd;
label_4:
      paymentInitRequest.comment = str6;
      paymentInitRequest.extraData = new KeyValuePair<string, string>("CustomerID", customerCd);
      paymentInitRequest.useReliability = true;
      paymentInitRequest.cssLocation1 = str5 == transactionTypeEnum.PURCHASE.ToString() ? str4 : str3;
      payByHttpRequest.initRequest = paymentInitRequest;
      PXTrace.WriteInformation("Realtime Payby Request : " + transactionRequest2.initRequest?.ToString());
      PaymentInitResponse paymentInitResponse = this.Processor(transactionRequest2);
      if (paymentInitResponse != null && !string.IsNullOrWhiteSpace(paymentInitResponse?.paymentPageUrl))
      {
        if (IframeType == 0)
          return ((HostedFormData) null, true);
        ProfileServer.SavePaybyInitRequest(customerCd, settingsValues, paymentInitResponse.reqid);
        string paymentPageUrl = paymentInitResponse.paymentPageUrl;
        PXTrace.WriteInformation("Realtime Payby Response : " + paymentInitResponse?.ToString());
        return (new HostedFormData()
        {
          Token = (string) null,
          Caption = "Add Credit Card",
          Url = paymentPageUrl,
          Parameters = (Dictionary<string, string>) null,
          UseGetMethod = true
        }, true);
      }
      if (IframeType == 0)
        return ((HostedFormData) null, false);
            // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
            throw new PXException("Response Null");
    }

    private PaymentInitResponse Processor(PayByHttpRequest transactionRequest2) => this.ProcessInitResponse(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createInitController>(transactionRequest2, new createInitController(transactionRequest2)));

    private PaymentInitResponse ProcessInitResponse(PaybyHttpResponse response)
    {
      PaymentInitResponse paymentInitResponse1 = (PaymentInitResponse) null;
      if (!response.IsSuccess)
        return paymentInitResponse1;
      PaymentInitResponse paymentInitResponse2 = new PaymentInitResponse();
            // ISSUE: reference to a compiler-generated field
            /* if (PayByHostedFormHelperV2.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
             {
               // ISSUE: reference to a compiler-generated field
               PayByHostedFormHelperV2.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PaymentInitResponse>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (PaymentInitResponse), typeof (PayByHostedFormHelperV2)));
             }
             // ISSUE: reference to a compiler-generated field
             // ISSUE: reference to a compiler-generated field
             return PayByHostedFormHelperV2.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) PayByHostedFormHelperV2.\u003C\u003Eo__7.\u003C\u003Ep__0, response.Response);*/
            if (response.IsSuccess)
            {
                //completeResp = new PaymentCompleteResponse();
                paymentInitResponse2 = (PaymentInitResponse)response.Response;

            }
            return paymentInitResponse2;
        }
  }
}
