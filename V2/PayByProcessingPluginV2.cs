// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByProcessingPluginV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Attributes;
using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  [PXDisplayTypeName("PayBy Tokenized Plug-in")]
  public class PayByProcessingPluginV2 : ICCProcessingPlugin
  {
    public T CreateProcessor<T>(IEnumerable<SettingsValue> settingValues) where T : class
    {
      if (typeof (T) == typeof (ICCProfileProcessor))
        return new PayByProfileProcessorV2(settingValues) as T;
      if (typeof (T) == typeof (ICCHostedFormProcessor))
        return new PayByHostedFormProcessorV2(settingValues) as T;
      if (typeof (T) == typeof (ICCHostedPaymentFormProcessor))
        return new PayByHostedPaymentFormProcessorV2(settingValues) as T;
      if (typeof (T) == typeof (ICCTransactionProcessor))
        return new PayByTransactionProcessorV2(settingValues) as T;
      if (typeof (T) == typeof (ICCTransactionGetter))
        return new TransactionGetterV2(settingValues) as T;
      if (typeof (T) == typeof (ICCProfileCreator))
        return new ProfileCreatorV2(settingValues) as T;
      if (typeof (T) == typeof (ICCTranStatusGetter))
        return new PayByTranStatusGetterV2() as T;
      int num = typeof (T) != typeof (ICCWebhookProcessor) ? 1 : 0;
      return default (T);
    }

    public IEnumerable<SettingsDetail> ExportSettings() => PayByPluginHelper.GetDefaultSettings_CreditCard();

    public void TestCredentials(IEnumerable<SettingsValue> settingValues) => new PayByAuthenticateTestProcessor(settingValues).TestCredentials();

    public string ValidateSettings(SettingsValue setting) => PayByValidatorV2.Validate(setting);
  }
}
