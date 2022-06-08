// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.ARRegisterExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Objects.AR;

namespace MYOB.PayBy.CCProcessing
{
  public class ARRegisterExt : PXCacheExtension<ARRegister>
  {
    [PXDBBool]
    [PXUIField(DisplayName = "IsDDPaymentCancel")]
    public virtual bool? UsrIsDDPaymentCancel { get; set; }

    public abstract class usrIsDDPaymentCancel : IBqlField, IBqlOperand
    {
    }
  }
}
