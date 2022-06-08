// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.ConstantCommon
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data.BQL;

namespace MYOB.PayBy.CCProcessing.Common
{
  public static class ConstantCommon
  {
    public const string PluginError = "Plugin Error";
    public const string yyyy_MM = "yyyy-MM";
    public const string MM_yy = "MMyy";
    public const string Card12Zero = "000000000000";
    public const string DE_CODE_HFR = "HFR";
    public const string DE_CODE_FAIL = "FAL";
    public const string DE_CODE_CAN = "CAN";
    public const string DE_CODE_APR = "APR";
    public const string DE_CODE_ERR = "ERR";
    public const string DDProcTypeNameV2 = "MYOB.PayBy.CCProcessing.V2.PayByDirectDebitProcessingPlugin";
    public const string CCProcTypeNameV2 = "MYOB.PayBy.CCProcessing.V2.PayByProcessingPluginV2";
    public const string DE_DESCR_S = "Transaction has been submitted to the PPS and is awaiting transmission to your financial institution";
    public const string DE_DESCR_L = "Transaction has been submitted to your financial institution, but no validation has been undertaken";
    public const string DE_DESCR_G = "Transaction has been submitted to your financial institution, there are currently no errors, but the transaction could be rejected in the coming days";
    public const string DE_DESCR_E = "Transaction has been submitted to your financial institution and has failed initial validation";
    public const string DE_DESCR_R = "Transaction has been submitted to your financial institution and has passed initial validation, but has subsequently been rejected by the customer’s bank";
    public const string DE_DESCR_V = "Transaction cancelled.This has not been sent to the Bank";
    public const string DE_DESCR_ERR = "Transaction has an error.This has not been sent to the Bank";
    public const string DE_STATUS_ERR = "Unknown Error";
    public const string DE_STATUS_HFR = "Held for Review";
    public const string DE_STATUS_FAIL = "Failed";
    public const string DE_STATUS_APR = "Pass";
    public const string DE_STATUS_CAN = "Cancelled";
    public const string Amount0Validation = "Amount should be greater than zero";
    public const string PayAmountValidation = "Payment amount should be greater than zero";
    public const string PaymentMethodRequired = "Please select correct payment method for Pay with New Card action";
    public const string PaymentMethodNotConfig = "This payment method not configured for Payment with New Card. Please check the processing center setting";
    public const string CardAlreadyExists = "This card already exists";

    public static string customerCD { get; set; }

    public static string RefNbr { get; set; }

    public class dDProcTypeNameV2 : 
      BqlType<IBqlString, string>.Constant<ConstantCommon.dDProcTypeNameV2>
    {
      public dDProcTypeNameV2()
        : base("MYOB.PayBy.CCProcessing.V2.PayByDirectDebitProcessingPlugin")
      {
      }
    }

    public class cCprocTypeNameV2 : 
      BqlType<IBqlString, string>.Constant<ConstantCommon.cCprocTypeNameV2>
    {
      public cCprocTypeNameV2()
        : base("MYOB.PayBy.CCProcessing.V2.PayByProcessingPluginV2")
      {
      }
    }
  }
}
