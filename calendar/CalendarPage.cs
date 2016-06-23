using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace calendar
{
    public class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            CreateUI(DateTime.Now.Month, DateTime.Now.Year);
        }

        void CreateUI(int month, int year)
        {
            var dtn = new DateTime(year, month, DateTime.Now.Day);

            var months = new List<string>
            {
                "January", "February", "March", "April", "May",
                "June","July", "August", "September", "October", "November", "December"
            };

            var lblDate = new Label
            {
                Text = string.Format("{0} {1}", months[month - 1], year.ToString()),
                BackgroundColor = Color.Blue,
                WidthRequest = App.ScreenSize.Width * .8,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var currentDay = dtn.Day;

            var btnBack = new Button
            {
                Text = "<",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                WidthRequest = App.ScreenSize.Width * .1,
            };

            var btnNext = new Button
            {
                Text = ">",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                WidthRequest = App.ScreenSize.Width * .1
            };


            var stackTop = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { btnBack, lblDate, btnNext }
            };

            var width = (App.ScreenSize.Width * .8) / 7;

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = App.ScreenSize.Height * .1 },
                    new RowDefinition { Height = App.ScreenSize.Height * .1 },
                    new RowDefinition { Height = App.ScreenSize.Height * .1 },
                    new RowDefinition { Height = App.ScreenSize.Height * .1 },
                    new RowDefinition { Height = App.ScreenSize.Height * .1 },
                    new RowDefinition { Height = App.ScreenSize.Height * .1 }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                }
            };

            var dayLabels = CreateDayLabels();
            var dateLabels = CreateDateLabels();

            int left = 0, top = 0;
            foreach (var dl in dayLabels)
                grid.Children.Add(dl, left++, top);

            left = (int)dtn.DayOfWeek - 1; // sunday is day 0
            top++;

            if (left < 0)
                left = 0;

            foreach (var dl in dateLabels)
            {
                var tgr = new TapGestureRecognizer();
                tgr.Tapped += LabelTapped;
                dl.GestureRecognizers.Add(tgr);
                grid.Children.Add(dl, left, top);

                left++;
                if (left == 7)
                {
                    left = 0;
                    top++;
                }
            }

            var cont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = App.ScreenSize.Width * .95,
                Children =
                {
                    stackTop,
                    grid
                }
            };

            Content = cont;

            btnBack.Clicked += delegate
            {
                month--;
                if (month == 0)
                {
                    month = 12;
                    year -= 1;
                }

                CreateUI(month, year);
            };

            btnNext.Clicked += delegate
            {
                month++;
                if (month == 13)
                {
                    month = 1;
                    year += 1;
                }

                CreateUI(month, year);
            };
        }

        void LabelTapped(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            var ending = lbl.StyleId == "1" ? "st" : lbl.StyleId == "2" ? "nd" : lbl.StyleId == "3" ? "rd" : "th";
            DisplayAlert("Date clicked", string.Format("You have clicked the {0}{1} of the month", lbl.StyleId, ending), "OK");
        }

        ObservableCollection<Label> CreateDayLabels()
        {
            return new ObservableCollection<Label>
            {
                new Label {Text = "Sun",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Mon",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Tue",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Wed",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Thu",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Fri",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = "Sat",HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
            };
        }

        ObservableCollection<Label> CreateDateLabels()
        {
            var labelList = new ObservableCollection<Label>();
            for (int n = 0; n < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); ++n)
            {
                labelList.Add
                (
                    new Label
                    {
                        Text = (n + 1).ToString(),
                        StyleId = (n + 1).ToString(),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                );
            }
            return labelList;
        }
    }
}
