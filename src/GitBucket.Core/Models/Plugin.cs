using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Plugin
{
    public string PluginId { get; set; } = null!;

    public string Version { get; set; } = null!;
}
