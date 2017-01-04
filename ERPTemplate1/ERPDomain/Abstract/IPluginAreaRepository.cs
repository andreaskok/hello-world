using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IPluginAreaRepository
    {
        IEnumerable<PluginArea> PluginArea { get; }

        void SavePluginArea(PluginArea pluginArea);
        void DeletePluginArea(PluginArea pluginArea);
    }
}
