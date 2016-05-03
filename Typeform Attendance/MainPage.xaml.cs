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
using System.Collections;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Typeform_Attendance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool hasPreviousDate = false;
        string typeformAPIKey = "e3f571d06b5607e7fb7de79ad72472f3b973cd56";
        string typeformUID = "tuEGEV";

        public MainPage()
        {
            this.InitializeComponent();
            new MyToolkit.Controls.DataGrid();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://api.typeform.com/v1/form/" + typeformUID + "?key=" + typeformAPIKey + "&completed=true";
            System.Diagnostics.Debug.WriteLine(url);
            Main.getRootobject(url);
            DateBox.Date = DateTime.Now;
            TimeBox.Time = DateTime.Now.TimeOfDay;
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
                DataGrid.ItemsSource = personList;
            }
        }

        private void OnSelectedItemsChanged(EventArgs e)
        {

        }

        private void textBoxAPI_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxAPI.Text = String.Empty;
        }

        private void textBoxUID_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxUID.Text = String.Empty;
        }
    }
}
