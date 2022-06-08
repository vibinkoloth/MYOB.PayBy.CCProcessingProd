// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.Common.operationEnum
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

namespace MYOB.PayBy.CCProcessing.Common
{
  public enum operationEnum
  {
    PAYMENT_INIT,
    PAYMENT_COMPLETE,
    PAYMENT_REAL_TIME,
    PAYMENT_BATCH,
    PAYMENT_BBPOS,
    VAULT_STORE_CARD,
    VAULT_DELETE_TOKEN,
    VAULT_RETRIEVE_CARD,
    VAULT_UPDATE_CARD,
    VAULT_VERIFY_TOKEN,
  }
}
