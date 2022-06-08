// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByValidatorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MYOB.PayBy.CCProcessing.V2
{
  internal static class PayByValidatorV2
  {
    public static string ValidateClientId(string clientid) => string.IsNullOrWhiteSpace(clientid) ? "The Client ID cannot be empty" : string.Empty;

    public static string ValidateAuthToken(string authtoken) => string.IsNullOrWhiteSpace(authtoken) ? "The Auth Token cannot be empty" : string.Empty;

    public static string ValidateHMACKEY(string key)
    {
      if (string.IsNullOrWhiteSpace(key))
        return "The Hmac secret cannot be empty";
      return !key.All<char>(new Func<char, bool>(char.IsLetterOrDigit)) ? "The Hmac secret must contain alphanumeric characters only" : string.Empty;
    }

    public static string ValidateTRANCODE(string key)
    {
      if (string.IsNullOrWhiteSpace(key))
        return "The Transaction code cannot be empty";
      return !key.All<char>(new Func<char, bool>(char.IsDigit)) ? "The Transaction code must contain numeric value only" : string.Empty;
    }

    public static string ValidateValidationMode(string mode) => mode != EnvironmentMode.LiveMode.ToString() && mode != EnvironmentMode.TestMode.ToString() ? "The validation mode has to be LiveMode or TestMode" : string.Empty;

    public static string Validate(SettingsValue setting)
    {
      string str = string.Empty;
      if (setting == null)
        return "One or more of the following settings have not been specified: the auth token, the client id, the hmac secret. Make sure the values of the settings are entered";
      string detailId = setting.DetailID;
      if (detailId == "CLIENTID")
        str = PayByValidatorV2.ValidateClientId(setting.Value);
      else if (detailId == "CERTPASS")
        str = string.Empty;
      else if (detailId == "CURRENCY")
        str = string.IsNullOrWhiteSpace(setting.Value) ? "Currency value cannot be empty" : string.Empty;
      else if (detailId == "AUTHTOKEN")
        str = PayByValidatorV2.ValidateAuthToken(setting.Value);
      else if (detailId == "HMACKEY")
        str = PayByValidatorV2.ValidateHMACKEY(setting.Value);
      else if (detailId == "TRANCODE")
        str = PayByValidatorV2.ValidateTRANCODE(setting.Value);
      else if (detailId == "TESTMODE")
      {
        bool result;
        bool.TryParse(setting.Value, out result);
        if (result && !result)
          str = "The allowed values are 0 and 1 only";
      }
      else if (!(detailId == "ENV") && !(detailId == "RCLIENTID"))
      {
        int num = detailId == "DEBUGURL" ? 1 : 0;
      }
      return str;
    }

    public static string Validate(IEnumerable<SettingsValue> settingValues) => settingValues == null || !settingValues.Any<SettingsValue>() ? "One or more of the following settings have not been specified: the auth token,the client id, the hmac secret, Make sure the values of the settings are entered" : settingValues.Aggregate<SettingsValue, string>(string.Empty, (Func<string, SettingsValue, string>) ((current, sv) => current + PayByValidatorV2.Validate(sv)));

    internal static string Validate(CustomerData customerData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (customerData == null)
      {
        stringBuilder.AppendLine("The customer data cannot be defined");
        return stringBuilder.ToString();
      }
      if (string.IsNullOrWhiteSpace(customerData.CustomerCD))
        stringBuilder.AppendLine("The CustomerCD cannot be defined");
      return stringBuilder.Length != 0 ? stringBuilder.ToString() : string.Empty;
    }

    internal static string ValidateCustomerProfileId(string customerProfileId)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrWhiteSpace(customerProfileId) || !customerProfileId.All<char>(new Func<char, bool>(char.IsLetterOrDigit)))
        stringBuilder.AppendLine("The customer profile ID is not valid");
      return stringBuilder.Length != 0 ? stringBuilder.ToString() : string.Empty;
    }

    public static string Validate(CreditCardData cardData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (cardData == null)
      {
        stringBuilder.AppendLine("The credit card data cannot be defined");
        return stringBuilder.ToString();
      }
      if (string.IsNullOrWhiteSpace(cardData.CardNumber))
        stringBuilder.AppendLine("The card number is not correct.Enter a correct card number");
      DateTime? cardExpirationDate = cardData.CardExpirationDate;
      DateTime dateTime = new DateTime();
      if ((cardExpirationDate.HasValue ? (cardExpirationDate.HasValue ? (cardExpirationDate.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0) != 0)
        stringBuilder.AppendLine("The card expiration date is not correct.Enter a correct card expiration date");
      return stringBuilder.Length != 0 ? stringBuilder.ToString() : string.Empty;
    }

    public static string ValidateForTransaction(ProcessingInput processingInput)
    {
      if (processingInput == null)
        return "The data required for transaction cannot be defined";
      StringBuilder stringBuilder = new StringBuilder();
      if (processingInput.CustomerData == null)
        stringBuilder.AppendLine("The customer data cannot be defined");
      else if (string.IsNullOrWhiteSpace(processingInput.CustomerData.CustomerProfileID))
        stringBuilder.AppendLine("The customer profile ID cannot be defined");
      if (processingInput.CardData == null)
        stringBuilder.AppendLine("The credit card data cannot be defined");
      else if (string.IsNullOrWhiteSpace(processingInput.CardData.PaymentProfileID))
        stringBuilder.AppendLine("The payment profile ID cannot be defined");
      if (processingInput.Amount <= 0M)
        stringBuilder.AppendLine("The amount should be greater than zero");
      return stringBuilder.Length > 0 ? stringBuilder.ToString() : string.Empty;
    }

    public static string ValidateTranType(ProcessingInput processingInput)
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (processingInput.TranType)
      {
        case CCTranType.PriorAuthorizedCapture:
          if (string.IsNullOrWhiteSpace(processingInput.OrigTranID))
          {
            stringBuilder.AppendLine("The original transaction ID cannot be defined");
            break;
          }
          break;
        case CCTranType.CaptureOnly:
          if (string.IsNullOrWhiteSpace(processingInput.AuthCode) || processingInput.AuthCode.Length != 6)
          {
            stringBuilder.AppendLine("The authorization code must be equal to 6 characters");
            break;
          }
          break;
        case CCTranType.Credit:
          if (string.IsNullOrWhiteSpace(processingInput.OrigTranID))
          {
            stringBuilder.AppendLine("The original transaction ID cannot be defined");
            break;
          }
          break;
        case CCTranType.Void:
          if (string.IsNullOrWhiteSpace(processingInput.OrigTranID))
          {
            stringBuilder.AppendLine("The original transaction ID cannot be defined");
            break;
          }
          break;
        case CCTranType.VoidOrCredit:
          stringBuilder.AppendLine("This transaction type is not supported with the plug-in implementation");
          break;
      }
      return stringBuilder.ToString();
    }

    public static string ValidatePaymentProfileId(string paymentProfileId)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrWhiteSpace(paymentProfileId) || !paymentProfileId.All<char>(new Func<char, bool>(char.IsLetterOrDigit)))
        stringBuilder.AppendLine("The payment profile ID is not valid");
      return stringBuilder.Length > 0 ? stringBuilder.ToString() : string.Empty;
    }

    public static string AmountShouldNotbeZero() => throw new PXException("Amount should be greater than zero");

    public static string ValidateCreateTransactionResponse(PaybyHttpResponse response)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!response.IsSuccess)
        stringBuilder.Append(response.HttpResponseCode.ToString() + ": " + response.Message + ". ");
      return stringBuilder.ToString();
    }
  }
}
