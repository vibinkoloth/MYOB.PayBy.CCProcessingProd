// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.PAYBY.DI.MACreditCardData
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using MYOB.PayBy.CCProcessing.Common;
using PX.CCProcessingBase.Interfaces.V2;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MYOB.PayBy.CCProcessing.PAYBY.DI
{
  public class MACreditCardData : IMACreditCardData
  {
    public CreditCardData GetCardData(ProcessingInput aInput)
    {
      CreditCardData cardData = new CreditCardData();
      KeyValuePair<string, string> keyValuePair = this.GetCPMDetail(aInput).Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (o => o.Key == "EXPDATE")).FirstOrDefault<KeyValuePair<string, string>>();
      if (!string.IsNullOrEmpty(keyValuePair.Value))
        cardData.CardExpirationDate = new DateTime?(PayByPluginHelper.Expiration(keyValuePair.Value, out string _));
      return cardData;
    }

    public Dictionary<string, string> GetCPMDetail(ProcessingInput aInput)
    {
      Dictionary<string, string> cpmDetail = new Dictionary<string, string>();
      PXResultset<CustomerPaymentMethodDetail> pxResultset = PXSelectBase<CustomerPaymentMethodDetail, PXSelectJoin<CustomerPaymentMethodDetail, InnerJoin<PaymentMethodDetail, On<CustomerPaymentMethodDetail.detailID, Equal<PaymentMethodDetail.detailID>, And<CustomerPaymentMethodDetail.paymentMethodID, Equal<PaymentMethodDetail.paymentMethodID>>>, InnerJoin<ARPayment, On<ARPayment.pMInstanceID, Equal<CustomerPaymentMethodDetail.pMInstanceID>>, InnerJoin<CustomerPaymentMethod, On<CustomerPaymentMethod.pMInstanceID, Equal<CustomerPaymentMethodDetail.pMInstanceID>>>>>, Where<ARPayment.refNbr, Equal<Required<ARPayment.refNbr>>, And<ARPayment.docType, Equal<Required<ARPayment.docType>>>>>.Config>.Select((PXGraph) PXGraph.CreateInstance<CustomerPaymentMethodMaint>(), (object) aInput.DocumentData.DocRefNbr, (object) aInput.DocumentData.DocType);
      if (pxResultset != null)
      {
        foreach (PXResult<CustomerPaymentMethodDetail, PaymentMethodDetail, ARPayment, CustomerPaymentMethod> pxResult in pxResultset)
        {
          PaymentMethodDetail paymentMethodDetail1 = (PaymentMethodDetail) pxResult;
          CustomerPaymentMethodDetail paymentMethodDetail2 = (CustomerPaymentMethodDetail) pxResult;
          CustomerPaymentMethod customerPaymentMethod = (CustomerPaymentMethod) pxResult;
          Dictionary<string, string> dictionary = cpmDetail;
          DateTime? expirationDate = customerPaymentMethod.ExpirationDate;
          DateTime dateTime;
          string str;
          if (!expirationDate.HasValue)
          {
            dateTime = DateTime.Now;
            dateTime = dateTime.AddYears(1);
            str = dateTime.ToString("MM/yy");
          }
          else
          {
            expirationDate = customerPaymentMethod.ExpirationDate;
            dateTime = expirationDate.Value;
            dateTime = dateTime.AddDays(-1.0);
            str = dateTime.ToString("MM/yy");
          }
          dictionary["EXPDATE"] = str;
          bool? nullable = paymentMethodDetail1.IsCCProcessingID;
          bool flag1 = true;
          if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
            cpmDetail["CCPID"] = paymentMethodDetail2.Value;
          nullable = paymentMethodDetail1.IsIdentifier;
          bool flag2 = true;
          if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
            cpmDetail["CCDNUM"] = paymentMethodDetail2.Value;
          nullable = paymentMethodDetail1.IsCVV;
          bool flag3 = true;
          if (nullable.GetValueOrDefault() == flag3 & nullable.HasValue)
            cpmDetail["CVV"] = paymentMethodDetail2.Value;
          nullable = paymentMethodDetail1.IsOwnerName;
          bool flag4 = true;
          if (nullable.GetValueOrDefault() == flag4 & nullable.HasValue)
            cpmDetail["NAMEONCC"] = paymentMethodDetail2.Value;
        }
      }
      return cpmDetail;
    }
  }
}
