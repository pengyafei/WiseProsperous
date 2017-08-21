using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RZXK.Dal;
using RZXK.Model;

namespace RZXK.Bll
{
   public  class AdminBll
    {
       AdminDal adminDal = new AdminDal();
       public AdminModel getAdminModelByUserName(string userName) {
           return adminDal.getAdminModelByUserName(userName);
       }
    }
}
