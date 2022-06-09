// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.CustomerPaymentMethodExt
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;

namespace MYOB.PayBy.CCProcessing
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class CustomerPaymentMethodExt : PXCacheExtension<CustomerPaymentMethod>
  {
    [PXString(10)]
    [PXUIField(DisplayName = "Expiration Date", Enabled = false)]
    public virtual string UsrExpirationDate { get; set; }

    #region UsrPayByRequestID
    [PXDBString(128)]
    [PXUIField(DisplayName = "UsrPayByRequestID")]
    public virtual string UsrPayByRequestID { get; set; }
    public abstract class usrPayByRequestID : PX.Data.BQL.BqlString.Field<usrPayByRequestID> { }
    #endregion


    #region UsrCCToken
    [PXDBString(50)]
    [PXUIField(DisplayName = "UsrCCToken")]
    public virtual string UsrCCToken { get; set; }
    public abstract class usrCCToken : PX.Data.BQL.BqlString.Field<usrCCToken> { }
    #endregion

        public abstract class usrExpirationDate : 
      BqlType<IBqlString, string>.Field<CustomerPaymentMethodExt.usrExpirationDate>
    {
    }
  }
}
