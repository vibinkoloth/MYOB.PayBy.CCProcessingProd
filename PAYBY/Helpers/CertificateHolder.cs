// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.PAYBY.Helpers.CertificateHolder
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;

namespace MYOB.PayBy.CCProcessing.PAYBY.Helpers
{
  internal static class CertificateHolder
  {
    private static List<X509Certificate> trustedSelfSignedCerts = new List<X509Certificate>();
    private static bool inited = false;

    public static bool ValidateServerCertficate(
      object sender,
      X509Certificate cert,
      X509Chain chain,
      SslPolicyErrors sslPolicyErrors)
    {
      if (sslPolicyErrors == SslPolicyErrors.None)
        return true;
      if (!CertificateHolder.inited)
      {
        try
        {
          string filename1 = HostingEnvironment.MapPath("~/Content/payby.cer");
          CertificateHolder.trustedSelfSignedCerts.Add(X509Certificate.CreateFromCertFile(filename1));
          string filename2 = HostingEnvironment.MapPath("~/Content/payby_test.cer");
          CertificateHolder.trustedSelfSignedCerts.Add(X509Certificate.CreateFromCertFile(filename2));
        }
        catch (Exception ex)
        {
        }
        CertificateHolder.inited = true;
      }
      return CertificateHolder.trustedSelfSignedCerts.Any<X509Certificate>((Func<X509Certificate, bool>) (c => c.Equals(cert)));
    }
  }
}
