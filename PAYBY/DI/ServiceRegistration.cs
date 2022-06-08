// Decompiled with JetBrains decompiler
// Type: MYOB.PayBy.CCProcessing.PAYBY.DI.ServiceRegistration
// Assembly: MYOB.PayBy.CCProcessing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6CF05C63-45B7-42BC-B793-82353CAC70B3
// Assembly location: C:\PayByCust\MAPayBy\Bin\MYOB.PayBy.CCProcessing.dll

using Autofac;
using System;

namespace MYOB.PayBy.CCProcessing.PAYBY.DI
{
  public class ServiceRegistration : Module
  {
    protected override void Load(ContainerBuilder builder) => builder.Register<MACreditCardData>((Func<IComponentContext, MACreditCardData>) (context => new MACreditCardData())).As<IMACreditCardData>().InstancePerLifetimeScope();
  }
}
