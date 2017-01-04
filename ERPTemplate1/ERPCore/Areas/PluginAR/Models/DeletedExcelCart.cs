using System.Collections.Generic;

namespace PluginAR.Models
{
    public class DeletedExcelCart
    {
        private List<DebitNoteLineExcelModel> excelLineCollection = new List<DebitNoteLineExcelModel>();
        public void Add2DeletedCart(DebitNoteLineExcelModel excelLine)
        {
            excelLineCollection.Add(excelLine);
        }

        public void Clear()
        {
            excelLineCollection.Clear();
        }

        public IEnumerable<DebitNoteLineExcelModel> GetDeletedExcelLines
        {
            get { return excelLineCollection; }
        }
    }
}