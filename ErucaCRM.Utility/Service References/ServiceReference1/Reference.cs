﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErucaCRM.Utility.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendEmail", ReplyAction="http://tempuri.org/IService1/SendEmailResponse")]
        bool SendEmail(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendEmail", ReplyAction="http://tempuri.org/IService1/SendEmailResponse")]
        System.Threading.Tasks.Task<bool> SendEmailAsync(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : ErucaCRM.Utility.ServiceReference1.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<ErucaCRM.Utility.ServiceReference1.IService1>, ErucaCRM.Utility.ServiceReference1.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool SendEmail(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml) {
            return base.Channel.SendEmail(ToEmail, Subject, MessageBody, IsBodyHtml);
        }
        
        public System.Threading.Tasks.Task<bool> SendEmailAsync(string ToEmail, string Subject, string MessageBody, bool IsBodyHtml) {
            return base.Channel.SendEmailAsync(ToEmail, Subject, MessageBody, IsBodyHtml);
        }
    }
}
