using Scout.Services;
using System.Threading.Tasks;

namespace Scout.Operations
{
    class LogFilesOperation : IOperation
    {
        private readonly OutputProvider outputProvider;

        public LogFilesOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }
        public async Task Run()
        {
            await outputProvider.GetAppFiles(".log");
        }
    }
}
