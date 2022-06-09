// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByCompleteFormProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;
using Microsoft.CSharp.RuntimeBinder;
using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByCompleteFormProcessorV2 : PayByProcessorV2
  {
    public PayByCompleteFormProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
    }

    public PaymentCompleteResponse Processor(
      PayByHttpRequest transactionRequest2)
    {
      return this.ProcessCompleteResponse(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createInitController>(transactionRequest2, new createInitController(transactionRequest2)));
    }

    private PaymentCompleteResponse ProcessCompleteResponse(
      PaybyHttpResponse response)
    {
      PaymentCompleteResponse completeResponse1 = (PaymentCompleteResponse) null;
      if (!response.IsSuccess)
        return completeResponse1;
      PaymentCompleteResponse completeResponse2 = new PaymentCompleteResponse();
            // ISSUE: reference to a compiler-generated field
            //if (PayByCompleteFormProcessorV2.\u003Eo__2.\u003C\u003Ep__0 == null)
            //{
            //  // ISSUE: reference to a compiler-generated field
            //  PayByCompleteFormProcessorV2.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, PaymentCompleteResponse>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (PaymentCompleteResponse), typeof (PayByCompleteFormProcessorV2)));
            //}
            //// ISSUE: reference to a compiler-generated field
            //// ISSUE: reference to a compiler-generated field
            //return PayByCompleteFormProcessorV2.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) PayByCompleteFormProcessorV2.\u003C\u003Eo__2.\u003C\u003Ep__0, response.Response);
            if (response.IsSuccess)
            {
                //completeResp = new PaymentCompleteResponse();
                completeResponse2 = (PaymentCompleteResponse)response.Response;
                
            }
            return completeResponse2;
            // return completeResp;
        }
  }
}
