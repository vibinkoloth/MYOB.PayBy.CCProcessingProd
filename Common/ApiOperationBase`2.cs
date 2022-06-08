// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.ApiOperationBase`2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.config;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MYOB.PayBy.CCProcessing.Common
{
  public abstract class ApiOperationBase<TQ, TS> : IApiOperation<TQ, TS>
    where TQ : PayByHttpRequest
    where TS : PaybyHttpResponse
  {
    private TQ _apiRequest;
    private TS _apiResponse;
    private readonly Type _requestClass;
    private readonly Type _responseClass;
    private PaybyHttpResponse _errorResponse;
    private const string NullEnvironmentErrorMessage = "Environment not set. Set environment using setter or use overloaded method to pass appropriate environment";
    protected List<string> Results;
    protected messageTypeEnum ResultCode;

    public static EnvironmentV2 RunEnvironment { get; set; }

    public static ClientConfig MerchantAuthentication { get; set; }

    protected ApiOperationBase(TQ apiRequest)
    {
      if ((object) apiRequest == null)
        throw new ArgumentNullException(nameof (apiRequest), "Input request cannot be null");
      if ((object) this.GetApiResponse() != null)
        throw new InvalidOperationException("Response should be null");
      this._requestClass = typeof (TQ);
      this._responseClass = typeof (TS);
      this.SetApiRequest(apiRequest);
      this.Validate();
    }

    protected TQ GetApiRequest() => this._apiRequest;

    protected void SetApiRequest(TQ apiRequest) => this._apiRequest = apiRequest;

    public TS GetApiResponse() => this._apiResponse;

    private void SetApiResponse(TS apiResponse) => this._apiResponse = apiResponse;

    public PaybyHttpResponse GetErrorResponse() => this._errorResponse;

    private void SetErrorResponse(PaybyHttpResponse errorResponse) => this._errorResponse = errorResponse;

    public TS ExecuteWithApiResponse(EnvironmentV2 environment = null)
    {
      this.Execute(environment);
      return this.GetApiResponse();
    }

    public void Execute(EnvironmentV2 environment = null)
    {
      this.BeforeExecute();
      if (environment == null)
        environment = ApiOperationBase<PayByHttpRequest, PaybyHttpResponse>.RunEnvironment;
      PaybyHttpResponse paybyHttpResponse = environment != null ? PayByHttpUtility.CallPayByApi<TQ, TS>(environment, (PayByHttpRequest) this.GetApiRequest()) : throw new ArgumentException("Environment not set. Set environment using setter or use overloaded method to pass appropriate environment");
      if (paybyHttpResponse != null)
      {
        if (!paybyHttpResponse.IsSuccess)
          this.SetErrorResponse(paybyHttpResponse);
        else if (paybyHttpResponse.GetType() == this._responseClass)
          this.SetApiResponse((TS) paybyHttpResponse);
        else
          this.SetErrorResponse(paybyHttpResponse);
        this.SetResultStatus();
      }
      else
        this.AfterExecute();
    }

    public messageTypeEnum GetResultCode() => this.ResultCode;

    private void SetResultStatus()
    {
      this.Results = new List<string>();
      PaybyHttpResponse resultMessage = this.GetResultMessage();
      if (resultMessage != null)
        this.ResultCode = resultMessage.HttpResponseCode;
      if (resultMessage == null)
        return;
      this.Results.Add(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}:{1}", (object) resultMessage.HttpResponseCode, (object) resultMessage.Message));
    }

    public List<string> GetResults() => this.Results;

    private PaybyHttpResponse GetResultMessage()
    {
      PaybyHttpResponse resultMessage = (PaybyHttpResponse) null;
      if (this.GetErrorResponse() != null)
        resultMessage = this.GetErrorResponse();
      else if ((object) this.GetApiResponse() != null)
        resultMessage = (PaybyHttpResponse) this.GetApiResponse();
      return resultMessage;
    }

    protected virtual void BeforeExecute()
    {
    }

    protected virtual void AfterExecute()
    {
    }

    protected abstract void ValidateRequest();

    private void Validate()
    {
      TQ apiRequest = this.GetApiRequest();
      this.ValidateAndSetMerchantAuthentication();
      this.SetClientId();
      ClientConfig clientConfig = apiRequest.paybyClientConfig.clientConfig;
      this.ValidateRequest();
    }

    private void ValidateAndSetMerchantAuthentication()
    {
      PayByHttpRequest apiRequest = (PayByHttpRequest) this.GetApiRequest();
      if (apiRequest.paybyClientConfig.clientConfig != null)
        return;
      apiRequest.paybyClientConfig.clientConfig = ApiOperationBase<PayByHttpRequest, PaybyHttpResponse>.MerchantAuthentication != null ? ApiOperationBase<PayByHttpRequest, PaybyHttpResponse>.MerchantAuthentication : throw new ArgumentException("MerchantAuthentication cannot be null");
    }

    private void SetClientId()
    {
    }
  }
}
