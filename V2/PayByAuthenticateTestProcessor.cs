// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByAuthenticateTestProcessor
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;
using PX.CCProcessingBase.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByAuthenticateTestProcessor : PayByHostedFormHelperV2
  {
    private IEnumerable<SettingsValue> _settingsValues;

    public PayByAuthenticateTestProcessor(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
      this._settingsValues = settingValues;
    }

    public void TestCredentials()
    {
      using (LogProvider.OpenNestedContext("PayByAuthenticateTestProcessor.TestCredentials"))
      {
        this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Debug, (Func<string>) (() => "Start."), (Exception) null);
        this.GetHostedFormData(new ProcessingInput(), this._settingsValues, curyid: this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>) (v => v.Value)).FirstOrDefault<string>());
        this.Logger.Log(PX.CCProcessingBase.Logging.LogLevel.Debug, (Func<string>) (() => "End."), (Exception) null);
      }
    }
  }
}
