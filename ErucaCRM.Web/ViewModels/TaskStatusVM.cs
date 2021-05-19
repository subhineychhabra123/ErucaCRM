using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "TaskStatus")]
    public class TaskStatusVM
    {
        public int TaskStatusId { get; set; }
        public String StatusName { get; set; }
    }
}