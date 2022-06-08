// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.DDResponseFilter
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Objects.AR;
using PX.Objects.CA;
using PX.Objects.CM;
using System;

namespace MYOB.PayBy.CCProcessing.Common
{
  [Serializable]
  public class DDResponseFilter : IBqlTable
  {
    protected DateTime? _PayDate;
    protected string _StatementCycleId;
    protected int? _CustomerID;
    protected Decimal? _Balance;
    protected Decimal? _CurySelTotal;
    protected Decimal? _SelTotal;
    protected string _CuryID;
    protected long? _CuryInfoID;
    protected string _ProcessingCenterID;
    protected string _PaymentMethodID;

    [PXDBDate]
    [PXDefault(typeof (AccessInfo.businessDate))]
    [PXUIField(DisplayName = "Payment Date Before", Visibility = PXUIVisibility.Visible)]
    public virtual DateTime? PayDate
    {
      get => this._PayDate;
      set => this._PayDate = value;
    }

    [PXDBString(10, IsUnicode = true)]
    [PXUIField(DisplayName = "Statement Cycle ID")]
    [PXSelector(typeof (ARStatementCycle.statementCycleId))]
    public virtual string StatementCycleId
    {
      get => this._StatementCycleId;
      set => this._StatementCycleId = value;
    }

    [Customer(DescriptionField = typeof (Customer.acctName), Required = false, Visibility = PXUIVisibility.SelectorVisible)]
    [PXDefault]
    public virtual int? CustomerID
    {
      get => this._CustomerID;
      set => this._CustomerID = value;
    }

    [PXDefault(TypeCode.Decimal, "0.0")]
    [PXDBDecimal(4)]
    [PXUIField(DisplayName = "Balance", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual Decimal? Balance
    {
      get => this._Balance;
      set => this._Balance = value;
    }

    [PXDefault(TypeCode.Decimal, "0.0")]
    [PXDBCurrency(typeof (DDResponseFilter.curyInfoID), typeof (DDResponseFilter.selTotal))]
    [PXUIField(DisplayName = "Selection Total", Enabled = false, Visibility = PXUIVisibility.SelectorVisible)]
    public virtual Decimal? CurySelTotal
    {
      get => this._CurySelTotal;
      set => this._CurySelTotal = value;
    }

    [PXDBDecimal(4)]
    public virtual Decimal? SelTotal
    {
      get => this._SelTotal;
      set => this._SelTotal = value;
    }

    [PXDBString(5, InputMask = ">LLLLL", IsUnicode = true)]
    [PXUIField(DisplayName = "Currency", Enabled = false, Visibility = PXUIVisibility.SelectorVisible, Visible = false)]
    [PXSelector(typeof (PX.Objects.CM.Currency.curyID))]
    public virtual string CuryID
    {
      get => this._CuryID;
      set => this._CuryID = value;
    }

    [PXDBLong]
    [CurrencyInfo(ModuleCode = "AP")]
    public virtual long? CuryInfoID
    {
      get => this._CuryInfoID;
      set => this._CuryInfoID = value;
    }

    [PXDefault(typeof (Search<CCProcessingCenter.processingCenterID, Where<CCProcessingCenter.processingTypeName, Equal<ConstantCommon.dDProcTypeNameV2>>>))]
    [PXDBString(10, IsUnicode = true)]
    [PXSelector(typeof (Search<CCProcessingCenter.processingCenterID, Where<CCProcessingCenter.processingTypeName, Equal<ConstantCommon.dDProcTypeNameV2>>>), DescriptionField = typeof (CCProcessingCenter.name))]
    [PXUIField(DisplayName = "Processing Center ID")]
    public virtual string ProcessingCenterID
    {
      get => this._ProcessingCenterID;
      set => this._ProcessingCenterID = value;
    }

    [PXDBString(10, IsUnicode = true)]
    [PXUIField(DisplayName = "Payment Method", Visibility = PXUIVisibility.SelectorVisible)]
    [PXSelector(typeof (Search2<PaymentMethod.paymentMethodID, InnerJoin<CCProcessingCenterPmntMethod, On<PaymentMethod.paymentMethodID, Equal<CCProcessingCenterPmntMethod.paymentMethodID>>>, Where<PaymentMethod.isActive, Equal<True>, And<PaymentMethod.aRIsProcessingRequired, Equal<True>, And<PaymentMethod.paymentType, Equal<PaymentMethodType.creditCard>, And<CCProcessingCenterPmntMethod.processingCenterID, Equal<Current<DDResponseFilter.processingCenterID>>>>>>>), DescriptionField = typeof (PaymentMethod.descr))]
    public virtual string PaymentMethodID
    {
      get => this._PaymentMethodID;
      set => this._PaymentMethodID = value;
    }

    public abstract class payDate : IBqlField, IBqlOperand
    {
    }

    public abstract class statementCycleId : IBqlField, IBqlOperand
    {
    }

    public abstract class customerID : IBqlField, IBqlOperand
    {
    }

    public abstract class balance : IBqlField, IBqlOperand
    {
    }

    public abstract class curySelTotal : IBqlField, IBqlOperand
    {
    }

    public abstract class selTotal : IBqlField, IBqlOperand
    {
    }

    public abstract class curyID : IBqlField, IBqlOperand
    {
    }

    public abstract class curyInfoID : IBqlField, IBqlOperand
    {
    }

    public abstract class processingCenterID : IBqlField, IBqlOperand
    {
    }

    public abstract class paymentMethodID : IBqlField, IBqlOperand
    {
    }
  }
}
