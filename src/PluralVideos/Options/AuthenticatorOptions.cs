using CommandLine;

namespace PluralVideos.Options
{
    [Verb("auth", HelpText = "Authenticates the app to pluralsight")]
    public class AuthenticatorOptions
    {
        [Option("login", HelpText = "Login the App to Pluralsight")]
        public bool Login { get; set; }

        [Option("logout", HelpText = "Logout the App from Pluralsight")]
        public bool Logout { get; set; }
    }
}
