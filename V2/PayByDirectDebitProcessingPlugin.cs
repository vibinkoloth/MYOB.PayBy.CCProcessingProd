// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByDirectDebitProcessingPlugin
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Attributes;
using PX.CCProcessingBase.Interfaces.V2;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.V2
{
  [PXDisplayTypeName("PayBy Direct Debit Plug-in")]
  public class PayByDirectDebitProcessingPlugin : ICCProcessingPlugin
  {
    public T CreateProcessor<T>(IEnumerable<SettingsValue> settingValues) where T : class => typeof (T) == typeof (ICCTransactionProcessor) ? new PayByDDTransactionProcessorV2(settingValues) as T : default (T);

    public IEnumerable<SettingsDetail> ExportSettings() => PayByPluginHelper.GetDefaultSettings_DirectDebit();

    public void TestCredentials(IEnumerable<SettingsValue> settingValues) => new PayByAuthenticateTestProcessor(settingValues).TestCredentials();

    public string ValidateSettings(SettingsValue setting) => PayByValidatorV2.Validate(setting);
  }
}
