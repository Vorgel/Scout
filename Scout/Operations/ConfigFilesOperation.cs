﻿using Scout.Helpers;
using Scout.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Scout.Operations
{
    public class ConfigFilesOperation : IOperation
    {
        private readonly OutputProvider outputProvider;

        public ConfigFilesOperation(OutputProvider outputProvider)
        {
            this.outputProvider = outputProvider;
        }
        public async Task Run()
        {
            await outputProvider.GetAppFiles(".config");
        }
    }
}
