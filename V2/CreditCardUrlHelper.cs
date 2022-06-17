using gateway_client_csharp.au.com.gateway.client.component;
using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.Common;
using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
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
                PXTrace.WriteInformation("Credit Card Url Response : " + initResponse?.ToString());
                return (new CreditCardUrlResponse()
                {
                    URL = initResponse.paymentPageUrl,
                    URLExpiryDate = initResponse.expireAt,
                    RequestID = initResponse.reqid
                }, true);
            }
            else {
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
    }
}
