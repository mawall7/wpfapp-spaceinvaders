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
    public partial class MainWindow: Window
    {
        public bool invadertoggle = false;
        public int ShipX = 10;
        public int InvX = 0;
        public int InvY = 0;
        public int LaserX;
        public int LaserY = 19;
        public bool Fire = false;
        public bool SiHit = false;
        public Image Toerase { get; set; }
        public object Sitoerase { get; set; }
        public int test;
     
        public int Points { get; set; } = 0;
        public Timer T { get; set; }
        public SpaceInvaders SpI { get; set; }
        public List<Image> Images { get; set; }
      


        

        public MainWindow()
        {
             
            
                //bool update = true;
                //int c = 0;
                InitializeComponent();
                UpdateInvadersGrid();
                UpdateFireTasks();
                Update();
               
            // UpCheckCollision();

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();


        }

        public void Laser()
        {
                if(Fire ==  false || LaserY == 0)/*if(LaserY == 0)*/
                {
                    LaserY = 19;
                    Fire = false;
                    Grid.SetRow(myLaser, LaserY); //to do flyttas kanske till collision
                    Grid.SetColumn(myLaser, LaserX);
                    myLaser.Visibility = Visibility.Hidden;

            } 
            
                if (Fire)
                {
                    Grid.SetRow(myLaser, LaserY--);
                    Grid.SetColumn(myLaser, LaserX);
                    
                }
                 

                //myLaser.Visibility = Visibility.Hidden;
        }

        private async void UpdateFireTasks()
        {
            while (true)
            {
                await Task.Delay(50);
                Laser();
                CheckCollision(); //to do break vid träff kör nu flera gånger för varje förflyttning av sis
            }
        }

        private async void Update()
        {
            while (true)
            {
                invadertoggle = !invadertoggle;
                await Task.Delay(800);
                UpdateInvaders(invadertoggle);
               
              

                //SpI.DrawInvaders();
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

            if(e.Key == Key.Space && Fire == false)
            {
                Fire = true;
                myLaser.Visibility = Visibility.Visible;
                LaserX = ShipX;

            }
            
            Grid.SetColumn(myImage, ShipX);
            
        }

        public void UpdateInvaders(bool toggle) //toggle is for image
        {
            string path;
            int count = 1;
            string name;
            object sitoerase = null;

          
            SpI.UpdateInvaders(); //uppdaterar x och y horisontellt och radbyte
            
            
                //Image testi = (Image)MyGrid.FindName("si2");
                //if (testi != null)
                //{
                //    testi.Source = null;
                //}
                //if (testi!= null) {Images.Remove(testi);
                //MyGrid.Children.Remove(testi);} //tar bort
            
            
            //ändrar grid värden(row, column) till x y ovan för imagescontrolls  
            if (Toerase != null)
            {
               
                //RemoveLogicalChild(Toerase);
                //Images.Remove(Toerase); // > behövs inte möjligen för att det inte går att ta bort då elementen är i logical tree behöver isf vara i visual tree ? MyGrid.Children.Remove(Toerase);

            }

            foreach (var item in SpI.List)
                {
                    
                    name = "si" + count; //to do fungerar logiken ? när någon tagits bort? 

                // * TextBlock testfind = (TextBlock)MyGrid.FindName("textblock");
                // * Image foundimage = (Image)MyGrid.FindName(name); //returnerar imagecontrols

                //Image foundimage = Images.Where(i => i.Name == name).FirstOrDefault(); 
                //**?nedan
                //if (Toerase != null) {
                //    Toerase = null;
                //    UIElementCollection col = (UIElementCollection)MyGrid.Children;
                //    myPoints.Text = col.Count.ToString();
                //    col.Remove(Toerase);
                   
                //}
                Image foundimage = Images[SpI.List.IndexOf(item)];
                    //if (foundimage.Source != null)
                    //{
                        Grid.SetRow(foundimage, item.PosY);
                        Grid.SetColumn(foundimage, item.PosX);
                    //}
                    path = toggle == true ? "/wpfapp1;component/Images/si1_2.png" : "/wpfapp1;component/Images/si1_1.png";
                    BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                    foundimage.Source = image;
                    //*done Collision behöver flyttas ut! och skapa en till update metod. annars kollas bara collision då de flyttas!
                
                count++;

                }
            

            //Grid.SetRow(si1, InvX++)
        }

        public void CheckCollision()
        {
               
                Toerase = null;
                int count = 0;
                foreach (var item in SpI.List)
                {
                    count++;

                    if (LaserY == item.PosY && LaserX == item.PosX && Toerase == null ) //Nextmove ska bara köra igen om spi rört sig
                    {
                       Toerase = Images[count - 1];
                       //Toerase = Images[count - 1].Visibility == Visibility.Visible ? Images[count - 1] : null;
                        if (Toerase != null)
                        {
                        //Toerase.Visibility = Visibility.Hidden; // si hit control
                                                                // myLaser.Visibility = Visibility.Hidden; // laser control
                            Fire = false;
                            Points++;
                            myPoints.Text = "myPoints | " + Points; 
                            //LaserY = 19;
                            
                            //Sitoerase = item;
                     
                          
                           // to do funkar ej bilderna byter position varför? break löser det men fortfarande går inte inv till kanten
                           SpI.List.Remove((GameObject)item); //tar bort object i invaders klassen
                           Toerase.Visibility = Visibility.Hidden; 
                           Images.Remove(Toerase);//to do remove element image tar bort elementet men ritas ändå ut där den blir skjuten löst det temporärt med Visibliity hidden. tas bort från listan och Image collection i Update. 
                           
                           break;   
                       
                        //Toerase = null;
                      
                        //Toerase = null;
                        //*break;
                        
                       
                        //MyGrid.Children.Remove(toerase); // to do funkar ej bilderna byter position varför? break löser det men fortfarande går inte inv till kanten
                        //SpI.List.Remove((GameObject)sitoerase);

                        }
                    
                    //to do 

                }
            }

            //if (sitoerase != null)
            //{
            //    SpI.List.Remove((GameObject)sitoerase);
            //}
        }
        
        public void UpdateInvadersGrid() //obs bilder högerklicka > välj properties build action > ändra None > resources 
        {
            //skapa rows and columns 
            string path = "/wpfapp1;component/Images/si1_2.png";
            SpI = new SpaceInvaders(22, SpaceInvaders.Typeui.WpfApp);
            SpI.InitEnemies();
            int count = 0;
            int row = 0;
            Images = new List<Image>();
            foreach (var item in SpI.List.ToArray())
            {
                count++;
                var contrtag = "si" + count; //Name prop
                var ImageControl = new Image(); ImageControl.Name = contrtag;
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                ImageControl.Source = image;
                ImageControl.Width = 30; ImageControl.Height = 30;
                Images.Add(ImageControl); // skriv in i SpaceInvaders class istället?
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
