using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ErucaCRM.Utility
{
    public class ReadConfiguration
    {
        public static string SiteUrl;
        static ReadConfiguration()
        {
            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        }

        public static string DefaultUserCulture
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultUserCulture"];

            }
        }
        public static bool DefaultMailing
        {
            get
            {
                try
                {

                    return bool.Parse(ConfigurationManager.AppSettings["DefaultMailing"]);
                }
                catch
                {
                    return true;
                }

             
            }
        }

        public static string EMailID
        {
            get
            {
                return ConfigurationManager.AppSettings["EMailID"];

            }
        }
        public static string EMailName
        {
            get
            {
                return ConfigurationManager.AppSettings["EMailName"];

            }
        }
        public static string SMTPHostName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPHostName"];

            }
        }
        public static bool SMTPEnableSSL
        {
            get
            {
                bool enableSSL = false;
                bool.TryParse(ConfigurationManager.AppSettings["EnableSSL"], out enableSSL);
                return enableSSL;
            }
        }
        public static string SMTPUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPUserName"];

            }
        }
        public static string SMTPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPPassword"];

            }
        }
        public static int ProfileImageWidth
        {
            get
            {
                int width = 0;
                int.TryParse(ConfigurationManager.AppSettings["ProfileThumbnailResizeWidth"], out width);
                return width;
            }
        }
        public static int ProfileImageHieght
        {
            get
            {
                int height = 0;
                int.TryParse(ConfigurationManager.AppSettings["ProfileThumbnailResizeHeight"], out height);
                return height;
            }
        }
        public static int PageSize
        {
            get
            {
                int pageSize;
                int.TryParse(ConfigurationManager.AppSettings["PageSize"], out pageSize);
                return pageSize;

            }
        }
        public static int NotificationListPageSize
        {
            get
            {
                int pageSize;
                int.TryParse(ConfigurationManager.AppSettings["NotificationListPageSize"], out pageSize);
                return pageSize;
            }
        }
        public static int StageWidth
        {
            get
            {
                int stageWidth;
                int.TryParse(ConfigurationManager.AppSettings["StageWidth"], out stageWidth);
                return stageWidth;

            }
        }

        public static string CultureExcelFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["CultureExcelFilePath"];
            }
        }
        public static string CultureJSFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["CultureJSFilePath"];
            }
        }
        public static string CultureHelpJSFilePath
        {
            get { return ConfigurationManager.AppSettings["CultureHelpJSFilePath"]; }
        }

        public static int SubAdminProfileID
        {
            get
            {
               int profileId = 0;
               int.TryParse(ConfigurationManager.AppSettings["Sub-AdminProfileId"],out profileId);
               return profileId;
            }
        }

        public static string WebsiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteUrl"];
            }
        }

        public static string WebsiteLogoPath
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteLogoPath"];
            }
        }
        public static string BulkUploadContactFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["BulkUploadContactFilePath"];
            }
        }

        public static string LeadDocumentPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LeadDocumntPath"];
            }
        }


        public static string DownLoadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DownLoadPath"];
            }
        }

        public static string ContactDocumntPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ContactDocumntPath"];
            }
        }

        public static string ActivityDocumntPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ActivityDocumntPath"];
            }
        }

        public static string AccountDocumntPath
        {
            get
            {
                return ConfigurationManager.AppSettings["AccountDocumntPath"];
            }
        }
        public static string LeadAudioPath
        {
            get { 
            return ConfigurationManager.AppSettings["LeadAudioPath"];
            }
        }
        public static string CommentAudioPath
        {
            get
            {
                return ConfigurationManager.AppSettings["CommentAudioPath"];
            }
        }
        public static string TaskItemAudioPath
        {
            get {
                return ConfigurationManager.AppSettings["TaskItemAudioPath"];
            }
        }

        public static string UsersImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["UsersImageUrl"];
            }
        }


        public static string EmailForScheduler
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailForScheduler"];
            }
        }

        public static string MailTemplateFolder { get { return ConfigurationManager.AppSettings["MailTemplateFolder"]; } }
        public static string EnglishMailTemplateFile { get { return ConfigurationManager.AppSettings["EnglishMailTemplateFile"]; } }
        public static string ChineseMailTemplateFile { get { return ConfigurationManager.AppSettings["ChineseMailTemplateFile"]; } }
        public static string ErucaCRMURL
        {
            get
            {
                return ConfigurationManager.AppSettings["ErucaCRMURL"];

            }
        }
        public static string OwnerDetail
        {
            get
            {
                return ConfigurationManager.AppSettings["OwnerDetail"].ToString();
            }
        }
        public static string UploadFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadFilePath"].ToString();
            }
        }
        public static string NoImage
        {
            get
            {
                return ConfigurationManager.AppSettings["NoImage"].ToString();
            }
        }
        public static string PopUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PopUrl"].ToString();
            }
        }
        public static string CommentsClipsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["CommentsClipsPath"];
            }
        }
    }
}
