using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace EmployeeService
{
    public class EmployeeSecurity
    {
        public static EmployeeEntities db = new EmployeeEntities();

        public static bool Login(string username, string password)
        {
            return db.Users.Any(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                                        user.Password == password);
        }
    }
}