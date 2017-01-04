using System;

namespace ERPDomain.Entities
{
    public class SubMenu
    {
        public Int64 ID { get; set; }
        public Int64 ParentMenuID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string MethodName { get; set; }
        public bool Plugin { get; set; }
        public bool Buy { get; set; }
    }
}
