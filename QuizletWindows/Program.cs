using DevExpress.Skins;
using DevExpress.UserSkins;
using QuizletWindows.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;


namespace QuizletWindows
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetUp();
            Application.Run(new FrmMainMenu());
        }
        static void SetUp()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://apigateway");

            UserApi.Instance.SetHttpClient(httpClient);
        }
    }
}
