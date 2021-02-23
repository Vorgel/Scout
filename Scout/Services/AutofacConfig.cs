using Autofac;
using Scout.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Services
{
    class AutofacConfig
    {
        private readonly OutputProvider outputProvider;

        public AutofacConfig(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }

        public IEnumerable<IOperation> GetContainer()
        {
            var container = this.Configure();

            return container.Resolve<IEnumerable<IOperation>>();
        }

        private IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.outputProvider).As<OutputProvider>();
            builder.RegisterType<OSInfoOperation>().As<IOperation>();

            return builder.Build();
        }
    }
}
