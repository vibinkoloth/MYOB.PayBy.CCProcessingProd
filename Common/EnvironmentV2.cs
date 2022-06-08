// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.EnvironmentV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

namespace MYOB.PayBy.CCProcessing.Common
{
  public class EnvironmentV2
  {
    public static readonly EnvironmentV2 SANDBOX = new EnvironmentV2("https://test-merchants.paycorp.com.au", "https://test-ws.paycorp.com.au");
    public static readonly EnvironmentV2 PRODUCTION = new EnvironmentV2("https://merchants.paycorp.com.au", "https://webservices.paycorp.com.au");
    public static readonly EnvironmentV2 LOCAL_VM = new EnvironmentV2((string) null, (string) null);
    public static readonly EnvironmentV2 HOSTED_VM = new EnvironmentV2("https://webhook.site/8d7f7866-decc-4dff-97a3-e45de0ae1f2a", "https://webhook.site/8d7f7866-decc-4dff-97a3-e45de0ae1f2a");
    public static EnvironmentV2 CUSTOM = new EnvironmentV2((string) null, (string) null);
    private string _baseUrl;
    private string _soapBaseUrl;

    internal EnvironmentV2(string baseUrl, string soapBaseUrl)
    {
      this._baseUrl = baseUrl;
      this._soapBaseUrl = soapBaseUrl;
    }

    public string getBaseUrl() => this._baseUrl;

    public string getSoapBaseUrl() => this._soapBaseUrl;

    public enum Environment
    {
      DEBUG,
      TEST,
      LIVE,
    }
  }
}
