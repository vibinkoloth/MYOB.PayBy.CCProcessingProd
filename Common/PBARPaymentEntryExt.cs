// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PBARPaymentEntryExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using gateway_client_csharp.au.com.gateway.client.payment;
using MYOB.PayBy.CCProcessing.PAYBY.PaybyGatewayExt;
using PX.Common;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.AR.CCPaymentProcessing.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PBARPaymentEntryExt : PXGraphExtension<ARPaymentEntry>
  {
    protected void ARPayment_RowSelected(
      PXCache cache,
      PXRowSelectedEventArgs e,
      PXRowSelected baseevent)
    {
      baseevent(cache, e);
      if (!(e.Row is ARPayment row1))
        return;
      CCProcTran ccProcTran = (CCProcTran) null;
      PaymentState paymentState = new PaymentState((IEnumerable<PXResult<CCProcTran>>) this.Base.ccProcTran.Select());
      if (paymentState.lastTran != null)
        ccProcTran = this.Base.ccProcTran.View.SelectSingleBound(new object[1]
        {
          (object) paymentState.lastTran
        }) as CCProcTran;
      if (ccProcTran == null || !(e.Row is ARRegister row2))
        return;
      ARRegisterExt extension = row2.GetExtension<ARRegisterExt>();
      bool? nullable = row1.Hold;
      bool flag1 = true;
      if (!(nullable.GetValueOrDefault() == flag1 & nullable.HasValue) || ccProcTran == null || !(ccProcTran.PCResponseCode == "HFR"))
        return;
      nullable = extension.UsrIsDDPaymentCancel;
      bool flag2 = true;
      int num = nullable.GetValueOrDefault() == flag2 & nullable.HasValue ? 1 : 0;
    }

    protected void ARPayment_RowPersisting(
      PXCache cache,
      PXRowPersistingEventArgs e,
      PXRowPersisting baseevent)
    {
      ARPayment row1 = (ARPayment) e.Row;
      baseevent(cache, e);
      if (row1 == null || !(row1.DocType == "PMT") && !(row1.DocType == "REF"))
        return;
      row1.GetExtension<ARPaymentExt>();
      bool flag1 = this.Base.ARPayment_DocType_RefNbr.Cache.Deleted.Count() != 0L;
      if (this.Base.ARPayment_DocType_RefNbr.Cache.Updated.Count() == 0L || flag1)
        return;
      CCProcTran ccProcTran = (CCProcTran) null;
      PaymentState paymentState = new PaymentState((IEnumerable<PXResult<CCProcTran>>) this.Base.ccProcTran.Select());
      if (paymentState.lastTran != null)
        ccProcTran = this.Base.ccProcTran.View.SelectSingleBound(new object[1]
        {
          (object) paymentState.lastTran
        }) as CCProcTran;
      if (ccProcTran == null || !(e.Row is ARRegister row2))
        return;
      ARRegisterExt extension = row2.GetExtension<ARRegisterExt>();
      if (ccProcTran == null || !(ccProcTran.PCResponseCode == "HFR"))
        return;
      bool? isDdPaymentCancel = extension.UsrIsDDPaymentCancel;
      bool flag2 = true;
      if (isDdPaymentCancel.GetValueOrDefault() == flag2 & isDdPaymentCancel.HasValue)
        return;
      if (row1.DocType == "REF")
        ccProcTran.TranType = "AUT";
      this.Base.ccProcTran.Cache.SetValue<CCProcTran.tranStatus>((object) this.Base, (object) PayByPluginHelper.CCTranStatusCode2(ccProcTran.PCResponseCode));
      ccProcTran.TranStatus = PayByPluginHelper.CCTranStatusCode2(ccProcTran.PCResponseCode);
      this.Base.ccProcTran.Update(ccProcTran);
    }

    public class PaymentTransactionExt : 
      PXGraphExtension<ARPaymentEntry.PaymentTransaction, ARPaymentEntry>
    {
      [PXOverride]
      public IEnumerable SyncPaymentTransaction(
        PXAdapter adapter,
        PBARPaymentEntryExt.PaymentTransactionExt.SyncPaymentTransactionDelegate baseMethod)
      {
        PX.Objects.CR.BAccount baccount = (PX.Objects.CR.BAccount) PXSelectBase<PX.Objects.CR.BAccount, PXSelect<PX.Objects.CR.BAccount, Where<PX.Objects.CR.BAccount.bAccountID, Equal<Required<PX.Objects.CR.BAccount.bAccountID>>>>.Config>.Select((PXGraph) this.Base, (object) this.Base.Document.Current.CustomerID);
        if (!ProfileServer.DoesCustomerHavePendingRequest(baccount.AcctCD))
          return baseMethod(adapter);
        ARPayment doc = adapter.Get<ARPayment>().First<ARPayment>();
        ProfileServer.GetAllPaymentProfiles(baccount.AcctCD);
        PaymentCompleteResponse response = ProfileServer.GetCustomerTransaction(baccount.AcctCD);
        PXLongOperation.StartOperation((PXGraph) this.Base, (PXToggleAsyncDelegate) (() =>
        {
          this.Base1.SyncPaymentTransactionById(doc, new List<string>()
          {
            response.token
          });
          PXSelect<CustomerPaymentMethod, Where<CustomerPaymentMethod.pMInstanceID, Equal<Required<CustomerPaymentMethod.pMInstanceID>>>> pxSelect = new PXSelect<CustomerPaymentMethod, Where<CustomerPaymentMethod.pMInstanceID, Equal<Required<CustomerPaymentMethod.pMInstanceID>>>>((PXGraph) this.Base);
          CustomerPaymentMethod customerPaymentMethod = (CustomerPaymentMethod) pxSelect.Select((object) doc.PMInstanceID);
          string number = response.creditCard.number;
          string str = number.Substring(number.Length - 4);
          customerPaymentMethod.Descr = customerPaymentMethod.Descr.Substring(0, customerPaymentMethod.Descr.LastIndexOf("-") + 1) + str;
          pxSelect.Cache.Update((object) customerPaymentMethod);
          pxSelect.Cache.Persist(PXDBOperation.Update);
        }));
        return adapter.Get();
      }

      protected void ARPayment_RowSelected(
        PXCache cache,
        PXRowSelectedEventArgs e,
        PXRowSelected baseevent)
      {
        baseevent(cache, e);
        if (!(e.Row is ARPayment row))
          return;
        bool? newCard = row.NewCard;
        if (!newCard.HasValue)
          return;
        newCard = row.NewCard;
        if (!newCard.Value)
          return;
        this.Base1.authorizeCCPayment.SetCaption("Add Credit Card");
        this.Base1.captureCCPayment.SetEnabled(false);
      }

      public delegate IEnumerable SyncPaymentTransactionDelegate(PXAdapter adapter);
    }
  }
}
