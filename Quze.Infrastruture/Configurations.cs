namespace Quze.Infrastruture
{
    public static class Configurations
    {
        private readonly static string emailPassword = "GmailQuze!Reply";
        public static string EmailPassword => emailPassword;

        private static string sendingEmailFromAddress = "norply.quze@gmail.com";
        public static string SendingEmailFromAddress => sendingEmailFromAddress;
    }
}
