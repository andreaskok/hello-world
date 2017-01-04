using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Calendar
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 CountryStateID { get; set; } //Foreign Key
        public DateTime CalendarDate { get; set; }
        public Int64 CalendarYear { get; set; }
        public Int64 CalendarMonth { get; set; }
        public Int64 CalendarDay { get; set; }
        public Int64 DayOfWeekName { get; set; }
        public DateTime FirstDateOfWeek { get; set; }
        public DateTime LastDateOfWeek { get; set; }
        public DateTime FirstDateOfMonth { get; set; }
        public DateTime LastDateOfMonth { get; set; }
        public DateTime FirstDateOfQuarter { get; set; }
        public DateTime LastDateOfQuarter { get; set; }
        public DateTime FirstDateOfYear { get; set; }
        public DateTime LastDateOfYear { get; set; }
        public bool IsBusinessDay { get; set; }
        public bool Weekend { get; set; }
        public bool Holiday { get; set; }
        public bool Weekday { get; set; }
        public String CalendarDateDescription { get; set; }

        public virtual CountryState CountryState { get; set; }

    }
}
