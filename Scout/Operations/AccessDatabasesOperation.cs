using Scout.Services;
using System.Threading.Tasks;

namespace Scout.Operations
{
    public class AccessDatabasesOperation : IOperation
    {
        private readonly OutputProvider outputProvider;

        public AccessDatabasesOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }
        public async Task Run()
        {
            await outputProvider.GetAppFiles(".el");
        }
    }
}
