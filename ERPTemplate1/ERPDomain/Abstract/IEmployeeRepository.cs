﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Employee { get; }
        void SaveEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
