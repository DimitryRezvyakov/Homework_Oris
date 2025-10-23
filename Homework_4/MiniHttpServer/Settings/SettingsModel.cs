using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Settings
{
    public class SettingsModel
    {
        public required string Port { get; set; }
        public required string Domain { get; set; }
        public required string DomainPath { get; set; }
        public string? Prefix { get; set; }
    }
}
