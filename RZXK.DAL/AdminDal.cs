using RZXK.Common;
using RZXK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RZXK.Dal
{
   public class AdminDal
    {
       public AdminModel getAdminModelByUserName(string userName) 
       {
           string sql = string.Format("select * from  sys_admin sa where sa.userName='{0}'",userName);
         return DataTableHelper.ToList<AdminModel>( DataClass_Mysql.ExecuteDataTable(sql)).FirstOrDefault();
       }
    }
}
