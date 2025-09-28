namespace Tic_Tac_Toe
{
    public partial class MainPage : ContentPage
    {
        public List<ContentPage> lehed = new List<ContentPage>() { new MangPage(), new SeadedPage(), new StatistikaPage() };
        public List<string> tekstid = new List<string>() { "Mängi / Играть", "Seaded / Настройки", "Statistika / Статистика" };

        VerticalStackLayout vsl;
        ScrollView sv;
        Label title;
        Image tic;

        public MainPage()
        {
            Title = "Avaleht";
            BackgroundImageSource = "taust.jpg"; vsl = new VerticalStackLayout();
            vsl = new VerticalStackLayout();

            title = new Label
            {
                Text = "Trips Traps Trull",
                FontFamily = "MrBedfort-Regular",
                FontSize = 34,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Colors.Black,
                Margin = new Thickness(0, 10, 0, 10),
                FontAttributes = FontAttributes.Bold
            };
            vsl.Add(title);

            tic = new Image
            {
                Source = "tictactoe.png",
                Aspect = Aspect.AspectFit,
                HeightRequest = 400,
                Margin = new Thickness(0, 10)
            };
            vsl.Add(tic);

            for (int i = 0; i < lehed.Count; i++)
            {
                Button nupp = new Button
                {
                    Text = tekstid[i],
                    FontSize = 15,
                    BackgroundColor = Color.FromRgba(100,100,100,0),
                    TextColor = Colors.Black,
                    CornerRadius = 5,
                    BorderColor = Colors.Black,
                    BorderWidth = 2,
                    Margin = 10,
                    FontFamily = "Kanit-MediumItalic",
                    ZIndex = i
                };
                vsl.Add(nupp);
                nupp.Clicked += Nupp_Clicked;
            }
            sv = new ScrollView { Content = vsl };
            Content = sv;
        }

        private async void Nupp_Clicked(object? sender, EventArgs e)
        {
            Button nupp = (Button)sender;
            await Navigation.PushAsync(lehed[nupp.ZIndex]);
        }
    }
}
