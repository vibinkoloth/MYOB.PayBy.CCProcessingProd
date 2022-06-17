// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using Microsoft.CSharp.RuntimeBinder;
using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using PX.CCProcessingBase.Logging;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web.Script.Serialization;

namespace MYOB.PayBy.CCProcessing.V2
{
  public abstract class PayByProcessorV2
  {
    private PayByClientConfig _paybyClientConfig;
    protected IEnumerable<SettingsValue> _settingValues;

    protected PayByProcessorV2(IEnumerable<SettingsValue> setting)
    {
      this.Logger = LogProvider.GetCurrentClassLogger();
      string.IsNullOrEmpty(PayByValidatorV2.Validate(setting));
      this._settingValues = setting;
    }

    internal ILog Logger { get; set; }

    protected EnvironmentV2 Environment
    {
      get
      {
        switch (PayByPluginHelper.GetEnvironment(this._settingValues))
        {
          case EnvironmentV2.Environment.DEBUG:
            string str = this._settingValues.FirstOrDefault<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "DEBUGURL"))?.Value;
            return string.IsNullOrEmpty(str) ? EnvironmentV2.SANDBOX : new EnvironmentV2(str, str);
          case EnvironmentV2.Environment.TEST:
            return EnvironmentV2.SANDBOX;
          case EnvironmentV2.Environment.LIVE:
            return EnvironmentV2.PRODUCTION;
          default:
            return EnvironmentV2.SANDBOX;
        }
      }
    }

    protected PayByClientConfig ClientConfigAuthentication
    {
      get
      {
        if (this._settingValues != null && this._paybyClientConfig == null)
          this._paybyClientConfig = PayByPluginHelper.GetClientConfig(this._settingValues);
        return this._paybyClientConfig;
      }
    }

    protected virtual string ValidateResponse(PaybyHttpResponse response) => PayByPluginHelper.GetResponseError(response);

    private string ValidateErrorResponse(PaybyHttpResponse errorResponse)
    {
            if (errorResponse != null)
            {
                return PayByPluginHelper.BuildMessagesString(errorResponse.Response);
            }

            return string.Empty;
    }
    
    protected TS ProcessRequest<TQ, TS, TC>(TQ request, TC controller)
      where TQ : PayByHttpRequest
      where TS : PaybyHttpResponse
      where TC : ApiOperationBase<TQ, TS>
    {
      try
      {
        if (this._paybyClientConfig != null && this._paybyClientConfig?.tranCode == "1000")
        {
          PXTrace.WriteInformation("Environment : " + this.Environment.getBaseUrl());
          PXTrace.WriteInformation("Client Config : " + new JavaScriptSerializer().Serialize((object) this._paybyClientConfig));
          if (request.OperationType == operationEnum.PAYMENT_BATCH)
            PXTrace.WriteInformation("Environment DD: " + this.Environment.getBaseUrl() + "/wsi/services/Payments");
          else if (request.OperationType == operationEnum.PAYMENT_REAL_TIME)
            PXTrace.WriteInformation("Environment CC: " + this.Environment.getBaseUrl() + "/rest/service/proxy");
        }
        if (request.OperationType == operationEnum.PAYMENT_BATCH)
          PXTrace.WriteInformation("Direct Entry Payby Request : " + SecurityElement.Escape(request.soapRequest.soapEnvelopeXml.InnerXml));
        else if (request.OperationType == operationEnum.PAYMENT_REAL_TIME)
          PXTrace.WriteInformation("Realtime Payby Request : " + request.realTimeRequest?.ToString());
        TS s = controller.ExecuteWithApiResponse(this.Environment);
        PaybyHttpResponse errorResponse = controller.GetErrorResponse();
        string empty = string.Empty;
        string message = string.Empty;
        if (errorResponse == null)
          PXTrace.WriteInformation("Payby Response : " + s.Message);
        else
          message = errorResponse.Message;
        if (!string.IsNullOrEmpty(message))
        {
          this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Error, (Func<string>) (() => "ErrorResponse: {0}"), (Exception) null, (object) message);
          throw new PXException(message);
        }
        return s;
      }
      catch (WebException ex)
      {
        if (ex.Response != null)
        {
          PXTrace.WriteError("Response received: {0}", (object) ex.Message);
          this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Error, (Func<string>) (() => "Response received: {0}"), (Exception) null, (object) ex.Response, (object) ex, (object) ex.Message);
        }
        else
          PXTrace.WriteError("Received Response Is Empty");
        this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Error, (Func<string>) (() => "Received Response Is Empty"), (Exception) null);
                // Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
                throw new PXException("The request cannot be processed. Probably there are some network issues.", (Exception) ex);
      }
      catch (Exception ex)
      {
        PXTrace.WriteError("Error while processing request");
        this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Error, (Func<string>) (() => "Error while processing request"), ex);
        throw new PXException(ex.Message, ex);
      }
    }
  }
}
