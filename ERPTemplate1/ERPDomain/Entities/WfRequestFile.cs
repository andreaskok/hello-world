using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfRequestFile
    {
        public long ID { get; set; }
        public long WfRequestID { get; set; } //Foreign key to parent table WfRequest
        public int SH_USERID { get; set; } //Foreign key to parent table SH_USER
        public DateTime DateUploaded { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent {get;set;}
        public string MIMEType { get; set; }
        public string FilePath { get; set; }
        public virtual WfRequest WfRequest { get; set; }//Navigation property to parent table WfRequest
        public virtual SH_USER SH_USER { get; set; } //Navigation proprty to parent table SH_USER

    }
}
