using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginAR.Models
{
    public class ExcelCart
    {
        private List<DebitNoteLineExcelModel> excelLineCollection = new List<DebitNoteLineExcelModel>();
        public void AddExcelLine(DebitNoteLineExcelModel excelLine)
        {
            DebitNoteLineExcelModel line = excelLineCollection
                .Where(p => p.ID == excelLine.ID)
                .FirstOrDefault();
            if (line == null)
            {
                excelLineCollection.Add(excelLine);
            }
        }

        public void RemoveExcelLine(DebitNoteLineExcelModel excelLine)
        {
            excelLineCollection.RemoveAll(l => l.ID ==
            excelLine.ID);
        }

        public void RemoveExcelLine2(Int64 recordId)
        {
            excelLineCollection.RemoveAll(l => l.ID ==
            recordId);
        }
        public void Clear()
        {
            excelLineCollection.Clear();
        }

        public IEnumerable<DebitNoteLineExcelModel> ExcelLines
        {
            get { return excelLineCollection; }
        }
    }
}