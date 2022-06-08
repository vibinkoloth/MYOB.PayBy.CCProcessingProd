// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByHostedFormProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByHostedFormProcessorV2 : PayByHostedFormHelperV2, ICCHostedFormProcessor
  {
    private IEnumerable<SettingsValue> _settingsValues;

    public PayByHostedFormProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
      this._settingsValues = settingValues;
    }

    public HostedFormData GetDataForCreateForm(CustomerData customerData)
    {
      ProcessingInput input = new ProcessingInput()
      {
        CustomerData = new CustomerData()
      };
      input.CustomerData = customerData;
      string curyid = this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>) (v => v.Value)).FirstOrDefault<string>();
      return this.GetHostedFormData(input, this._settingsValues, 1, curyid).FormData;
    }

    public HostedFormData GetDataForManageForm(
      CustomerData customerData,
      CreditCardData cardData)
    {
      ProcessingInput input = new ProcessingInput()
      {
        CustomerData = new CustomerData()
      };
      input.CustomerData = customerData;
      input.CardData = new CreditCardData();
      input.CardData = cardData;
      string curyid = this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>) (v => v.Value)).FirstOrDefault<string>();
      return this.GetHostedFormData(input, this._settingsValues, 1, curyid).FormData;
    }
  }
}
