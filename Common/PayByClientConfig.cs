// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByClientConfig
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.config;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PayByClientConfig
  {
    public string curyID { get; set; }

    public string clientId { get; set; }

    public string recurClientId { get; set; }

    public string certPassword { get; set; }

    public string tranCode { get; set; }

    public string refundTranCode { get; set; }

    public EnvironmentV2.Environment Environment { get; set; }

    public ClientConfig clientConfig { get; set; }
  }
}
