// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PBDirectEntryProcessing
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.V2;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Common;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.AR.CCPaymentProcessing;
using PX.Objects.AR.CCPaymentProcessing.Common;
using PX.Objects.CA;
using PX.Objects.CM;
using PX.Objects.Extensions.PaymentTransaction;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PBDirectEntryProcessing : PXGraph<PBDirectEntryProcessing>
  {
    private ARPaymentEntry paymentGraph;
    public PXFilter<DDResponseFilter> Filter;
    public PXCancel<DDResponseFilter> Cancel;
    [PXFilterable(new System.Type[] {})]
    public PXFilteredProcessing<ARPaymentInfo, DDResponseFilter> ARDocumentList;
    public ToggleCurrency<DDResponseFilter> CurrencyView;
    public PXSelect<CurrencyInfo> currencyinfo;
    public PXSelect<CurrencyInfo, Where<CurrencyInfo.curyInfoID, Equal<Required<CurrencyInfo.curyInfoID>>>> CurrencyInfo_CuryInfoID;
    [Obsolete("Will be removed in Acumatica 2019R1")]
    public PXAction<DDResponseFilter> EditDetail;
    public PXSetup<PX.Objects.AR.ARSetup> ARSetup;

    public PBDirectEntryProcessing()
    {
      PXCurrencyAttribute.SetBaseCalc<DDResponseFilter.curySelTotal>(this.Filter.Cache, (object) null, false);
      this.ARDocumentList.SetSelected<ARPayment.selected>();
      DDResponseFilter filter = this.Filter.Current;
      this.ARDocumentList.SetProcessDelegate<ARPaymentCCProcessing>((PXProcessingBase<ARPaymentInfo>.ProcessItemDelegate<ARPaymentCCProcessing>) ((aGraph, doc) => PBDirectEntryProcessing.ProcessPayment(aGraph, doc, filter)));
      this.ARDocumentList.Cache.AllowInsert = false;
      this.paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();
    }

    protected virtual IEnumerable ardocumentlist()
    {
      PBDirectEntryProcessing graph = this;
      DateTime date = DateTime.Now.Date;
      PXSelectBase<CCProcTran> tranSelect = (PXSelectBase<CCProcTran>) new PXSelect<CCProcTran, Where<CCProcTran.docType, Equal<Optional<CCProcTran.docType>>, And<CCProcTran.refNbr, Equal<Optional<CCProcTran.refNbr>>>>, OrderBy<Desc<CCProcTran.tranNbr>>>((PXGraph) graph);
      foreach (PXResult<ARPaymentInfo, Customer, CustomerPaymentMethod, PaymentMethod, CCProcessingCenterPmntMethod> pxResult in PXSelectBase<ARPaymentInfo, PXSelectJoin<ARPaymentInfo, InnerJoin<Customer, On<Customer.bAccountID, Equal<ARPayment.customerID>>, InnerJoin<CustomerPaymentMethod, On<CustomerPaymentMethod.pMInstanceID, Equal<ARPayment.pMInstanceID>>, InnerJoin<PaymentMethod, On<PaymentMethod.paymentMethodID, Equal<ARPayment.paymentMethodID>, And<PaymentMethod.paymentType, Equal<PaymentMethodType.creditCard>, And<PaymentMethod.aRIsProcessingRequired, Equal<True>>>>, LeftJoin<CCProcessingCenterPmntMethod, On<CCProcessingCenterPmntMethod.paymentMethodID, Equal<PaymentMethod.paymentMethodID>, And<CCProcessingCenterPmntMethod.isDefault, Equal<True>, And<CCProcessingCenterPmntMethod.isActive, Equal<True>>>>>>>>, Where<ARPayment.released, Equal<False>, And<ARPayment.voided, Equal<False>, And2<Where<ARPayment.docType, Equal<ARDocType.payment>, Or<ARPayment.docType, Equal<ARDocType.prepayment>, Or<ARPayment.docType, Equal<ARDocType.refund>>>>, And<ARPayment.docDate, LessEqual<Current<DDResponseFilter.payDate>>, And<ARPayment.isMigratedRecord, NotEqual<True>, And<CCProcessingCenterPmntMethod.processingCenterID, Equal<Current<DDResponseFilter.processingCenterID>>, And2<Where<PaymentMethod.paymentMethodID, Equal<Current<DDResponseFilter.paymentMethodID>>, Or<Current<DDResponseFilter.paymentMethodID>, IsNull>>, And<Match<Customer, Current<AccessInfo.userName>>>>>>>>>>, OrderBy<Asc<ARPayment.refNbr>>>.Config>.Select((PXGraph) graph))
      {
        ARPaymentInfo doc = (ARPaymentInfo) pxResult;
        CustomerPaymentMethod customerPaymentMethod = (CustomerPaymentMethod) pxResult;
        CCProcessingCenterPmntMethod centerPmntMethod = (CCProcessingCenterPmntMethod) pxResult;
        if (string.IsNullOrEmpty(graph.Filter.Current.ProcessingCenterID) || !(graph.Filter.Current.ProcessingCenterID != centerPmntMethod.ProcessingCenterID))
        {
          ARDocKey arDocKey = new ARDocKey((ARRegister) doc);
          if (graph.Filter.Current == null || !graph.Filter.Current.CustomerID.HasValue || object.Equals((object) graph.Filter.Current.CustomerID, (object) doc.CustomerID))
          {
            doc.PMInstanceDescr = customerPaymentMethod.Descr;
            doc.PaymentMethodID = customerPaymentMethod.PaymentMethodID;
            doc.ProcessingCenterID = centerPmntMethod != null ? centerPmntMethod.ProcessingCenterID : string.Empty;
            PaymentState paymentState = new PaymentState((IEnumerable<PXResult<CCProcTran>>) tranSelect.Select((object) doc.DocType, (object) doc.RefNbr));
            CCProcTran lastTran = (CCProcTran) paymentState.lastTran;
            doc.CCPaymentStateDescr = paymentState.Description;
            doc.CCPaymentStateDescr = paymentState.Description;
            doc.CCTranDescr = lastTran != null ? (string.IsNullOrEmpty(lastTran.PCResponseReasonText) ? lastTran.PCResponseReasonText : lastTran.ErrorText) : string.Empty;
            if (lastTran == null || string.IsNullOrEmpty(lastTran.PCTranNumber))
              yield return (object) doc;
            if ((paymentState.isCCCaptured || paymentState.isCCPreAuthorized) && (lastTran.TranStatus == "APR" || lastTran.TranStatus == "HFR"))
              yield return (object) doc;
            paymentState = (PaymentState) null;
            lastTran = (CCProcTran) null;
          }
          doc = (ARPaymentInfo) null;
        }
      }
    }

    [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = false)]
    [PXEditDetailButton]
    public virtual IEnumerable editDetail(PXAdapter adapter)
    {
      ARPayment current = (ARPayment) this.ARDocumentList.Current;
      if (current != null)
      {
        ARPaymentEntry instance = PXGraph.CreateInstance<ARPaymentEntry>();
        instance.Document.Current = (ARPayment) instance.Document.Search<ARPayment.refNbr>((object) current.RefNbr, (object) current.DocType);
        PXRedirectRequiredException requiredException = new PXRedirectRequiredException((PXGraph) instance, true, "Payment");
        requiredException.Mode = PXBaseRedirectException.WindowMode.NewWindow;
        throw requiredException;
      }
      return adapter.Get();
    }

    public static void ProcessPayment(
      ARPaymentCCProcessing aProcessingGraph,
      ARPaymentInfo aDoc,
      DDResponseFilter aFilter)
    {
      aProcessingGraph.Clear();
      aProcessingGraph.Document.Current = (ARPayment) aProcessingGraph.Document.Search<ARPayment.docType, ARPayment.refNbr>((object) aDoc.DocType, (object) aDoc.RefNbr);
      ARPaymentEntry.CheckValidPeriodForCCTran((PXGraph) aProcessingGraph, aProcessingGraph.Document.Current);
      PXGraph.CreateInstance<PBDirectEntryProcessing>().DDTranResponseUpdate(aProcessingGraph, aFilter);
    }

    public void DDTranResponseUpdate(
      ARPaymentCCProcessing aProcessingGraph,
      DDResponseFilter aFilter)
    {
      PayByHttpRequest payByHttpRequest1 = (PayByHttpRequest) new PayByRealtimePaymentRequest();
      payByHttpRequest1.soapRequest = new PayBySoapRequest();
      payByHttpRequest1.OperationType = operationEnum.PAYMENT_BATCH;
      IEnumerable<SettingsValue> settings = this.GetSettings(aFilter.ProcessingCenterID);
      payByHttpRequest1.paybyClientConfig = PayByPluginHelper.GetClientConfig(settings);
      PayByHttpRequest payByHttpRequest2 = payByHttpRequest1;
      payByHttpRequest1.transactionType = transactionTypeEnum.REPORT;
      PayByHttpRequest payByHttpRequest3 = payByHttpRequest1;
      CCProcTran ccProcTran = (CCProcTran) aProcessingGraph.ccProcTran.Select();
      if (settings != null && ccProcTran != null && !string.IsNullOrEmpty(ccProcTran.PCTranNumber))
      {
        PayByReportProcessorV2 reportProcessorV2 = new PayByReportProcessorV2(settings);
        string clientId = payByHttpRequest1.paybyClientConfig.clientId;
        bool flag1 = ccProcTran.PCResponseReasonText.StartsWith("Refund");
        if (flag1)
          clientId = payByHttpRequest1.paybyClientConfig.recurClientId;
        payByHttpRequest2.soapRequest.soapEnvelopeXml = PayByPluginHelper.GetTransactionReport(ccProcTran.PCResponseReasonCode, clientId);
        PX.CCProcessingBase.Interfaces.V2.ProcessingResult processingResult1 = new PX.CCProcessingBase.Interfaces.V2.ProcessingResult();
        PayByHttpRequest transactionRequest2 = payByHttpRequest3;
        PX.CCProcessingBase.Interfaces.V2.ProcessingResult processingResult2 = reportProcessorV2.Processor(transactionRequest2);
        ccProcTran.PCResponseCode = processingResult2.ResponseCode;
        ccProcTran.PCResponseReasonText = flag1 ? "Refund" : processingResult2.ResponseReasonText ?? "";
        ccProcTran.TranStatus = PayByPluginHelper.CCTranStatusCode2(processingResult2.ResponseCode);
        ccProcTran.StartTime = new DateTime?(PXTimeZoneInfo.Now);
        if (processingResult2.ResponseCode == "HFR")
          ccProcTran.ProcStatus = "OPN";
        else if (processingResult2.ResponseCode == "APR")
        {
          ccProcTran.TranType = flag1 ? "CDT" : "PAC";
          ccProcTran.ProcStatus = "FIN";
        }
        else
        {
          ccProcTran.TranType = "VDG";
          ccProcTran.ProcStatus = "ERR";
        }
        aProcessingGraph.ccProcTran.Current = ccProcTran;
        aProcessingGraph.ccProcTran.Insert(aProcessingGraph.ccProcTran.Current);
        aProcessingGraph.Actions.PressSave();
        PX.Objects.AR.ARSetup arSetup = (PX.Objects.AR.ARSetup) this.ARSetup.Select();
        if (arSetup == null)
          return;
        bool? integratedCcProcessing = arSetup.IntegratedCCProcessing;
        bool flag2 = true;
        if (!(integratedCcProcessing.GetValueOrDefault() == flag2 & integratedCcProcessing.HasValue))
          return;
        List<ARRegister> list = new List<ARRegister>();
        ARPayment current = aProcessingGraph.Document.Current;
        if (current != null)
        {
          bool? hold = current.Hold;
          bool flag3 = true;
          if (!(hold.GetValueOrDefault() == flag3 & hold.HasValue))
          {
            this.paymentGraph.Document.Current = current;
            if (this.paymentGraph.release.GetEnabled())
              list.Add((ARRegister) current);
          }
        }
        if (list.Count <= 0)
          return;
        PXLongOperation.StartOperation((PXGraph) this, (PXToggleAsyncDelegate) (() => ARDocumentRelease.ReleaseDoc(list, false)));
      }
      else
      {
        if (ccProcTran != null && !string.IsNullOrEmpty(ccProcTran.PCTranNumber))
          return;
        this.paymentGraph.Document.Current = aProcessingGraph.Document.Current;
        CCPaymentEntry ccPaymentEntry = new CCPaymentEntry((PXGraph) aProcessingGraph);
        if (this.paymentGraph.Document.Current.DocType == "REF")
        {
          ccPaymentEntry.AddAfterProcessCallback(new AfterTranProcDelegate(PBDirectEntryProcessing.ChangeRefundTranType));
          ccPaymentEntry.CreditCCPayment((ICCPayment) aProcessingGraph.Document.Current, (ICCPaymentTransactionAdapter) new PayByPluginHelper.PaybyCCPaymentTransactionAdapter());
        }
        else
        {
          if (!(this.paymentGraph.Document.Current.DocType == "PMT"))
            return;
          ccPaymentEntry.AddAfterProcessCallback(new AfterTranProcDelegate(ARPaymentEntry.PaymentTransaction.UpdateARPaymentAndSetWarning));
          ccPaymentEntry.AddAfterProcessCallback(new AfterTranProcDelegate(PaymentTransactionGraph<ARPaymentCCProcessing, ARPayment>.ReleaseARDocument));
          ccPaymentEntry.AddAfterProcessCallback(new AfterTranProcDelegate(PaymentTransactionGraph<ARPaymentCCProcessing, ARPayment>.CheckForHeldForReviewStatusAfterProc));
          ccPaymentEntry.AuthorizeCCpayment((ICCPayment) aProcessingGraph.Document.Current, (ICCPaymentTransactionAdapter) new PayByPluginHelper.PaybyCCPaymentTransactionAdapter());
        }
      }
    }

    public IEnumerable<SettingsValue> GetSettings(string aProcessingCenterID)
    {
      List<SettingsValue> settings = new List<SettingsValue>();
      foreach (PXResult<CCProcessingCenter, CCProcessingCenterDetail> pxResult in PXSelectBase<CCProcessingCenter, PXSelectJoin<CCProcessingCenter, InnerJoin<CCProcessingCenterDetail, On<CCProcessingCenter.processingCenterID, Equal<CCProcessingCenterDetail.processingCenterID>>>, Where<CCProcessingCenter.processingCenterID, Equal<Required<CCProcessingCenter.processingCenterID>>, And<CCProcessingCenter.isActive, Equal<True>>>>.Config>.Select((PXGraph) this, (object) aProcessingCenterID))
      {
        CCProcessingCenterDetail processingCenterDetail = (CCProcessingCenterDetail) pxResult;
        settings.Add(new SettingsValue()
        {
          DetailID = processingCenterDetail.DetailID,
          Value = processingCenterDetail.Value
        });
      }
      return (IEnumerable<SettingsValue>) settings;
    }

    protected virtual void DDResponseFilter_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
    {
      if (!(e.Row is DDResponseFilter row))
        return;
      CurrencyInfo currencyInfo = (CurrencyInfo) this.CurrencyInfo_CuryInfoID.Select((object) row.CuryInfoID);
    }

    protected virtual void DDResponseFilter_CustomerID_FieldUpdated(
      PXCache sender,
      PXFieldUpdatedEventArgs e)
    {
      this.ARDocumentList.Cache.Clear();
    }

    protected virtual void DDResponseFilter_PayDate_FieldUpdated(
      PXCache sender,
      PXFieldUpdatedEventArgs e)
    {
      foreach (PXResult<CurrencyInfo> data in PXSelectBase<CurrencyInfo, PXSelect<CurrencyInfo, Where<CurrencyInfo.curyInfoID, Equal<Current<DDResponseFilter.curyInfoID>>>>.Config>.Select((PXGraph) this, (object[]) null))
        this.currencyinfo.Cache.SetDefaultExt<CurrencyInfo.curyEffDate>((object) (CurrencyInfo) data);
      this.ARDocumentList.Cache.Clear();
    }

    protected virtual void DDResponseFilter_PaymentMethodID_FieldUpdated(
      PXCache sender,
      PXFieldUpdatedEventArgs e)
    {
      this.ARDocumentList.Cache.Clear();
    }

    protected virtual void DDResponseFilter_ProcessingCenterID_FieldUpdated(
      PXCache sender,
      PXFieldUpdatedEventArgs e)
    {
      this.ARDocumentList.Cache.Clear();
    }

    protected virtual void ARPaymentInfo_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
    {
      PXUIFieldAttribute.SetEnabled<ARPayment.docType>(sender, e.Row, false);
      PXUIFieldAttribute.SetEnabled<ARPayment.refNbr>(sender, e.Row, false);
    }

    private static void ChangeRefundTranType(IBqlTable aTable, PX.CCProcessingBase.CCTranType procTran, bool success)
    {
      ICCPayment ccPayment = aTable as ICCPayment;
      if (!(ccPayment != null & success))
        return;
      ARPaymentEntry instance = PXGraph.CreateInstance<ARPaymentEntry>();
      CCProcTran ccProcTran = new PXSelect<CCProcTran, Where<CCProcTran.docType, Equal<Required<CCProcTran.docType>>, And<CCProcTran.refNbr, Equal<Required<CCProcTran.refNbr>>>>, OrderBy<Desc<CCProcTran.tranNbr>>>((PXGraph) instance).SelectSingle((object) ccPayment.DocType, (object) ccPayment.RefNbr);
      ccProcTran.TranType = "AUT";
      ccProcTran.TranStatus = "HFR";
      instance.ccProcTran.Cache.Update((object) ccProcTran);
      instance.ccProcTran.Cache.Persist(PXDBOperation.Update);
    }
  }
}
