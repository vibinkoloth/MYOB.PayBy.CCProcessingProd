// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByHttpUtility
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client;
using gateway_client_csharp.au.com.gateway.client.config;
using gateway_client_csharp.au.com.gateway.client.payment;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace MYOB.PayBy.CCProcessing.Common
{
  public static class PayByHttpUtility
  {
    private static ClientConfig config;

    private static Uri GetPostUrl(EnvironmentV2 env) => new Uri(env.getBaseUrl() + "/rest/service/proxy");

    private static GatewayClient CallPayByGateway()
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      return new GatewayClient(PayByHttpUtility.config);
    }

    public static PaybyHttpResponse CallPayByApi<TQ, TS>(
      EnvironmentV2 env,
      PayByHttpRequest paybyHttpRequest)
      where TQ : PayByHttpRequest
      where TS : PaybyHttpResponse
    {
      PaybyHttpResponse response = new PaybyHttpResponse();
      if (paybyHttpRequest.OperationType == operationEnum.PAYMENT_BATCH)
        return PayBySoapUtility.CallPayByApi<PayByHttpRequest, PaybyHttpResponse>(env, paybyHttpRequest);
      PayByHttpUtility.config = paybyHttpRequest.paybyClientConfig.clientConfig;
      Uri postUrl = PayByHttpUtility.GetPostUrl(env);
      PayByHttpUtility.config.serviceEndpoint = postUrl.AbsoluteUri;
      GatewayClient gatewayClient = PayByHttpUtility.CallPayByGateway();
      try
      {
        if (paybyHttpRequest.OperationType == operationEnum.PAYMENT_INIT)
        {
          PaymentInitResponse paymentInitResponse = gatewayClient.payment.init(paybyHttpRequest.initRequest);
          response.Response = (object) paymentInitResponse;
        }
        else if (paybyHttpRequest.OperationType == operationEnum.PAYMENT_COMPLETE)
        {
          PaymentCompleteResponse completeResponse = gatewayClient.payment.complete(paybyHttpRequest.completeRequest);
          response.Response = (object) completeResponse;
        }
        else if (paybyHttpRequest.OperationType == operationEnum.PAYMENT_REAL_TIME)
        {
          PaymentRealTimeResponse realTimeResponse = gatewayClient.payment.realTime(paybyHttpRequest.realTimeRequest);
          response.Response = (object) realTimeResponse;
        }
        response = PayByHttpUtility.PayByResponse(ref response);
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
        response.HttpResponseCode = messageTypeEnum.Error;
        response.Message = ex.Message;
        response.Response = (object) ex.Message;
      }
      return response;
    }

    private static PaybyHttpResponse PayByResponse(ref PaybyHttpResponse response)
    {
            /*if (response.Response is PaymentRealTimeResponse)
            {
              // ISSUE: reference to a compiler-generated field
              if (PayByHttpUtility..\u003C\u003Ep__0 == null)
              {
                // ISSUE: reference to a compiler-generated field
                PayByHttpUtility.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PaymentRealTimeResponse>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (PaymentRealTimeResponse), typeof (PayByHttpUtility)));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PaymentRealTimeResponse realTimeResponse = PayByHttpUtility.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) PayByHttpUtility.\u003C\u003Eo__4.\u003C\u003Ep__0, response.Response);
              return PayByHttpUtility.ReponseMethod(realTimeResponse.responseCode, realTimeResponse.responseText, ref response);
            }
            response.IsSuccess = true;
            response.HttpResponseCode = messageTypeEnum.Ok;
            response.Message = "Success";
            return response;*/
            if (response.Response is PaymentRealTimeResponse)
            {
                PaymentRealTimeResponse obj = (PaymentRealTimeResponse)response.Response;
                return ReponseMethod(obj.responseCode, obj.responseText, ref response);
            }
            else // (response.Response is PaymentInitResponse)
            {
                response.IsSuccess = true;
                response.HttpResponseCode = messageTypeEnum.Ok;
                response.Message = "Success";
            }

            return response;
        }

    private static PaybyHttpResponse ReponseMethod(
      string responseCode,
      string responseText,
      ref PaybyHttpResponse response)
    {
      if (responseCode == "0" || responseCode == "00" || responseCode == "000" || responseCode == "08")
      {
        response.IsSuccess = true;
        response.HttpResponseCode = messageTypeEnum.Ok;
        response.Message = responseText;
        return response;
      }
      response.IsSuccess = false;
      response.HttpResponseCode = messageTypeEnum.Error;
      response.Message = responseCode + " : " + responseText;
      response.Response = (object) (responseCode + " : " + responseText);
      return response;
    }
  }
}
