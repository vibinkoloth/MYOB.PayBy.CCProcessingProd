// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PBCustomerPaymentMethodMaintExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Objects.AR;
using PX.Objects.AR.CCPaymentProcessing.Helpers;
using System;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PBCustomerPaymentMethodMaintExt : PXGraphExtension<CustomerPaymentMethodMaint>
  {
    protected void CustomerPaymentMethod_RowSelected(
      PXCache cache,
      PXRowSelectedEventArgs e,
      PXRowSelected baseevent)
    {
      baseevent(cache, e);
      CustomerPaymentMethod row = (CustomerPaymentMethod) e.Row;
      if (row == null)
        return;
      row.GetExtension<CustomerPaymentMethodExt>();
      bool isVisible = CCProcessingHelper.IsTokenizedPaymentMethod((PXGraph) this.Base, this.Base.CustomerPaymentMethod.Current.PMInstanceID);
      PXUIFieldAttribute.SetVisible<CustomerPaymentMethodExt.usrExpirationDate>(cache, (object) row, isVisible);
      if (!row.ExpirationDate.HasValue)
        return;
      PXCache cache1 = this.Base.CustomerPaymentMethod.Cache;
      CustomerPaymentMethod current = this.Base.CustomerPaymentMethod.Current;
      DateTime dateTime = row.ExpirationDate.Value;
      dateTime = dateTime.AddDays(-1.0);
      string str = dateTime.ToString("MM/yy");
      cache1.SetValue<CustomerPaymentMethodExt.usrExpirationDate>((object) current, (object) str);
    }
  }
}
