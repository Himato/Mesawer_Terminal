using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Mesawer
{
    internal class UpdateChecker
    {
        private static string _updateMessage = "";
        private static bool _updateStatus = true;

        public static void CheckingSimulation()
        {
            const string error = "An Error has happened, try again later.";
            _updateMessage = error;
            Console.Write("Checking for Updates ");

            CheckForUpdate(false);

            Console.Write(".");
            Thread.Sleep(1000);

            if (_updateMessage.Equals(error))
            {
                Thread.Sleep(2000);
                Console.Write(".");
            }
            else
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }

            if (_updateMessage.Equals(error))
            {
                Thread.Sleep(6000);
                Console.Write(".\n");
            }
            else
            {
                Thread.Sleep(1000);
                Console.Write(".\n");
            }

            Console.WriteLine(_updateMessage);

            if (!_updateStatus)
            {
                Process.Start(Shared.DownloadUrl);
            }
        }

        private static async void CheckForUpdate(bool isMain)
        {
            if (CheckForInternetConnection())
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        using (var response = await client.GetAsync(Shared.WebApi))
                        {
                            using (var content = response.Content)
                            {
                                var jsonResponse = await content.ReadAsStringAsync();
                                var root = JObject.Parse(jsonResponse);
                                var value = root["Version"].ToString();

                                if (!value.Equals(Shared.Version))
                                {
                                    _updateMessage = $"Update Status: old-version\nVersion: {value} is available now," +
                                                     $" you download it from our site {Shared.DownloadUrl}";
                                    if (isMain)
                                    {
                                        Console.WriteLine(_updateMessage);
                                        //MessageBox.Show(_updateMessage, "Update Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Process.Start(Shared.DownloadUrl);
                                    }
                                    _updateStatus = false;
                                }
                                else
                                {
                                    _updateMessage = "Update Status: You have the latest version";
                                    _updateStatus = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _updateMessage = e.Message;
                    _updateStatus = true;
                }
            }
            else
            {
                _updateMessage = "There's no internet connection";
                _updateStatus = true;
            }
        }

        private static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
