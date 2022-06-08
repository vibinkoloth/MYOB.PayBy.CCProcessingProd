// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByBatchProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByBatchProcessorV2 : PayByProcessorV2
  {
    public PayByBatchProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
    }

    public ProcessingResult Processor(PayByHttpRequest transactionRequest2) => PayByPluginHelper.ProcessBatchResponseV2(this.ProcessRequest<PayByHttpRequest, PaybyHttpResponse, createBatchController>(transactionRequest2, new createBatchController(transactionRequest2)));
  }
}
