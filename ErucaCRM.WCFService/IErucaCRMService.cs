using ErucaCRM.Mobile.Models.ResponseModel;
using ErucaCRM.Mobile.Models.Model;
using ErucaCRM.Service.Infrastructure;
using ErucaCRM.Utility;
using ErucaCRM.WCFService.Infrastructure;
using ErucaCRM.Mobile.Models.RequestModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ErucaCRM.WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IErucaCRMService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ValidateUser")]
        ResponseUserLogedInfo ValidateUser(User user);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "RegisterUser")]
        User RegisterUser(User user);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetAutherizedModuleName")]
        Stream GetAutherizedModuleName(Authorization authorization);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTimeZones")]
        Stream GetTimeZones();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ForgetPassword")]
        Stream ForgetPassword(User user);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeads")]
        Stream GetLeads(RequestLead listInfo);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddLead")]
        ResponseLead AddLead(RequestLead listInfo);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadsByStages")]
        ResponseLead GetLeadsAndStages(RequestLead listInfo);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadsByStageId")]
        ResponseLead GetLeadsByStageId(RequestLead listInfo);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "MoveLeadToStage")]
        ResponseLead MoveLeadToStage(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Stages")]
        ResponseStages Stages(RequestStages requestStages);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadContacts")]
        ResponseContact GetLeadContacts(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddContact")]
        ResponseContact AddContact(RequestContact requestContact);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "DeleteLead")]
        ResponseLead DeleteLead(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadDetail")]
        ResponseLead GetLeadDetail(RequestLead requestLead);

        [OperationContract]
        //[TokenValidation]
        [UserAccess(Constants.MODULE_LEAD + "|" + Constants.PERMISSION_EDIT)]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetEditLeadDetail")]
        ResponseLead GetEditLeadDetail(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateLead")]
        ResponseLead UpdateLead(RequestLead requestLead);

      

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateLeadRating")]
        ResponseLead UpdateLeadRating(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadHistory")]
        ResponseLead GetLeadHistory(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadTasks")]
        ResponseLead GetLeadTasks(RequestLead requestLead);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetTasks")]
        ResponseLead GetTasks(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetLeadDocuments")]
        ResponseLead GetLeadDocuments(RequestLead requestLead);


        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetNonAssociatedLeadContacts")]
        ResponseContact GetNonAssociatedLeadContactList(RequestLead requestContact);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "IsLeadContactExists")]
        Stream IsLeadContactExists(RequestLead requestLead);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddLeadContacts")]
        ResponseContact AddLeadContacts(RequestContact requestContact);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetContacts")]
        ResponseContact GetContacts(RequestContact requestContact);


        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ActivityDetail")]
        ResponseActivity ActivityDetail(TaskItem requestContact);
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "UploadFiles/{fileName}", BodyStyle = WebMessageBodyStyle.Bare)]
        //string UploadFiles(string fileName, Stream fileContents);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetAddEditActivity")]
        ResponseActivity GetAddEditActivity(TaskItem requestActivity);


        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetAssociatedModuleValues")]
        ResponseActivity GetAssociatedModuleValues(TaskItem requestActivity);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddEditActivity")]
        ResponseActivity AddEditActivity(TaskItem requestActivity);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "DeleteActivity")]
        ResponseActivity DeleteActivity(TaskItem requestActivity);

        [OperationContract]
        [TokenValidation]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetAccounts")]
        ResponseLead GetAccounts(RequestLead requestLead);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "GetContactDetail")]
        ResponseContact GetContactDetail(RequestContact requestContact);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateContact")]
        ResponseContact UpdateContact(RequestContact requestContact);


        [OperationContract]
        // [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UserProfile")]
        ResponseUser UserProfile(UserProfile lead);

        // Use a data contract as illustrated in the sample below to add composite types to service operations.
        [OperationContract]
        // [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "EditUserProfile")]
        ResponseUser EditUserProfile(UserProfile user);

        [OperationContract]
        // [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateUserProfile")]
        ResponseUser UpdateUserProfile(UserProfile user);


        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UploadFiles")]
        ResponseStatus UploadFiles(Stream FileData);


        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadLeadFile")]
        Stream DownloadLeadFile(RequestLead requestLead);
       
        
        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "RemoveLeadFile")]
        ResponseStatus RemoveLeadDocument(RequestLead requestLead);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetRecentActivities")]
        ResponseActivity GetRecentActivities(RequestTaskItem requestTaskItem);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetDashboardData")]
        ResponseActivity GetDashboardData(RequestDashboardData requestDashboardData);

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetLeadsByStageGroup")]
        ResponseActivity GetLeadsByStageGroup(AllUserDetail UserAllDetails);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetAppVersion")]
        ResponseUserLogedInfo GetAppVersion();

        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteContact")]
        ResponseContact DeleteContact(RequestContact requestContact);


        [OperationContract]
        [UserAccess()]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "AddLeadComment")]
        ResponseActivity AddLeadComment(LeadComment leadComment);
    }
}
