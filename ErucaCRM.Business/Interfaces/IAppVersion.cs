using ErucaCRM.Domain;
using ErucaCRM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
  public  interface IAppVersion
    {

        AppVersionModel GetVersion();
    }
}
