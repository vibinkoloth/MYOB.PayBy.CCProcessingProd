// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayBySoapUtility
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.config;
using MYOB.PayBy.CCProcessing.PAYBY.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

namespace MYOB.PayBy.CCProcessing.Common
{
  public static class PayBySoapUtility
  {
    private static ClientConfig config;

    private static Uri GetPostUrl(EnvironmentV2 env) => new Uri(env.getSoapBaseUrl() + "/wsi/services/Payments");

    public static PaybyHttpResponse CallPayByApi<TQ, TS>(
      EnvironmentV2 env,
      PayByHttpRequest paybyHttpRequest)
      where TQ : PayByHttpRequest
      where TS : PaybyHttpResponse
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertificateHolder.ValidateServerCertficate);
      Task<PaybyHttpResponse> soap = PayBySoapUtility.getSOAP(env, paybyHttpRequest);
      Task.WaitAll((Task) soap);
      return soap.Result;
    }

    private static async Task<PaybyHttpResponse> getSOAP(
      EnvironmentV2 env,
      PayByHttpRequest paybyHttpRequest)
    {
      PaybyHttpResponse paybyHttpResponse = new PaybyHttpResponse();
      PayByResponseObj payByResponseObj = new PayByResponseObj();
      PayBySoapUtility.config = paybyHttpRequest.paybyClientConfig.clientConfig;
      Uri postUrl = PayBySoapUtility.GetPostUrl(env);
      PayBySoapUtility.config.serviceEndpoint = postUrl.AbsoluteUri;
      try
      {
        HttpWebRequest webRequest = PayBySoapUtility.CreateWebRequest(paybyHttpRequest.paybyClientConfig);
        PayBySoapUtility.InsertSoapEnvelopeIntoWebRequest(paybyHttpRequest.soapRequest.soapEnvelopeXml, webRequest);
        webRequest.BeginGetResponse((AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne();
        List<PayByResponse> payByResponseList = new List<PayByResponse>();
        using (WebResponse responseAsync = await webRequest.GetResponseAsync())
        {
          using (StreamReader streamReader = new StreamReader(responseAsync.GetResponseStream()))
          {
            string end = streamReader.ReadToEnd();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(end);
            if (xmlDocument == null)
              return (PaybyHttpResponse) null;
            paybyHttpResponse.Message = SecurityElement.Escape(xmlDocument.InnerXml);
            if (paybyHttpRequest.transactionType == transactionTypeEnum.REPORT)
            {
              XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("DirectEntry");
              paybyHttpResponse.Response = (object) PayBySoapUtility.soapResponse(elementsByTagName);
              if (elementsByTagName.Count == 0)
                paybyHttpResponse.Response = (object) PayBySoapUtility.soapResponse(xmlDocument.GetElementsByTagName("Error"));
            }
            else
              paybyHttpResponse.Response = (object) PayBySoapUtility.soapResponse(xmlDocument.GetElementsByTagName("GroupId"));
          }
        }
        if (paybyHttpResponse != null)
        {
          paybyHttpResponse.IsSuccess = true;
          paybyHttpResponse.HttpResponseCode = messageTypeEnum.Ok;
        }
        else
        {
          paybyHttpResponse.IsSuccess = true;
          paybyHttpResponse.HttpResponseCode = messageTypeEnum.Error;
          paybyHttpResponse.Message = "Null Response";
          paybyHttpResponse.Response = (object) null;
        }
      }
      catch (Exception ex)
      {
        paybyHttpResponse.IsSuccess = false;
        paybyHttpResponse.HttpResponseCode = messageTypeEnum.Error;
        paybyHttpResponse.Message = ex.Message;
        paybyHttpResponse.Response = (object) ex.Message;
      }
      return paybyHttpResponse;
    }

    private static void InsertSoapEnvelopeIntoWebRequest(
      XmlDocument soapEnvelopeXml,
      HttpWebRequest webRequest)
    {
      using (Stream requestStream = webRequest.GetRequestStream())
        soapEnvelopeXml.Save(requestStream);
    }

    private static HttpWebRequest CreateWebRequest(PayByClientConfig pbClientConfig)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(pbClientConfig.clientConfig.serviceEndpoint);
      if (pbClientConfig.Environment == EnvironmentV2.Environment.LIVE && string.IsNullOrWhiteSpace(pbClientConfig.certPassword))
        throw new Exception("Certificate Password cannot be empty for Production environment");
      if (pbClientConfig.Environment == EnvironmentV2.Environment.LIVE)
      {
        string fileName = HostingEnvironment.MapPath("~/Content/0337.p12");
        webRequest.ClientCertificates.Add((X509Certificate) new X509Certificate2(fileName, pbClientConfig.certPassword));
      }
      webRequest.Headers.Add("SOAPAction", pbClientConfig.clientConfig.serviceEndpoint);
      webRequest.ContentType = "text/xml;charset=\"utf-8\"";
      webRequest.Accept = "text/xml";
      webRequest.Method = "POST";
      return webRequest;
    }

    private static PayByResponseObj soapResponse(XmlNodeList node)
    {
      PayByResponseObj payByResponseObj = new PayByResponseObj();
      foreach (XmlNode xmlNode in node)
      {
        if (xmlNode.Attributes.Count > 0)
        {
          PayByResponse payByResponse = new PayByResponse();
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
            payByResponseObj.addResponseValue(attribute.Name, attribute.InnerText);
        }
        else
          payByResponseObj.addResponseValue(xmlNode.Name, xmlNode.InnerText);
      }
      return payByResponseObj;
    }
  }
}
