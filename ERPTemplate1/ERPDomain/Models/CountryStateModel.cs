using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class CountryStateModel
    {
        public Country Country { get; set; }
        public IEnumerable<CountryState> CountryState { get; set; }
        public Currency Currency { get; set; }
    }
}
