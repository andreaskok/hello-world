using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFPluginAreaRepository : IPluginAreaRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<PluginArea> PluginArea
        {

            get { return context.PluginArea; }

        }

        public void SavePluginArea(PluginArea pluginArea)
        {
            if (pluginArea.ID == 0)
            {
                context.PluginArea.Add(pluginArea);
            }
            else
            {
                context.Entry(pluginArea).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeletePluginArea(PluginArea pluginArea)
        {
            context.PluginArea.Remove(pluginArea);
            context.SaveChanges();
        }
    }
}
