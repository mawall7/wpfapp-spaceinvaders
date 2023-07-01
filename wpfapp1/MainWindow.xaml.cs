using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using spaceinvaders;


namespace wpfapp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool invadertoggle = false;
        public int ShipX = 1;
        public int InvX = 0;
        public int InvY = 0;
        public Timer T { get; set; }

        

        public MainWindow()
        {
             
            
                //bool update = true;
                //int c = 0;
                InitializeComponent();
                UpdateInvadersGrid();
                //Update();
            
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();
        }

        private async void Update()
        {
            while (true)
            {
                invadertoggle = !invadertoggle;
                await Task.Delay(500);
               // UpdateInvaders(invadertoggle);
                //textblock.Text = DateTime.Now.ToLongTimeString();

            }
        }
        //void timer_Tick(object sender, EventArgs e)
        //{
        //    //UpdateInvaders();
        //    //textblock.Text = DateTime.Now.ToLongTimeString();
        //}
     
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)

        {
            
            //using (FileStream fs = File.Open("C:\\Users\\matte\\source\\repos\\wpfapp1\\wpfapp1\\Images\\si1_2.png", FileMode.Open))
            //{
            //    BitmapImage bitmap = new BitmapImage(); bitmap.StreamSource = fs;
            //    si1.Source = bitmap; //to do konvertera png till Bitmap
            //    this.si1.Source = bitmap;      
            //                }

            if ( e.Key == Key.Left)
            {
                ShipX--;

             
            }
            if (e.Key == Key.Right)
            {
                ShipX++;
            }
            
            Grid.SetColumn(myImage, ShipX);
            
        }

        //public void UpdateInvaders(bool toggle)
        //{
        //    string path;
        //    path = toggle == true ? "/wpfapp1;component/Images/si1_2.png" : "/wpfapp1;component/Images/si1_1.png";
        //    BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
        //    si1.Source = image;
        //    if (Grid.GetColumn(si1) == 2) /*&& Grid.GetRow(si1) == Even*/
        //    {
        //        InvY++;
        //        Grid.SetRow(si1, InvY); Grid.SetColumn(si1, 0);
        //        InvX = 0;
        //    }
        //    else
        //    {
        //        InvX++;
        //        Grid.SetColumn(si1, InvX);
        //    }
        //        //Grid.SetRow(si1, InvX++)
        //}

        public void UpdateInvadersGrid() //obs bilder högerklicka > välj properties build action > ändra None > resources 
        {
            //skapa rows and columns 
            string path = "/wpfapp1;component/Images/si1_2.png";
            //string path = "C:/Users/matte/source/repos/wpfapp1/wpfapp1/Images/si1_2.png";
            spaceinvaders.SpaceInvaders sp = new SpaceInvaders(SpaceInvaders.Typeui.WpfApp);
            sp.InitEnemies();
            int count = 0;
            int row = 0;
            
            foreach (var item in sp.List.ToArray())
            {
                count++;
                var contrtag = "si" + count;
                var ImageControl = new Image(); ImageControl.Name = contrtag;
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                ImageControl.Source = image;
                ImageControl.Width = 30; ImageControl.Height = 30;
                //row = count < 10 ? row : ; 
                Grid.SetRow(ImageControl, item.PosY);
                Grid.SetColumn(ImageControl, item.PosX);
                MyGrid.RowDefinitions.Add(new RowDefinition());
                MyGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MyGrid.Children.Add(ImageControl);
                
            }


            
            

            

        }
    }

}
