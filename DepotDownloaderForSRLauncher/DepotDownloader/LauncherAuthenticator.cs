// This file is subject to the terms and conditions defined
// in file 'LICENSE', which is part of this source code package.

using System;
using System.Threading.Tasks;
using DepotDownloader;

namespace SteamKit2.Authentication
{
    /// <summary>
    /// This is a default implementation of <see cref="IAuthenticator"/> to ease of use.
    ///
    /// This implementation will prompt user to enter 2-factor authentication codes in the console.
    /// </summary>
    public class UserLauncherAuthenticator : IAuthenticator
    {
        /// <inheritdoc />
        public Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
        {
            if (previousCodeWasIncorrect)
            {
                //Console.Error.WriteLine("The previous 2-factor auth code you have provided is incorrect.");
                DepotDownloaderClient.formatAndSend("GUARD-appauth-incorrect", "The previous 2-factor auth code you have provided is incorrect.", 0);
            }

            string? code;

            do
            {
                //Console.Error.Write("STEAM GUARD! Please enter your 2-factor auth code from your authenticator app: ");
                //code = Console.ReadLine()?.Trim();
                DepotDownloaderClient.formatAndSend("GUARD-appauth-ask", "STEAM GUARD! Please enter your 2-factor auth code from your authenticator app", 0);

                code = DepotDownloaderClient.recieveMessage();

                if (code != null)
                {
                    break;
                }
            }
            while (string.IsNullOrEmpty(code));

            return Task.FromResult(code!);
        }

        /// <inheritdoc />
        public Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect)
        {
            if (previousCodeWasIncorrect)
            {
                //Console.Error.WriteLine("The previous 2-factor auth code you have provided is incorrect.");
                DepotDownloaderClient.formatAndSend("GUARD-mailauth-incorrect", "The previous 2-factor auth code you have provided is incorrect.", 0);
            }

            string? code;

            do
            {
                //Console.Error.Write($"STEAM GUARD! Please enter the auth code sent to the email at {email}: ");
                DepotDownloaderClient.formatAndSend("GUARD-mailauth-ask", "STEAM GUARD! Please enter the auth code sent to the email", 0);
                //code = Console.ReadLine()?.Trim();

                code = DepotDownloaderClient.recieveMessage();
                DepotDownloaderClient.formatAndSend("LOG", "RECIEVED: " + code, 0);
                if (code != null)
                {
                    break;
                }
            }
            while (string.IsNullOrEmpty(code));

            return Task.FromResult(code!);
        }

        /// <inheritdoc />
        public Task<bool> AcceptDeviceConfirmationAsync()
        {
            //Console.Error.WriteLine("STEAM GUARD! Use the Steam Mobile App to confirm your sign in...");
            DepotDownloaderClient.formatAndSend("GUARD-mobileConfirm", "Use the Steam Mobile App to confirm your sign in...", 0);

            return Task.FromResult(true);
        }
    }
}
