using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IVendorRepository
    {
        IEnumerable<Vendor> Vendor { get; }
        IEnumerable<Vendor> VendorWildSearch(string fieldName, string fieldValue);
        void SaveVendor(Vendor vendor);
        void DeleteVendor(Vendor vendor);
    }
}
