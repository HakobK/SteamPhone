using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SteamPhone.Resources;
using SteamSharp;
using SteamSharp.Authenticators;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SteamPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        string SteamguardID = "";
        string SteamguardSolution = "";
        string CaptchaID = "";
        int loginPhase = 0;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SteamClient client = new SteamClient();
            lblCaptcha.Visibility = Visibility.Collapsed;
            lblSteamguard.Visibility = Visibility.Collapsed;
            txtCaptcha.Visibility = Visibility.Collapsed;
            txtSteamguard.Visibility = Visibility.Collapsed;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getAuth(txtUsername.Text,txtPassword.Password);
        }

        public async Task getAuth(string user, string password)
        {
            SteamClient client = new SteamClient();
            UserAuthenticator.CaptchaAnswer captchaAns = new UserAuthenticator.CaptchaAnswer();

            if (!string.IsNullOrWhiteSpace(txtSteamguard.Text))
            {
                SteamguardSolution = txtSteamguard.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCaptcha.Text))
            {
                captchaAns.SolutionText = txtCaptcha.Text;
            }

            //Wanneer er geen steamguard en captcha wordt meegegeven.
            if (loginPhase == 0)
            {
                try
                {
                    UserAuthenticator.SteamAccessRequestResult authId = await Task.Run(() => UserAuthenticator.GetAccessTokenForUserAsync(user, password));

                    if (authId.IsSteamGuardNeeded)
                    {
                        SteamguardID = authId.SteamGuardID;
                        MessageBox.Show("Please fill in the steam code you received by mail.");
                        txtSteamguard.Visibility = Visibility.Visible;
                        lblSteamguard.Visibility = Visibility.Visible;
                        loginPhase = 1;
                    }

                    if (authId.IsCaptchaNeeded)
                    {
                        CaptchaID = authId.CaptchaGID;
                        MessageBox.Show("Please fill in the captcha code");
                        txtCaptcha.Visibility = Visibility.Visible;
                        lblCaptcha.Visibility = Visibility.Visible;
                        new BitmapImage(new Uri(authId.CaptchaURL.ToString()));
                        loginPhase = 2;
                    }
                    MessageBox.Show(authId.User.AuthCookie.ToString());
                }
                catch(SteamAuthenticationException)
                {
                    MessageBox.Show("Incorrect login information");
                }
            }

            //Wanneer er een steamguard wordt meegegeven
            if(loginPhase == 1)
            {
                UserAuthenticator.SteamGuardAnswer steamguardAns = new UserAuthenticator.SteamGuardAnswer();
                steamguardAns.ID = SteamguardID;
                steamguardAns.SolutionText = SteamguardSolution;
                UserAuthenticator.SteamAccessRequestResult authId = await Task.Run(() => UserAuthenticator.GetAccessTokenForUserAsync(user, password,steamguardAns));

                if (authId.IsSteamGuardNeeded)
                {
                    SteamguardID = authId.SteamGuardID;
                    MessageBox.Show("Please fill in the steam code you received by mail.");
                    txtSteamguard.Visibility = Visibility.Visible;
                    lblSteamguard.Visibility = Visibility.Visible;
                }

                if (authId.IsCaptchaNeeded)
                {
                    CaptchaID = authId.CaptchaGID;
                    MessageBox.Show("Please fill in the captcha code");
                    txtCaptcha.Visibility = Visibility.Visible;
                    lblCaptcha.Visibility = Visibility.Visible;
                    new BitmapImage(new Uri(authId.CaptchaURL.ToString()));
                }

            MessageBox.Show(authId.User.AuthCookie.ToString());
           //UserAuthenticator s =  UserAuthenticator.ForProtectedResource(authId.User);
           //client.Authenticator = s;

           //ISteamRequest request;
           //request.SteamApiMethod(SteamNews.GetNewsForAppAsync(client,4,4,4));

           
            //MessageBox.Show(player.PersonaName.ToString());
            }


        }

        public async Task<SteamUserInterface.Player> playerInfo(SteamUserInterface.Player player, SteamClient client)
        { 
            player = await SteamUserInterface.GetPlayerSummaryAsync(client, "76561197960401227");
            return player;

        }

        public async Task getInfo()
        {
            UserAuthenticator.SteamGuardAnswer sAnswer = new UserAuthenticator.SteamGuardAnswer();
            UserAuthenticator.CaptchaAnswer cAnswer = new UserAuthenticator.CaptchaAnswer();

            if (!string.IsNullOrWhiteSpace(txtSteamguard.Text))
            {
                sAnswer.SolutionText = txtSteamguard.Text;
            }
            if (!string.IsNullOrWhiteSpace(txtCaptcha.Text))
            {
                cAnswer.SolutionText = txtCaptcha.Text;
            }


            string captcha = txtCaptcha.Text;
            MessageBox.Show("fasiejfosfjois");


                string user = txtUsername.Text;
                string password = txtPassword.Password;
                SteamUser swag = new SteamUser();

                UserAuthenticator.SteamAccessRequestResult result = await Task.Run(() => UserAuthenticator.GetAccessTokenForUserAsync(user, password));
                SteamguardID = result.SteamGuardID;

                if (result.IsCaptchaNeeded == true)
                {
                    imgCaptcha.Source = new BitmapImage(new Uri(result.CaptchaURL.ToString()));
                     MessageBox.Show("You received mail with captcha id");
                }

                if (result.IsSteamGuardNeeded == true)
                {
                    MessageBox.Show("You received mail with steamguard code");
                }

                
            
            //            MessageBox.Show("fasiejfosfjois");

            //     if (string.IsNullOrEmpty(txtCaptcha.Text) && !string.IsNullOrEmpty(txtSteamguard.Text))
            //{
            //    SteamClient client = new SteamClient();
            //    string user = txtUsername.Text;
            //    string password = txtPassword.Password;
            //    SteamUser swag = new SteamUser();


            //    UserAuthenticator.SteamGuardAnswer realAnswer = new UserAuthenticator.SteamGuardAnswer();
            //    realAnswer.SolutionText = txtSteamguard.Text;
            //    realAnswer.ID = SteamguardID;


            //    UserAuthenticator.SteamAccessRequestResult result = await Task.Run(() => UserAuthenticator.GetAccessTokenForUserAsync(user, password,realAnswer));

                

            //    if (result.IsCaptchaNeeded == true)
            //    {
            //        imgCaptcha.Source = new BitmapImage(new Uri(result.CaptchaURL.ToString()));
            //          MessageBox.Show("Check mail");


            //    }

            //    if (result.IsSteamGuardNeeded == true)
            //    {
            //        MessageBox.Show("Check mail");
            //    }

            //    SteamUser newUser = new SteamUser();
            //    newUser.SteamID = result.User.SteamID;
            //    newUser.AuthCookie = result.User.AuthCookie;
            //    newUser.TransferToken = result.User.TransferToken;

                
            //    //SteamID newId = new SteamID(result.User.SteamID);

            //    UserAuthenticator s =  UserAuthenticator.ForProtectedResource(newUser.AuthCookie);
            //   // SteamSharp.steam
            //    //a/]][s.Authenticate(client,)
                


            //    if(result.IsLoginComplete)
            //    {
            //        MessageBox.Show("Logged in as: " + newUser.SteamID + " auth token is :" + newUser.AuthCookie.Value.ToString());
            //    }

            //     }


        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
