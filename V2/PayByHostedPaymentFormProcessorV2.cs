// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.V2.PayByHostedPaymentFormProcessorV2
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.V2
{
  public class PayByHostedPaymentFormProcessorV2 : 
    PayByHostedFormHelperV2,
    ICCHostedPaymentFormProcessor
  {
    private IEnumerable<SettingsValue> _settingsValues;

    public PayByHostedPaymentFormProcessorV2(IEnumerable<SettingsValue> settingValues)
      : base(settingValues)
    {
      this._settingsValues = settingValues;
    }

    public HostedFormData GetDataForPaymentForm(ProcessingInput inputData)
    {
      string curyid = this._settingsValues.Where<SettingsValue>((Func<SettingsValue, bool>) (x => x.DetailID == "CURRENCY")).Select<SettingsValue, string>((Func<SettingsValue, string>) (v => v.Value)).FirstOrDefault<string>();
      PXSelectJoin<Customer, InnerJoin<ARPayment, On<Customer.bAccountID, Equal<ARPayment.customerID>>>, Where<ARPayment.refNbr, Equal<Required<ARPayment.refNbr>>, And<ARPayment.docType, Equal<Required<ARPayment.docType>>>>> pxSelectJoin = new PXSelectJoin<Customer, InnerJoin<ARPayment, On<Customer.bAccountID, Equal<ARPayment.customerID>>>, Where<ARPayment.refNbr, Equal<Required<ARPayment.refNbr>>, And<ARPayment.docType, Equal<Required<ARPayment.docType>>>>>((PXGraph) PXGraph.CreateInstance<ARPaymentEntry>());
      inputData.CustomerData = new CustomerData();
      inputData.CustomerData.CustomerCD = pxSelectJoin.SelectSingle((object) inputData.DocumentData.DocRefNbr, (object) inputData.DocumentData.DocType).AcctCD;
      return this.GetHostedFormData(inputData, this._settingsValues, 2, curyid).FormData;
    }
  }
}
