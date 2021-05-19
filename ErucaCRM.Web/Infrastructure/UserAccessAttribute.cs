using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Net;
using System.ServiceModel.Channels;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Business;
using ErucaCRM.Domain;
using ErucaCRM.WCFService.Models;

namespace ErucaCRM.WCFService.Infrastructure
{
    public class UserAccessAttribute : Attribute, IOperationBehavior, IOperationInvoker
    {

        #region Private Fields

        private IOperationInvoker _invoker;

        #endregion


        private string _userPermission;
        public UserAccessAttribute()
        {
            _userPermission = string.Empty;

            //you could also put your role validation code in here

        }
        public UserAccessAttribute(string userRole)
        {
            _userPermission = userRole;

            //you could also put your role validation code in here

        }

        public string GetUserRole()
        {
            return _userPermission;
        }
        #region IOperationBehavior Members
        public void ApplyDispatchBehavior(OperationDescription operationDescription,
                                          DispatchOperation dispatchOperation)
        {
            _invoker = dispatchOperation.Invoker;
            dispatchOperation.Invoker = this;
        }

        public void ApplyClientBehavior(OperationDescription operationDescription,
                                        ClientOperation clientOperation)
        {
        }

        public void AddBindingParameters(OperationDescription operationDescription,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {
            //   Authenticate("Client Name here");
        }

        #endregion

        #region IOperationInvoker Members

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            if (AuthenticateUserAccess())
                return _invoker.Invoke(instance, inputs, out outputs);
            else
            {
                outputs = null;
                return null;
            }
        }

        public object[] AllocateInputs()
        {
            return _invoker.AllocateInputs();
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs,
                                        AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public bool IsSynchronous
        {
            get
            {
                return true;
            }
        }

        #endregion

        private bool AuthenticateUserAccess()
        {
            string UserToken = GetToken(WebOperationContext.Current.IncomingRequest.Headers);

            if (!String.IsNullOrEmpty(UserToken))
            {
                IUnitOfWork unitOfWork = new UnitOfWork();
                UserBusiness userBusiness = new UserBusiness(unitOfWork);
                string moduleCONSTANT = string.Empty;
                string permissionCONSTANT = string.Empty;
                if (!string.IsNullOrEmpty(_userPermission))
                {
                    string[] permission = _userPermission.Split('|');
                    moduleCONSTANT = permission[0];
                    permissionCONSTANT = permission[1];
                }
                UserAccess userAccess = userBusiness.ValidateUserAccess(UserToken, moduleCONSTANT, permissionCONSTANT);
                if (!userAccess.IsValidToken)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    throw new WebFaultException<ErrorMessage>(new ErrorMessage { Message = "Not Authorized.", StatusCode = HttpStatusCode.Unauthorized.ToString() },
                    HttpStatusCode.OK);
                    return false;
                }
                if (!userAccess.hasMethodPermission && !string.IsNullOrEmpty(moduleCONSTANT))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                    throw new WebFaultException<ErrorMessage>(new ErrorMessage { Message = "Not sufficient permissions.", StatusCode = HttpStatusCode.MethodNotAllowed.ToString() },
                  HttpStatusCode.OK);
                    return false;
                }
                return true;
            }
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;

            return false;
        }

        private string GetToken(WebHeaderCollection headers)
        {
            string userToken = WebOperationContext.Current.IncomingRequest.Headers["token"];

            if (userToken != null)
                userToken = userToken.Trim();
            if (!string.IsNullOrEmpty(userToken))
            {
                return userToken;
            }

            return null;
        }
    }
}
