using gateway_client_csharp.au.com.gateway.client.component;
using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.Common;
using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using Newtonsoft.Json;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Common;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace MYOB.PayBy.CCProcessing.V2
{
    public class CreditCardUrlHelper : PayByProcessorV2
    {
        private string _callbackUrl;

        protected CreditCardUrlHelper(IEnumerable<SettingsValue> setting)
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
                if (HttpContext.Current != null && HttpContext.Current.Request.UrlReferrer != (Uri)null && currentHandler != null)
                {
                    string str = currentHandler.ResolveUrl("~/Frames/PayByConnector.html");
                    callbackUrl = HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Authority) + str;
                }
                return callbackUrl;
            }
            set => this._callbackUrl = value;
        }

        public (CreditCardUrlResponse response, bool IsSuccess) GetUrlForCreditCard(
         ProcessingInput input,
         IEnumerable<SettingsValue> settingsValues,
         string curyid = "AUD")
        {
            PayByHttpRequest request = new PayByHttpRequest();
            request.OperationType = operationEnum.PAYMENT_INIT;
            request.paybyClientConfig = this.ClientConfigAuthentication;
            int payNowAmt = 0;
            int clientIdTmp = Convert.ToInt32(this.ClientConfigAuthentication.clientId);
            HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string absolute = VirtualPathUtility.ToAbsolute("~/");
            string paybyCssUrl = baseUrl + absolute + "Content/paybyAmountHide.css";
            paybyCssUrl = paybyCssUrl.Replace("http://", "https://");
            string transactionType = transactionTypeEnum.TOKEN.ToString();
            string customerCd = input?.CustomerData?.CustomerCD;

            request.initRequest = new PaymentInitRequest()
            {
                clientId = clientIdTmp,
                transactionType = transactionType,
                tokenReference = customerCd,
                transactionAmount = new TransactionAmount()
                {
                    currency = input?.CuryID,
                    paymentAmount = payNowAmt
                },
                redirect = new Redirect()
                {
                    returnUrl = CallbackUrl,
                    returnMethod = "GET"
                },
                clientRef = customerCd,
                tokenize = true,
                comment = input?.DocumentData?.DocRefNbr == null ? customerCd : input?.DocumentData?.DocRefNbr,
                extraData = new KeyValuePair<string, string>("CustomerID", customerCd),
                useReliability = true,
                cssLocation1 = paybyCssUrl

            };

            PaymentInitResponse initResponse = this.Processor(request);
            if (initResponse != null && !string.IsNullOrWhiteSpace(initResponse?.paymentPageUrl))
            {
                ProfileServer.SavePaybyInitRequest(customerCd, settingsValues, initResponse.reqid);
                string data = "string";
                PXTrace.WriteInformation("GetAllPaymentProfiles on machine " + System.Environment.MachineName);
                PXDatabase.Update<MAKLRequestCustomerPaymentMethod>(new PXDataFieldAssign("Settings", JsonConvert.SerializeObject(settingsValues)),
                                new PXDataFieldRestrict("RequestID", PXDbType.NVarChar, 255, initResponse.reqid, PXComp.EQ));
                //string key = customerProfileId.SessionIdSufix();
                //using (PXDataRecord settings =
                //    PXDatabase.SelectSingle<MAKLRequestCustomerPaymentMethod>(
                //        new PXDataField("Settings"),
                //        new PXDataFieldValue("", "")))
                //{
                //    if (settings != null)
                //    {
                //        data = settings.GetString(0);
                //    }
                //}
                PXTrace.WriteInformation("Credit Card Url Response : " + initResponse?.ToString());
                return (new CreditCardUrlResponse()
                {
                    URL = initResponse.paymentPageUrl,
                    URLExpiryDate = initResponse.expireAt,
                    RequestID = initResponse.reqid
                }, true);
            }
            else
            {
                return (null, false);
            }
            // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
            throw new PXException("Response Null");
        }

        /// <summary>
        /// Send request to PayBy and get the Payment Url in response, then this Url use for to open the hosted form
        /// </summary>
        /// <param name="transactionRequest2"></param>
        /// <returns></returns>
        PaymentInitResponse Processor(PayByHttpRequest transactionRequest2)
        {
            var response = this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createInitController>(transactionRequest2, new createInitController(transactionRequest2));
            PaymentInitResponse processingResult = ProcessInitResponse(response);
            return processingResult;
        }

        /// <summary>
        /// Process Init Response - Payment URL which is going to be open as hosted form
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        PaymentInitResponse ProcessInitResponse(PaybyHttpResponse response)
        {
            PaymentInitResponse initResp = null;
            if (response.IsSuccess)
            {
                initResp = new PaymentInitResponse();
                initResp = response.Response;
                return initResp;
            }
            return initResp;
        }


        public IEnumerable<CreditCardData> GetAllPaymentProfiles(
      string customerProfileId, IEnumerable<SettingsValue> settingsValues,string requestedId)
        {
            PXTrace.WriteInformation("GetAllPaymentProfiles on machine " + System.Environment.MachineName);
           
            //if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(key))
            //{
            //    ProfileServer.syncSession();
            //    if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(key))
            //        // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
            //        throw new PXException("\r\n\r\nA card with this number is already registered to this customer");
            //}
            List<CreditCardData> allPaymentProfiles = new List<CreditCardData>();
            var tes  = syncPaybyRequestsForCustomer(customerProfileId,settingsValues,requestedId);
            PaymentCompleteResponse customerTransaction = tes;//ProfileServer.GetCustomerTransaction(customerProfileId);
            allPaymentProfiles.Add(customerTransaction != null ? ToCreditCardData(customerTransaction) : (CreditCardData)null);
            return (IEnumerable<CreditCardData>)allPaymentProfiles;
            //return null;
        }

        private static PaymentCompleteResponse syncPaybyRequestsForCustomer(string customerProfileId,IEnumerable<SettingsValue> settingsValues,string requestedId)
        {
            PXTrace.WriteInformation("syncPaybyRequestsForCustomer on machine " + System.Environment.MachineName);
            //string str = customerProfileId.SessionIdSufix();
            //if (!ProfileServer.paybyInitRequestOnTheFly.ContainsKey(str))
            //    return;
            //ProfileServer.PaybyInitReqPair paybyInitReqPair = ProfileServer.paybyInitRequestOnTheFly[str];
            //if (paybyInitReqPair == null)
            //    return;
            //PXTrace.WriteInformation("syncPaybyRequestsForCustomer, on the fly will be removed. " + str);
            //ProfileServer.paybyInitRequestOnTheFly.Remove(str);
            //ProfileServer.removeOnTheFlyFromSession(str);
            PayByClientConfig clientConfig = PayByPluginHelper.GetClientConfig(settingsValues);
            PayByHttpRequest transactionRequest2 = new PayByHttpRequest()
            {
                OperationType = operationEnum.PAYMENT_COMPLETE,
                paybyClientConfig = clientConfig
            };
            transactionRequest2.completeRequest = new PaymentCompleteRequest()
            {
                clientId = Convert.ToInt32(transactionRequest2.paybyClientConfig.clientId),
                reqid = requestedId
            };


            PaymentCompleteResponse completeResponse = new PayByCompleteFormProcessorV2(settingsValues).Processor(transactionRequest2);
            if (completeResponse == null || string.IsNullOrWhiteSpace(completeResponse.token))
            {
                return null;
            }
            //ProfileServer.customerTransactions[str] = new ProfileServer.Transaction()
            //{
            //    TransactionId = completeResponse.token,
            //    paymentCompleteResponse = completeResponse
            //};
            return completeResponse;
        }

        //public static CreditCardData GetPaymentProfile(string paymentProfileId)
        //{
        //    PaymentCompleteResponse response = ProfileServer.GetTransaction(paymentProfileId, true);
        //    if (response == null)
        //    {
        //        string str = (string)null;
        //        foreach (string key in ProfileServer.paybyInitRequestOnTheFly.Keys)
        //        {
        //            if (key.EndsWith(HttpContext.Current.Session.SessionID))
        //            {
        //                str = key.Replace(HttpContext.Current.Session.SessionID, "");
        //                break;
        //            }
        //        }
        //        if (str != null)
        //        {
        //            ProfileServer.syncPaybyRequestsForCustomer(str);
        //            response = ProfileServer.GetCustomerTransaction(str);
        //        }
        //        if (response.token != paymentProfileId)
        //            // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
        //            throw new PXException("\r\nThe credit card number entered does not match the existing one. \r\nYou must re-enter the current credit card number in order to change any of the other details (your changes have not been saved). \r\nPlease try again, entering the original credit card number along with any other updates.\r\nNote: If you want to change the credit card number, you will need to create a new Payment Method.");
        //    }
        //    return response.ToCreditCardData();
        //}

        public static CreditCardData ToCreditCardData(PaymentCompleteResponse response)
        {
            return new CreditCardData()
            {
                CardNumber = response.creditCard.number,
                PaymentProfileID = response.token,
                CardExpirationDate = new DateTime?(PayByPluginHelper.Expiration(response.creditCard.expiry, out string _))
            };
        }

        //   private static CreditCardData ToCreditCardData(
        //this PaymentCompleteResponse response)
        //   {
        //       return new CreditCardData()
        //       {
        //           CardNumber = response.creditCard.number,
        //           PaymentProfileID = response.token,
        //           CardExpirationDate = new DateTime?(PayByPluginHelper.Expiration(response.creditCard.expiry, out string _))
        //       };
        //   }
    }
}
