// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.SettingsKeys
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

namespace MYOB.PayBy.CCProcessing.Common
{
  internal static class SettingsKeys
  {
    public const string Key_APIURL = "APIURL";
    public const string Key_CLIENTID = "CLIENTID";
    public const string Key_RECURCLIENTID = "RCLIENTID";
    public const string Key_AUTHTOKEN = "AUTHTOKEN";
    public const string Key_DEBUGURL = "DEBUGURL";
    public const string Key_HMACKEY = "HMACKEY";
    public const string Key_ENVIRONMENT = "ENV";
    public const string Key_TRAN_CODE = "TRANCODE";
    public const string Key_REFUND_CODE = "REFUNDCODE";
    public const string Key_CERT_PASS = "CERTPASS";
    public const string Key_CURYID = "CURRENCY";
    public static string Key_VALIDATION_MODE = "VALIDATION";
    public const string Default_APIURL = "https://test-merchants.paycorp.com.au/rest/service/proxy";
    public const string Default_CLIENTID = "";
    public const string Default_AUTHTOKEN = "";
    public const string Default_HMACKEY = "";
    public const string Default_ENVIRONMENT = "DEBUG";
    public const string Default_TRAN_CODE = "13";
    public const string Default_REFUND_CODE = "50";
    public const string Default_CERT_PASS = "";
    public const string Default_CURYID = "AUD";
    public static string Default_VALIDATION_MODE = EnvironmentMode.LiveMode.ToString();
    public const string Descr_APIURL = "Specify API Url";
    public const string Descr_CLIENTID = "Specify client id of your Payby Account";
    public const string Descr_RCLIENTID = "Specify recurring(CC)/refund(DD) client id of your Payby Account";
    public const string Descr_AUTHTOKEN = "Specify Authentication key of your Payby Account";
    public const string Descr_HMACKEY = "Specify HMAC key of your Payby Account";
    public const string Descr_ENVIRONMENT = "Sets payby environment";
    public const string Descr_TRAN_CODE = "Sets transaction code for Direct Debit";
    public const string Descr_REFUND_CODE = "Sets transaction code for Direct Credit (refund)";
    public const string Descr_CERT_PASS = "Sets Certificate Password for DE Production only";
    public const string Descr_CURYID = "Sets Currency for Payment Gateway";
    public const string Descr_VALIDATION_MODE = "The processing mode for the request";
  }
}
