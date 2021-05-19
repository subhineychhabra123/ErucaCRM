using ErucaCRM.WCFService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Services.Description;

namespace ErucaCRM.WCFService.Infrastructure
{

    public sealed class AutomapServiceBehavior : Attribute, System.ServiceModel.Description.IServiceBehavior
    {
        public AutomapServiceBehavior()
        {
        }


        public void AddBindingParameters(System.ServiceModel.Description.ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<System.ServiceModel.Description.ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            AutoMapperWCFProfile.Run();
        }

        public void ApplyDispatchBehavior(System.ServiceModel.Description.ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        public void Validate(System.ServiceModel.Description.ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }


    }
}