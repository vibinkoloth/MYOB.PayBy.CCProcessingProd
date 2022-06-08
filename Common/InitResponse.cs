// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.InitResponse
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using System;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class InitResponse
  {
    public string reqid { get; set; }

    public DateTime expireAt { get; set; }

    public string paymentPageUrl { get; set; }
  }
}
