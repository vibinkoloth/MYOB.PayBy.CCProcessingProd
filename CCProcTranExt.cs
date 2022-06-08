// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.CCProcTranExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Objects.AR;
using System;

namespace MYOB.PayBy.CCProcessing
{
  public class CCProcTranExt : PXCacheExtension<CCProcTran>
  {
    [PXDBDate(DisplayMask = "g", InputMask = "g", PreserveTime = true)]
    [PXDefault]
    [PXUIField(DisplayName = "Tran. Time")]
    public virtual DateTime? StartTime { get; set; }
  }
}
