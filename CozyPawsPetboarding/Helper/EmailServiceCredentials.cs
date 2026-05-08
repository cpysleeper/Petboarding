using System.Configuration;

namespace CozyPawsPetboarding
{
    public static class EmailServiceCredentials
    {
        public static string EmailSMTPUrl { get; private set; }
        public static string PortNumber { get; private set; }
        public static string EmailSMTPUserNameHash { get; private set; }
        public static string EmailSMTPPasswordHash { get; private set; }
        public static string EmailFromAddress { get; private set; }
        public static string EmailFromName { get; private set; }
        public static string EmailAppName { get; private set; }

        public static void SetCredentials(
            string emailSMTPUrl,
            string portNumber,
            string emailSMTPUserNameHash,
            string emailSMTPPasswordHash,
            string emailFromAddress,
            string emailFromName,
            string emailAppName)
        {
            EmailSMTPUrl = emailSMTPUrl;
            PortNumber = portNumber;
            EmailSMTPUserNameHash = emailSMTPUserNameHash;
            EmailSMTPPasswordHash = emailSMTPPasswordHash;
            EmailFromAddress = emailFromAddress;
            EmailFromName = emailFromName;
            EmailAppName = emailAppName;
        }

        public static void PopulateEmailCredentialsFromAppConfig()
        {
            string emailSMTPURL = ConfigurationManager.AppSettings["emailSMTPURL"];
            string portNumber = ConfigurationManager.AppSettings["PortNumber"];
            string emailSMTPUserNameHash = ConfigurationManager.AppSettings["emailSMTPUserNameHash"];
            string emailSMTPPasswordHash = ConfigurationManager.AppSettings["emailSMTPPasswordHash"];
            string emailFromAddress = ConfigurationManager.AppSettings["emailFromAddress"];
            string emailFromName = ConfigurationManager.AppSettings["emailFromName"];
            string emailAppName = ConfigurationManager.AppSettings["emailAppName"];

            SetCredentials(
                emailSMTPURL,
                portNumber,
                emailSMTPUserNameHash,
                emailSMTPPasswordHash,
                emailFromAddress,
                emailFromName,
                emailAppName
            );
        }
    }
}