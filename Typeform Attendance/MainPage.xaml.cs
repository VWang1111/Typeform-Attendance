using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using MyToolkit.Controls;
using Windows.ApplicationModel.DataTransfer;
using System.Collections;
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Typeform_Attendance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool hasPreviousDate = false;
        string typeformAPIKey;
        string typeformUID;

        public MainPage()
        {
            this.InitializeComponent();
            this.DateBox.Date = new DateTime(0);
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            typeformAPIKey = (String)localSettings.Values["typeformAPIKey"];
            if (typeformAPIKey != null)
                textBoxAPI.Text = typeformAPIKey;

            typeformUID = (String)localSettings.Values["typeformUID"];
            if (typeformUID != null)
                textBoxUID.Text = typeformUID;

            String emailPerPage = (String)localSettings.Values["EmailPerPage"];
            if (emailPerPage != null)
            {
                Main.EmailPerPage = Int32.Parse(emailPerPage);
                textBoxEmailPage.Text = Main.EmailPerPage.ToString();
            }
            else
                Main.EmailPerPage = 50;
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if(typeformUID == null || typeformAPIKey == null)
            {
                var dialog = new MessageDialog("UID or API Key Missing");
                await dialog.ShowAsync();
                return;
            }

            string url = "https://api.typeform.com/v1/form/" + typeformUID + "?key=" + typeformAPIKey + "&completed=true";
            System.Diagnostics.Debug.WriteLine(url);
            Main.getRootobject(url);
            DateBox.Date = DateTime.Now;
            TimeBox.Time = DateTime.Now.TimeOfDay;
            averageAttending.Text = Main.getAverageAttendance().ToString();
        }

        private void HeaderBar_PivotItemLoaded(Windows.UI.Xaml.Controls.Pivot sender, PivotItemEventArgs args)
        {
            if(HeaderBar.SelectedIndex == 1)
            {
                List<Person> personList = Main.getAttendance().Cast<Person>().ToList();
                foreach (Person p in personList)
                {
                    System.Diagnostics.Debug.WriteLine(p);
                }
                DataGrid.ItemsSource = new List<Person>();
                DataGrid.ItemsSource = personList;
            }

            if(HeaderBar.SelectedIndex == 2)
            {
                PageTextBlock.Text = "1";
                MailDataGrid.ItemsSource = new List<Person>();
                MailDataGrid.ItemsSource = Main.getEmails(1);
            }
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if(Int32.Parse(PageTextBlock.Text) > 1)
            {
                PageTextBlock.Text = (Int32.Parse(PageTextBlock.Text)-1).ToString();
                MailDataGrid.ItemsSource = new List<Person>();
                MailDataGrid.ItemsSource = Main.getEmails(Int32.Parse(PageTextBlock.Text));
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            List<Person> emailList = Main.getEmails(Int32.Parse(PageTextBlock.Text) + 1);
            if(emailList.Count != 0)
            {
                PageTextBlock.Text = (Int32.Parse(PageTextBlock.Text) + 1).ToString();
                MailDataGrid.ItemsSource = new List<Person>();
                MailDataGrid.ItemsSource = emailList;
            }
        }

        private void ClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            List<Person> emailList = Main.getEmails(Int32.Parse(PageTextBlock.Text));
            List<String> pureEmailList = new List<String>();
            foreach(Person p in emailList)
            {
                pureEmailList.Add(p.Email);
            }
            String toCpy = "";

            foreach(String s in pureEmailList)
            {
                toCpy += s + ", ";
            }

            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(toCpy);
            Clipboard.SetContent(dataPackage);
        }

        private void textBoxEmailPage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Main.EmailPerPage = Int32.Parse(textBoxEmailPage.Text);
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["EmailPerPage"] = Main.EmailPerPage.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                textBoxEmailPage.Text = "50";
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["EmailPerPage"] = 50.ToString();
            }
        }

        private void textBoxUID_LostFocus(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["typeformUID"] = textBoxUID.Text;
            typeformUID = textBoxUID.Text;
        }

        private void textBoxAPI_LostFocus(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["typeformAPIKey"] = textBoxAPI.Text;
            typeformAPIKey = textBoxAPI.Text;
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("typeformAPIKey");
            localSettings.Values.Remove("typeformUID");
            localSettings.Values.Remove("EmailPerPage");

        }
    }
}
