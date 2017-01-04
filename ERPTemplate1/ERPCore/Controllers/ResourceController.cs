using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Web;
using System.Web.Mvc;
using ERPDomain.Models;
using ERPDomain.Abstract;
using ERPDomain.Logs;

namespace ERPCore.Controllers
{
    public class ResourceController : BaseController
    {
        // GET: Resource

        public ResourceController(IErrorLogRepository errorLogRepo) : base(errorLogRepo)
        {

        }
        public ActionResult Index()
        {
            try
            {
                // Create a instance of the ResourceReader and specify location of the resource file
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                System.Resources.ResourceReader RRobj = new ResourceReader(sBaseDir2 + "Resources.resx");

                // Read a resource file
                foreach (System.Collections.DictionaryEntry DE in RRobj)
                {
                    try
                    {
                        System.Console.WriteLine(DE.Key + "\t:\t" + DE.Value);
                    }
                    catch (Exception)
                    { }
                }

                // Close the ResourceReader
                RRobj.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        #region "Static Menu"
        public ActionResult Index_EN()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            ERPLog.WriteDebug("Index_EN");
            try
            {
                
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                ERPLog.WriteDebug("sBaseDir=" + sBaseDir);
                int iDirLen = sBaseDir.Length;
                ERPLog.WriteDebug("iDirLen=" + iDirLen);
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                ERPLog.WriteDebug("sBaseDir2=" + sBaseDir2);
                sBaseDir2 += "Resources\\";
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    ERPLog.WriteDebug("rm.Value=" + rm.Value);
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_EN(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "Resources.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }

        public ActionResult Index_ID()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            try
            {
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.id.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_ID(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//Localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.id.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "Resources.id.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }

        public ActionResult Index_ZH()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            try
            {
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.zh.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_ZH(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//Localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "Resources.zh.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "Resources.zh.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }
        #endregion

        #region "Plugin Menu"
        public ActionResult PluginIndex_EN()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            try
            {

                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PluginIndex_EN(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "ResourcesPlugin.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }

        public ActionResult PluginIndex_ID()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            try
            {
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.id.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PluginIndex_ID(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//Localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.id.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "ResourcesPlugin.id.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }

        public ActionResult PluginIndex_ZH()
        {
            List<ResourceModel> listResource = new List<ResourceModel>();
            try
            {
                String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                int iDirLen = sBaseDir.Length;
                String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
                sBaseDir2 += "Resources\\";
                //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
                String sBaseDir3 = String.Empty;
                sBaseDir += "Resources\\";
                if (sBaseDir.ToLower().Contains("wwwroot"))
                {
                    sBaseDir3 = sBaseDir;//IIS deploy path
                }
                else
                {
                    sBaseDir3 = sBaseDir2;//Localhost path
                }
                ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
                ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.zh.resx");
                IDictionaryEnumerator en = reader.GetEnumerator();
                while (en.MoveNext())
                {
                    ResourceModel rm = new ResourceModel();
                    rm.Name = en.Key.ToString();
                    rm.Value = en.Value.ToString();
                    listResource.Add(rm);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return View(listResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PluginIndex_ZH(List<ResourceModel> resourceModel)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            String sBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            int iDirLen = sBaseDir.Length;
            String sBaseDir2 = sBaseDir.Substring(0, iDirLen - 8);
            sBaseDir2 += "Resources\\";
            //ResXResourceReader reader = new ResXResourceReader(Server.MapPath("Default.aspx.resx"));
            String sBaseDir3 = String.Empty;
            sBaseDir += "Resources\\";
            if (sBaseDir.ToLower().Contains("wwwroot"))
            {
                sBaseDir3 = sBaseDir;//IIS deploy path
            }
            else
            {
                sBaseDir3 = sBaseDir2;//Localhost path
            }
            ERPLog.WriteDebug("sBaseDir3=" + sBaseDir3);
            ResXResourceReader reader = new ResXResourceReader(sBaseDir3 + "ResourcesPlugin.zh.resx");
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            foreach (var item in resourceModel)
            {
                if (!resourceEntries.ContainsKey(item.Name))
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Add(item.Name, value);
                }
                else
                {
                    String value = item.Value;
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(item.Name);
                    resourceEntries.Add(item.Name, value);
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(sBaseDir3 + "ResourcesPlugin.zh.resx");
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return View(resourceModel);
        }
        #endregion
    }
}