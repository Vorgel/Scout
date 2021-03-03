using Autofac;
using Scout.Operations;
using System.Collections.Generic;

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
            builder.RegisterType<ConfigFilesOperation>().As<IOperation>();
            builder.RegisterType<LogFilesOperation>().As<IOperation>();
            builder.RegisterType<PowershellInfoOperation>().As<IOperation>();
            builder.RegisterType<PythonInfoOperation>().As<IOperation>();
            builder.RegisterType<JavaInfoOperation>().As<IOperation>();
            builder.RegisterType<AccessDatabasesOperation>().As<IOperation>();

            return builder.Build();
        }
    }
}
