using System;
using System.Collections.Generic;

namespace NotificationCenter.Core.Domain
{
    public sealed class App
    {
        /// <summary>
        /// Represents the ID of an App
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        public string AppName { get; set; }

        public List<string> PackageNames { get; set; }

        public List<Device> Devices { get; set; }

    }
}