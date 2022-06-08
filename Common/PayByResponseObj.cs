// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.PayByResponseObj
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.Common
{
  public class PayByResponseObj
  {
    internal List<PayByResponse> responseList;

    public PayByResponseObj() => this.responseList = new List<PayByResponse>();

    internal void addResponseValue(string key, string value) => this.responseList.Add(new PayByResponse()
    {
      Key = key,
      Value = value
    });

    public string getResponseValue(string key) => this.responseList.Where<PayByResponse>((Func<PayByResponse, bool>) (o => o.Key == key)).Any<PayByResponse>() ? this.responseList.Where<PayByResponse>((Func<PayByResponse, bool>) (o => o.Key == key)).Select<PayByResponse, string>((Func<PayByResponse, string>) (o => o.Value)).FirstOrDefault<string>() : (string) null;
  }
}
