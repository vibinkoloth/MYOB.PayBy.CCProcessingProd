// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByPluginHelper
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.component;
using gateway_client_csharp.au.com.gateway.client.config;
using gateway_client_csharp.au.com.gateway.client.payment;
using Microsoft.CSharp.RuntimeBinder;
using MYOB.PayBy.CCProcessing.PAYBY.DI;
using MYOB.PayBy.CCProcessing.V2;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using PX.Objects.AR.CCPaymentProcessing;
using PX.Objects.AR.CCPaymentProcessing.Interfaces;
using PX.Objects.CA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace MYOB.PayBy.CCProcessing.Common
{
  public static class PayByPluginHelper
  {
    private static Dictionary<string, SettingsDetail> DefaultSettings_Common => new Dictionary<string, SettingsDetail>()
    {
      ["CLIENTID"] = new SettingsDetail()
      {
        DetailID = "CLIENTID",
        Descr = "Specify client id of your Payby Account",
        DefaultValue = "",
        IsEncryptionRequired = new bool?(true)
      },
      ["RCLIENTID"] = new SettingsDetail()
      {
        DetailID = "RCLIENTID",
        Descr = "Specify recurring(CC)/refund(DD) client id of your Payby Account",
        DefaultValue = "",
        IsEncryptionRequired = new bool?(true)
      },
      ["ENV"] = new SettingsDetail()
      {
        DetailID = "ENV",
        Descr = "Sets payby environment",
        DefaultValue = "DEBUG",
        ControlType = SettingsControlType.Combo,
        ComboValues = new Dictionary<string, string>()
        {
          {
            "DEBUG",
            "DEBUG"
          },
          {
            "TEST",
            "TEST"
          },
          {
            "LIVE",
            "LIVE"
          }
        }
      },
      ["CURRENCY"] = new SettingsDetail()
      {
        DetailID = "CURRENCY",
        Descr = "Sets Currency for Payment Gateway",
        ControlType = SettingsControlType.Combo,
        DefaultValue = "AUD",
        ComboValues = new Dictionary<string, string>()
        {
          {
            "AUD",
            "AUD"
          },
          {
            "NZD",
            "NZD"
          }
        }
      }
    };

    private static Dictionary<string, SettingsDetail> DefaultSettings_CreditCard
    {
      get
      {
        Dictionary<string, SettingsDetail> strs = new Dictionary<string, SettingsDetail>()
        {
          ["AUTHTOKEN"] = new SettingsDetail()
          {
            DetailID = "AUTHTOKEN",
            Descr = "Specify Authentication key of your Payby Account",
            DefaultValue = "",
            IsEncryptionRequired = new bool?(true)
          },
          ["HMACKEY"] = new SettingsDetail()
          {
            DetailID = "HMACKEY",
            Descr = "Specify HMAC key of your Payby Account",
            DefaultValue = "",
            IsEncryptionRequired = new bool?(true)
          }
        };
        PayByPluginHelper.DefaultSettings_Common.ToList<KeyValuePair<string, SettingsDetail>>().ForEach((System.Action<KeyValuePair<string, SettingsDetail>>) (x => strs.Add(x.Key, x.Value)));
        return strs;
      }
    }

    public static Dictionary<string, SettingsDetail> DefaultSettings_DirectDebit
    {
      get
      {
        Dictionary<string, SettingsDetail> strs = new Dictionary<string, SettingsDetail>()
        {
          ["TRANCODE"] = new SettingsDetail()
          {
            DetailID = "TRANCODE",
            Descr = "Sets transaction code for Direct Debit",
            DefaultValue = "13",
            ControlType = SettingsControlType.Text
          },
          ["CERTPASS"] = new SettingsDetail()
          {
            DetailID = "CERTPASS",
            Descr = "Sets Certificate Password for DE Production only",
            DefaultValue = "",
            IsEncryptionRequired = new bool?(true)
          },
          ["REFUNDCODE"] = new SettingsDetail()
          {
            DetailID = "REFUNDCODE",
            Descr = "Sets transaction code for Direct Credit (refund)",
            DefaultValue = "50",
            ControlType = SettingsControlType.Text
          }
        };
        PayByPluginHelper.DefaultSettings_Common.ToList<KeyValuePair<string, SettingsDetail>>().ForEach((System.Action<KeyValuePair<string, SettingsDetail>>) (x => strs.Add(x.Key, x.Value)));
        return strs;
      }
    }

    public static IEnumerable<SettingsDetail> GetDefaultSettings_CreditCard() => (IEnumerable<SettingsDetail>) PayByPluginHelper.DefaultSettings_CreditCard.Values.ToList<SettingsDetail>();

    public static IEnumerable<SettingsDetail> GetDefaultSettings_DirectDebit() => (IEnumerable<SettingsDetail>) PayByPluginHelper.DefaultSettings_DirectDebit.Values.ToList<SettingsDetail>();

    internal static string GetResponseError(PaybyHttpResponse response)
    {
      if (response == null || response.Message == null)
        return "The response is empty.";
      if (response.IsSuccess)
        return string.Empty;
      // ISSUE: reference to a compiler-generated field
      if (PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (PayByPluginHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target = PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p1 = PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildMessagesString", (IEnumerable<System.Type>) null, typeof (PayByPluginHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) PayByPluginHelper.\u003C\u003Eo__8.\u003C\u003Ep__0, typeof (PayByPluginHelper), response.Response);
      return target((CallSite) p1, obj);
    }

    internal static string BuildMessagesString(string responseMessages)
    {
      if (responseMessages == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(responseMessages);
      return stringBuilder.ToString();
    }

    private static string BuildMessagesStringWithoutCodes(PaybyHttpResponse[] messages)
    {
      if (messages == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PaybyHttpResponse message in messages)
        stringBuilder.Append(message.Message + " ");
      return stringBuilder.ToString();
    }

    internal static T GetTestMode<T>(IEnumerable<T> settingValues, Func<T, bool> settingValues2) => settingValues.First<T>(settingValues2);

    public static PayByClientConfig GetClientConfig(
      IEnumerable<SettingsValue> settingValues)
    {
      string clientid = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CLIENTID"))?.Value;
      string str1 = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "RCLIENTID"))?.Value;
      string authtoken = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "AUTHTOKEN"))?.Value;
      string key = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "HMACKEY"))?.Value;
      string str2 = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "TRANCODE"))?.Value;
      string str3 = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "REFUNDCODE"))?.Value;
      string str4 = PayByValidatorV2.ValidateClientId(clientid) + PayByValidatorV2.ValidateAuthToken(authtoken) + PayByValidatorV2.ValidateHMACKEY(key);
      string str5 = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CERTPASS"))?.Value;
      string str6 = settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CURRENCY"))?.Value;
      EnvironmentV2.Environment testMode = PayByPluginHelper.GetTestMode(settingValues);
      return new PayByClientConfig()
      {
        certPassword = str5,
        clientId = clientid,
        recurClientId = str1,
        tranCode = str2,
        refundTranCode = str3,
        curyID = str6,
        clientConfig = new ClientConfig()
        {
          authToken = authtoken,
          hmacSecret = key
        },
        Environment = testMode
      };
    }

    public static PX.CCProcessingBase.Interfaces.V2.ProcessingResult ProcessTransactionResponseV2(
      PaybyHttpResponse response)
    {
      PX.CCProcessingBase.Interfaces.V2.ProcessingResult processingResult = new PX.CCProcessingBase.Interfaces.V2.ProcessingResult();
      if (!response.IsSuccess)
        return (PX.CCProcessingBase.Interfaces.V2.ProcessingResult) null;
      // ISSUE: reference to a compiler-generated field
      if (PayByPluginHelper.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PayByPluginHelper.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PaymentRealTimeResponse>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (PaymentRealTimeResponse), typeof (PayByPluginHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PaymentRealTimeResponse realTimeResponse = PayByPluginHelper.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) PayByPluginHelper.\u003C\u003Eo__13.\u003C\u003Ep__0, response.Response);
      processingResult.AuthorizationNbr = realTimeResponse.authCode;
      processingResult.ResponseCode = realTimeResponse.responseCode;
      processingResult.ResponseReasonText = realTimeResponse.responseText;
      processingResult.TransactionNumber = realTimeResponse.txnReference;
      return processingResult;
    }

    public static PX.CCProcessingBase.Interfaces.V2.ProcessingResult ProcessBatchResponseV2(
      PaybyHttpResponse response)
    {
      PX.CCProcessingBase.Interfaces.V2.ProcessingResult processingResult = new PX.CCProcessingBase.Interfaces.V2.ProcessingResult();
      if (!response.IsSuccess)
        return (PX.CCProcessingBase.Interfaces.V2.ProcessingResult) null;
      // ISSUE: reference to a compiler-generated field
      if (PayByPluginHelper.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PayByPluginHelper.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PayByResponseObj>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (PayByResponseObj), typeof (PayByPluginHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PayByResponseObj payByResponseObj = PayByPluginHelper.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) PayByPluginHelper.\u003C\u003Eo__14.\u003C\u003Ep__0, response.Response);
      processingResult.TransactionNumber = payByResponseObj.getResponseValue("GroupId");
      return processingResult;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp) => source != null && source.IndexOf(toCheck, comp) > 0;

    public static PX.CCProcessingBase.Interfaces.V2.ProcessingResult ProcessReportResponseV2(
      PaybyHttpResponse response)
    {
      PX.CCProcessingBase.Interfaces.V2.ProcessingResult processingResult = new PX.CCProcessingBase.Interfaces.V2.ProcessingResult();
      if (!response.IsSuccess)
        return (PX.CCProcessingBase.Interfaces.V2.ProcessingResult) null;
      // ISSUE: reference to a compiler-generated field
      if (PayByPluginHelper.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PayByPluginHelper.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PayByResponseObj>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (PayByResponseObj), typeof (PayByPluginHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PayByResponseObj payByResponseObj = PayByPluginHelper.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) PayByPluginHelper.\u003C\u003Eo__16.\u003C\u003Ep__0, response.Response);
      if (payByResponseObj.responseList.Where<PayByResponse>((Func<PayByResponse, bool>) (o => o.Key == "Error")).Any<PayByResponse>())
      {
        processingResult.ResponseCode = PayByPluginHelper.DirectEntryStatus("S").DirectEntry_Code;
        processingResult.ResponseReasonText = payByResponseObj.getResponseValue("Error");
        processingResult.ResponseText = payByResponseObj.getResponseValue("Error");
        processingResult.AuthorizationNbr = "-500";
        processingResult.TransactionNumber = "-500";
      }
      else
      {
        processingResult.AuthorizationNbr = payByResponseObj.getResponseValue("LodgementRef");
        processingResult.ResponseCode = PayByPluginHelper.DirectEntryStatus(payByResponseObj.getResponseValue("Status")).DirectEntry_Code;
        processingResult.ResponseReasonText = PayByPluginHelper.DirectEntryStatus(payByResponseObj.getResponseValue("Status")).DirectEntry_Descr;
        if (processingResult.ResponseCode == "ERR")
          throw new Exception(((PX.CCProcessingBase.Interfaces.V2.ProcessingResult) null).ResponseReasonText);
        processingResult.TransactionNumber = payByResponseObj.getResponseValue("TxnReference");
        processingResult.ResponseReasonCode = payByResponseObj.getResponseValue("BatchId");
        processingResult.ResponseText = payByResponseObj.getResponseValue("Status");
      }
      return processingResult;
    }

    public static bool IsAuthorized(string responsecode) => responsecode == "00";

    public static bool IsDDAuthorized(string responsecode)
    {
      bool flag;
      switch (responsecode)
      {
        case "E":
        case "G":
        case "HFR":
        case "L":
        case "R":
        case "S":
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }

    public static PX.CCProcessingBase.CCTranStatus InterpretResponseCode(
      string aPcResponseCode)
    {
      return aPcResponseCode == "00" ? PX.CCProcessingBase.CCTranStatus.Approved : PX.CCProcessingBase.CCTranStatus.Unknown;
    }

    public static PX.CCProcessingBase.CCTranStatus DDInterpretResponseCode(
      string aPcResponseCode)
    {
      switch (aPcResponseCode)
      {
        case "E":
        case "G":
        case "HFR":
        case "L":
        case "R":
        case "S":
          return PX.CCProcessingBase.CCTranStatus.Approved;
        case "V":
          return PX.CCProcessingBase.CCTranStatus.Declined;
        default:
          return PX.CCProcessingBase.CCTranStatus.Unknown;
      }
    }

    public static string Last4DigitsOfCardNumber(string cardNumber) => string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length <= 4 ? string.Empty : cardNumber.Substring(cardNumber.Length - 4, 4);

    public static int ToPayByAmount(Decimal? amount)
    {
      if (!amount.HasValue)
        return 0;
      Decimal? nullable = amount;
      Decimal num = (Decimal) 100;
      return (int) (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() * num) : new Decimal?()).Value;
    }

    public static Decimal ToMYOBAmount(Decimal? amount)
    {
      if (!amount.HasValue)
        return 0M;
      Decimal? nullable = amount;
      Decimal num = (Decimal) 100;
      return (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() / num) : new Decimal?()).Value;
    }

    public static DateTime Expiration(string CardExpiration, out string ExpirationDate)
    {
      ExpirationDate = string.Empty;
      if (string.IsNullOrEmpty(CardExpiration) || CardExpiration.Length == 0)
        return new DateTime(1900, 1, 1);
      string str1 = CardExpiration?.Substring(0, 2);
      int int32_1 = Convert.ToInt32(str1);
      string str2 = DateTime.Now.Year.ToString().Substring(0, 2) + CardExpiration?.Substring(CardExpiration.Length - 2);
      int int32_2 = Convert.ToInt32(str2);
      ExpirationDate = str2 + str1;
      DateTime dateTime = new DateTime(int32_2, int32_1, 1);
      ExpirationDate = str1 + CardExpiration?.Substring(CardExpiration.Length - 2);
      return dateTime;
    }

    public static (string DirectEntry_Code, string DirectEntry_Status, string DirectEntry_Descr) DirectEntryStatus(
      string aDDStatus)
    {
      if (aDDStatus == "S")
        return ("HFR", "Held for Review", "Transaction has been submitted to the PPS and is awaiting transmission to your financial institution");
      if (aDDStatus == "L")
        return ("HFR", "Held for Review", "Transaction has been submitted to your financial institution, but no validation has been undertaken");
      if (aDDStatus == "R")
        return ("CAN", "Failed", "Transaction has been submitted to your financial institution and has passed initial validation, but has subsequently been rejected by the customer’s bank");
      if (aDDStatus == "E")
        return ("CAN", "Failed", "Transaction has been submitted to your financial institution and has failed initial validation");
      if (aDDStatus == "G")
        return ("APR", "Pass", "Transaction has been submitted to your financial institution, there are currently no errors, but the transaction could be rejected in the coming days");
      if (aDDStatus == "V")
        return ("CAN", "Cancelled", "Transaction cancelled.This has not been sent to the Bank");
      return aDDStatus == "HFR" ? ("HFR", "Held for Review", "Transaction has been submitted to the PPS and is awaiting transmission to your financial institution") : ("FAL", "Unknown Error", "Transaction has an error.This has not been sent to the Bank");
    }

    public static string CCTranStatusCode2(string aDDStatus)
    {
      if (aDDStatus == "HFR")
        return "HFR";
      if (aDDStatus == "FAL")
        return "ERR";
      if (aDDStatus == "APR")
        return "APR";
      return aDDStatus == "CAN" ? "ERR" : (string) null;
    }

    public static PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus InterpretCVVResponse(
      string aPCCVVResponseCode)
    {
      if (aPCCVVResponseCode == "M")
        return PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.Match;
      if (aPCCVVResponseCode == "N")
        return PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.NotMatch;
      if (aPCCVVResponseCode == "P")
        return PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.NotProcessed;
      if (aPCCVVResponseCode == "S")
        return PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.ShouldHaveBeenPresent;
      return !(aPCCVVResponseCode == "U") ? PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.Unknown : PX.CCProcessingBase.Interfaces.V2.CcvVerificationStatus.IssuerUnableToProcessRequest;
    }

    public static int? AuthorizationValidPeriod => new int?(30);

    public static XmlDocument GetTransactionReport(string BatchId, string clientId)
    {
      XmlDocument transactionReport = new XmlDocument();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ");
      stringBuilder.Append("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instancee\"><soapenv:Body>");
      stringBuilder.Append("<ReportRequest Network=\"live\" xmlns=\"http://paycorp.com.au/ns/reports1.2.1\">");
      stringBuilder.Append("<ClientId>" + clientId + "</ClientId><BatchId>" + BatchId + "</BatchId></ReportRequest>");
      stringBuilder.Append("</soapenv:Body>");
      stringBuilder.Append("</soapenv:Envelope>");
      transactionReport.LoadXml(stringBuilder.ToString());
      return transactionReport;
    }

    public static PaymentRealTimeRequest GetTransactionRequest(
      PX.CCProcessingBase.Interfaces.V2.ProcessingInput aInputData,
      string clientId,
      string transType,
      string curyId)
    {
      CustomerData customerData = aInputData.CustomerData;
      CreditCardData cardData1 = aInputData.CardData;
      DocumentData documentData = aInputData.DocumentData;
      CreditCardData cardData2 = new MACreditCardData().GetCardData(aInputData);
      PaymentRealTimeRequest transactionRequest = new PaymentRealTimeRequest();
      transactionRequest.clientId = Convert.ToInt32(clientId);
      transactionRequest.clientRef = customerData?.CustomerCD;
      transactionRequest.comment = documentData?.DocRefNbr;
      transactionRequest.transactionType = transType;
      CreditCard creditCard = new CreditCard();
      creditCard.number = cardData1.PaymentProfileID;
      creditCard.holderName = customerData.CustomerName;
      DateTime? cardExpirationDate = cardData2.CardExpirationDate;
      string str;
      if (cardExpirationDate.HasValue)
      {
        cardExpirationDate = cardData2.CardExpirationDate;
        if (cardExpirationDate.HasValue)
        {
          cardExpirationDate = cardData2.CardExpirationDate;
          str = cardExpirationDate.Value.ToString("MMyy");
          goto label_4;
        }
      }
      str = (string) null;
label_4:
      creditCard.expiry = str;
      creditCard.secureId = (string) null;
      creditCard.secureIdSupplied = true;
      transactionRequest.creditCard = creditCard;
      transactionRequest.originalTxnReference = !string.IsNullOrEmpty(aInputData.OrigTranID) ? aInputData.OrigTranID : (string) null;
      transactionRequest.transactionAmount = new TransactionAmount()
      {
        paymentAmount = aInputData != null ? PayByPluginHelper.ToPayByAmount(new Decimal?(aInputData.Amount)) : 0,
        currency = curyId
      };
      return transactionRequest;
    }

    public static PayByClientConfig GetClientConfig(string aProcessingCenterID) => PayByPluginHelper.GetClientConfig(PayByPluginHelper.GetSettingsValue(aProcessingCenterID));

    public static IEnumerable<SettingsValue> GetSettingsValue(
      string processingCenterID)
    {
      List<SettingsValue> settingsValue = new List<SettingsValue>();
      foreach (PXResult<CCProcessingCenter, CCProcessingCenterDetail> pxResult in PXSelectBase<CCProcessingCenter, PXSelectJoin<CCProcessingCenter, InnerJoin<CCProcessingCenterDetail, On<CCProcessingCenter.processingCenterID, Equal<CCProcessingCenterDetail.processingCenterID>>>, Where<CCProcessingCenter.processingCenterID, Equal<Required<CCProcessingCenter.processingCenterID>>, And<CCProcessingCenter.isActive, Equal<True>>>>.Config>.Select(new PXGraph(), (object) processingCenterID))
      {
        CCProcessingCenterDetail processingCenterDetail = (CCProcessingCenterDetail) pxResult;
        settingsValue.Add(new SettingsValue()
        {
          DetailID = processingCenterDetail.DetailID,
          Value = processingCenterDetail.Value
        });
      }
      return (IEnumerable<SettingsValue>) settingsValue;
    }

    internal static EnvironmentV2.Environment GetTestMode(
      IEnumerable<SettingsValue> settingValues)
    {
      string str = settingValues.First<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "ENV")).Value;
      if (str == "DEBUG")
        return EnvironmentV2.Environment.DEBUG;
      return str == "TEST" || !(str == "LIVE") ? EnvironmentV2.Environment.TEST : EnvironmentV2.Environment.LIVE;
    }

    public static PX.CCProcessingBase.Interfaces.V2.CCTranStatus TransactionStatus(
      string authCode)
    {
      if (authCode == "00")
        return PX.CCProcessingBase.Interfaces.V2.CCTranStatus.Approved;
      return authCode != "00" ? PX.CCProcessingBase.Interfaces.V2.CCTranStatus.Declined : PX.CCProcessingBase.Interfaces.V2.CCTranStatus.Error;
    }

    public static PX.CCProcessingBase.Interfaces.V2.CCTranType TransactionType(
      string paybyTran)
    {
      if (paybyTran == transactionTypeEnum.AUTHORISATION.ToString() || paybyTran == transactionTypeEnum.TOKEN.ToString())
        return PX.CCProcessingBase.Interfaces.V2.CCTranType.AuthorizeOnly;
      if (paybyTran == transactionTypeEnum.PURCHASE.ToString())
        return PX.CCProcessingBase.Interfaces.V2.CCTranType.AuthorizeAndCapture;
      if (paybyTran == transactionTypeEnum.COMPLETION.ToString())
        return PX.CCProcessingBase.Interfaces.V2.CCTranType.PriorAuthorizedCapture;
      return paybyTran == transactionTypeEnum.REVERSAL.ToString() ? PX.CCProcessingBase.Interfaces.V2.CCTranType.Void : PX.CCProcessingBase.Interfaces.V2.CCTranType.CaptureOnly;
    }

    public class PaybyCCPaymentTransactionAdapter : ICCPaymentTransactionAdapter
    {
      public ICCPaymentTransaction Current => throw new NotImplementedException();

      public PXCache Cache => throw new NotImplementedException();

      public IEnumerable<ICCPaymentTransaction> Select(
        params object[] arguments)
      {
        return (IEnumerable<ICCPaymentTransaction>) new List<ICCPaymentTransaction>();
      }
    }
  }
}
