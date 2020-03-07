using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USLocations;

using Xamarin.Forms;

namespace Homework2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        USLocations.USLocations location;
        public MainPage()
        {
            InitializeComponent();
            options.SelectedIndex = 0;
            location = new USLocations.USLocations();
            location.Helper("zipcodes.tsv");
            
        }

       
        private void Button_Clicked(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
          
                
                if (options.SelectedItem.ToString() == "Amount")
                {
                try
                {
                    if (textAmount.Text == null)
                    {
                        list.Add("Please enter a number for the amount");
                        view.ItemsSource = list;
                    }
                    else
                    {
                        int result = Int32.Parse(textAmount.Text.ToString());
                        List<string> valueList = new List<string>();
                     
                            valueList.AddRange(location.TaxNearInput(result));
                        if (valueList.Count == 0)
                        {
                            valueList.Add("No items for this selection");
                        }
                        view.ItemsSource = valueList;
                    }
                }
                catch (FormatException)
                {
                    list.Add("Please enter a number for the amount");
                    view.ItemsSource = list;
                }
            } else
                {
                    List<string> valueList = new List<string>();
                    valueList.AddRange(location.NearLocation(City.Text,State.Text));
                if (valueList.Count == 0)
                {
                    valueList.Add("No items for this selection");
                 }
                view.ItemsSource = valueList;
                }
               
          
            
            //label.Text = options.SelectedItem.ToString(); can be used to get selected item
        }
    }
}
