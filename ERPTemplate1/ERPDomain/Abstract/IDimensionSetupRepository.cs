using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IDimensionSetupRepository
    {
        IEnumerable<DimensionSetup> DimensionSetup { get; }
        IEnumerable<DimensionSetup> DimensionSetupWildSearch(string fieldName, string fieldValue);
        void SaveDimensionSetup(DimensionSetup dimensionsetup);
        void DeleteDimensionSetup(DimensionSetup dimensionsetup);
    }
}
