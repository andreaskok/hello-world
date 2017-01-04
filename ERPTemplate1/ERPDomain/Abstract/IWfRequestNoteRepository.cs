using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfRequestNoteRepository
    {
        IEnumerable<WfRequestNote> WfRequestNote { get; }
        void SaveWfRequestNote(WfRequestNote wfRequestNote);
        void DeleteWfRequestNote(WfRequestNote wfRequestNote);
    }
}
