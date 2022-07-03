// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt.ProfileServer
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.Common;
using MYOB.PayBy.CCProcessing.V2;
using Newtonsoft.Json;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Web;

namespace MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt
{
    public static class ProfileServer
    {
        private static Dictionary<string, ProfileServer.PaybyInitReqPair> paybyInitRequestOnTheFly = new Dictionary<string, ProfileServer.PaybyInitReqPair>();
        private static Dictionary<string, ProfileServer.Transaction> customerTransactions = new Dictionary<string, ProfileServer.Transaction>();

        public static string CreateCustomerProfile(CustomerData customerData) => customerData.CustomerCD;

        public static bool DoesCustomerHavePendingRequest(string customerId)
        {
            PXTrace.WriteInformation("DoesCustomerHavePendingRequest on machine " + System.Environment.MachineName);
            if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(customerId.SessionIdSufix()))
                ProfileServer.syncSession();
            return ProfileServer.paybyInitRequestOnTheFly.ContainsKey(customerId.SessionIdSufix());
        }

        public static CreditCardData GetPaymentProfile(string paymentProfileId, string customerProfileId)
        {
            string requestIdKey = null;
            //if (paymentProfileId == null)
            //{
                using (PXDataRecord dataRecord =
                PXDatabase.SelectSingle<MAKLRequestCustomerPaymentMethod>(
                    new PXDataField("RequestID"),
                    new PXDataFieldValue("CustomerCCPID", customerProfileId),
                    new PXDataFieldValue("IsRequestTokenized", 0),
                    new PXDataFieldValue("IsActiveRequest", 1)))
                {
                    if (dataRecord != null)
                    {
                        requestIdKey = dataRecord.GetString(0);
                    }
                }
                paymentProfileId = $"{customerProfileId}{requestIdKey}";
          //  }

            paymentProfileId = $"{customerProfileId}{requestIdKey}";
            //if(ProfileServer.customerTransactions.ContainsKey(paymentProfileId))
            //{
            //    var transaction = ProfileServer.customerTransactions[paymentProfileId].TransactionId;
            //    response = 
            //}
            //var transactionID = 
            //PaymentCompleteResponse response = ProfileServer.GetTransaction(paymentProfileId, true);
            PaymentCompleteResponse response = ProfileServer.customerTransactions[paymentProfileId].paymentCompleteResponse;
            //if (response == null)
            //{
            //    string str = (string)null;
            //    foreach (string key in ProfileServer.paybyInitRequestOnTheFly.Keys)
            //    {
            //        if (key.EndsWith(HttpContext.Current.Session.SessionID))
            //        {
            //            str = key.Replace(HttpContext.Current.Session.SessionID, "");
            //            break;
            //        }
            //    }
            //    if (str != null)
            //    {
            //        ProfileServer.syncPaybyRequestsForCustomer(str);
            //        response = ProfileServer.GetCustomerTransaction(str);
            //    }
            //    if (response.token != paymentProfileId)
            //        // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
            //        throw new PXException("\r\nThe credit card number entered does not match the existing one. \r\nYou must re-enter the current credit card number in order to change any of the other details (your changes have not been saved). \r\nPlease try again, entering the original credit card number along with any other updates.\r\nNote: If you want to change the credit card number, you will need to create a new Payment Method.");
            //}
            return response.ToCreditCardData();
        }

        public static PaymentCompleteResponse GetTransaction(
          string transactionId,
          bool removeAfterGet = false)
        {
            PaymentCompleteResponse transaction = (PaymentCompleteResponse)null;
            string key1 = (string)null;
            foreach (string key2 in ProfileServer.customerTransactions.Keys)
            {
                if (ProfileServer.customerTransactions[key2].TransactionId == transactionId)
                {
                    transaction = ProfileServer.customerTransactions[key2].paymentCompleteResponse;
                    if (transaction.transactionType == "TOKEN")
                        transaction.responseCode = "00";
                    if (removeAfterGet || transaction.responseCode != "00")
                        key1 = key2;
                }
            }
            if (key1 != null)
                ProfileServer.customerTransactions.Remove(key1);
            return transaction;
        }

        public static PaymentCompleteResponse GetCustomerTransaction(
          string customerId)
        {
            return ProfileServer.customerTransactions[customerId]?.paymentCompleteResponse;
        }

        public static IEnumerable<CreditCardData> GetAllPaymentProfiles(
          string customerProfileId)
        {

            IEnumerable<SettingsValue> settingsValues = null;
            PXTrace.WriteInformation("GetAllPaymentProfiles on machine " + System.Environment.MachineName);
            string getMerchantSettings = null;
            string getRequestID = string.Empty;
            using (PXDataRecord dataRecord =
                PXDatabase.SelectSingle<MAKLRequestCustomerPaymentMethod>(
                    new PXDataField("Settings"),
                    new PXDataField("RequestID"),
                    new PXDataFieldValue("CustomerCCPID", customerProfileId),
                    new PXDataFieldValue("IsRequestTokenized", 0),
                    new PXDataFieldValue("IsActiveRequest", 1)))
            {
                if (dataRecord != null)
                {
                    getMerchantSettings = dataRecord.GetString(0);
                    getRequestID = dataRecord.GetString(1);
                }
            }
            if (getMerchantSettings != null)
            {
                settingsValues = JsonConvert.DeserializeObject<IEnumerable<SettingsValue>>(getMerchantSettings);
            }
            List<CreditCardData> allPaymentProfiles = new List<CreditCardData>();
            ProfileServer.syncPaybyRequestsForCustomer(customerProfileId, settingsValues, getRequestID);
            string key = $"{customerProfileId}{getRequestID}";
            PaymentCompleteResponse customerTransaction = ProfileServer.GetCustomerTransaction(key);
            allPaymentProfiles.Add(customerTransaction != null ? customerTransaction.ToCreditCardData() : (CreditCardData)null);
            return (IEnumerable<CreditCardData>)allPaymentProfiles;
        }

        private static void syncPaybyRequestsForCustomer(string customerProfileId, IEnumerable<SettingsValue> settingsValues = null, string getRequestID = null)
        {
            PXTrace.WriteInformation("syncPaybyRequestsForCustomer on machine " + System.Environment.MachineName);
            string transactionKey = $"{customerProfileId}{getRequestID}";//customerProfileId.SessionIdSufix();
            //if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(str))
            //  return;
            //ProfileServer.PaybyInitReqPair paybyInitReqPair = ProfileServer.paybyInitRequestOnTheFly[str];
            //if (paybyInitReqPair == null)
            //  return;
            PXTrace.WriteInformation("syncPaybyRequestsForCustomer, on the fly will be removed. " + transactionKey);
            //ProfileServer.paybyInitRequestOnTheFly.Remove(str);
            //ProfileServer.removeOnTheFlyFromSession(str);
            //PayByClientConfig clientConfig = PayByPluginHelper.GetClientConfig(paybyInitReqPair.SettingValues);
            PayByClientConfig clientConfig = PayByPluginHelper.GetClientConfig(settingsValues);
            PayByHttpRequest transactionRequest2 = new PayByHttpRequest()
            {
                OperationType = operationEnum.PAYMENT_COMPLETE,
                paybyClientConfig = clientConfig
            };
            transactionRequest2.completeRequest = new PaymentCompleteRequest()
            {
                clientId = Convert.ToInt32(transactionRequest2.paybyClientConfig.clientId),
                reqid = getRequestID
            };

            PaymentCompleteResponse completeResponse = new PayByCompleteFormProcessorV2(settingsValues).Processor(transactionRequest2);
            if (completeResponse == null || string.IsNullOrWhiteSpace(completeResponse.token))
                return;
            ProfileServer.customerTransactions[transactionKey] = new ProfileServer.Transaction()
            {
                TransactionId = completeResponse.token,
                paymentCompleteResponse = completeResponse
            };
        }

        public static void SavePaybyInitRequest(
          string customerProfileId,
          IEnumerable<SettingsValue> settingValues,
          string paybyInitRequestId)
        {
            PXTrace.WriteInformation("SavePaybyInitRequest on machine " + System.Environment.MachineName);
            ProfileServer.paybyInitRequestOnTheFly[customerProfileId.SessionIdSufix()] = new ProfileServer.PaybyInitReqPair()
            {
                SettingValues = settingValues,
                RequestId = paybyInitRequestId
            };
            ProfileServer.syncSession();
        }

        private static void syncSession()
        {
            Dictionary<string, ProfileServer.PaybyInitReqPair> dictionary = (Dictionary<string, ProfileServer.PaybyInitReqPair>)HttpContext.Current.Session["PhoenixPaybyProfileServer"];
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(key))
                        ProfileServer.paybyInitRequestOnTheFly[key] = dictionary[key];
                }
            }
            HttpContext.Current.Session["PhoenixPaybyProfileServer"] = (object)ProfileServer.paybyInitRequestOnTheFly;
        }

        private static void removeOnTheFlyFromSession(string customerProfileID)
        {
            Dictionary<string, ProfileServer.PaybyInitReqPair> dictionary = (Dictionary<string, ProfileServer.PaybyInitReqPair>)HttpContext.Current.Session["PhoenixPaybyProfileServer"];
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(key))
                        ProfileServer.paybyInitRequestOnTheFly[key] = dictionary[key];
                }
                ProfileServer.paybyInitRequestOnTheFly.Remove(customerProfileID);
            }
            HttpContext.Current.Session["PhoenixPaybyProfileServer"] = (object)ProfileServer.paybyInitRequestOnTheFly;
        }

        private static string SessionIdSufix(this string id)
        {
            string sessionId = HttpContext.Current.Session.SessionID;
            return id + sessionId;
        }

        private static CreditCardData ToCreditCardData(
          this PaymentCompleteResponse response)
        {
            return new CreditCardData()
            {
                CardNumber = response.creditCard.number,
                PaymentProfileID = response.token,
                CardExpirationDate = new DateTime?(PayByPluginHelper.Expiration(response.creditCard.expiry, out string _))
            };
        }

        private class PaybyInitReqPair
        {
            public IEnumerable<SettingsValue> SettingValues { get; set; }

            public string RequestId { get; set; }
        }

        private class Transaction
        {
            public string TransactionId { get; set; }

            public PaymentCompleteResponse paymentCompleteResponse { get; set; }
        }
    }
}
