// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.IApiOperation`2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.Common
{
  public interface IApiOperation<TQ, TS>
    where TQ : PayByHttpRequest
    where TS : PaybyHttpResponse
  {
    TS GetApiResponse();

    TS ExecuteWithApiResponse(EnvironmentV2 environment = null);

    void Execute(EnvironmentV2 environment = null);

    messageTypeEnum GetResultCode();

    List<string> GetResults();
  }
}
