using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RZXK.Model
{
   public class AdminModel
    {
       public int Id { get; set; }
       public string UserName { get; set; }

       public string RealName { get; set; }

       public string PassWord { get; set; }

       public string UserType { get; set; }

       public string Phone { get; set; }

       public string Address { get; set; }
       public string Status { get; set; }
    }
}
