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
        public int ShipY = 20;
        public int LaserY = 19;
        public bool Fire = false;
        public bool SiHit = false;
        public bool InvFire = true;
        int InvFireX { get; set; }
        int InvFireY { get; set; }
        
        public Image Toerase { get; set; } = null;
        public object Sitoerase { get; set; }
        public int test;
     
        public int Points { get; set; } = 0;
        public Timer T { get; set; }
        public SpaceInvaders SpI { get; set; }
        public List<Image> Images { get; set; }
      
        private Image Invlaser { get; set; }

        

        public MainWindow()
        {
             
            
                //bool update = true;
                //int c = 0;
                InitializeComponent();
                UpdateInvadersGrid();
                UpdateShipFireTasks(); 
                UpdateInvadersFireTasks();
                Update();
                

        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)

        {

     
            if (e.Key == Key.Left && ShipX > 0)
            {
                ShipX--;
                Grid.SetColumn(myShip, ShipX);


            }
            if (e.Key == Key.Right && ShipX <= 200)
            {
                ShipX++;
                Grid.SetColumn(myShip, ShipX);
            }

            if (e.Key == Key.Space && Fire == false)
            {
                Fire = true;
                myLaser.Visibility = Visibility.Visible;
                LaserX = ShipX;
                

            }

            

        }
        private void RemoveShipLaser()
        {
            MyGrid.Children.Remove(myLaser);
            myLaser.Visibility = Visibility.Hidden;
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

        private async void UpdateShipFireTasks()
        {
           
            while (SpI.List.Count > 0)
            {
                await Task.Delay(50);
                Laser();
                CheckCollision(); // to do gör som delegat lägg till metoder collision för både skepp och invs
            }
            
        }
        private async void UpdateInvadersFireTasks()
        {
            
            while (SpI.List.Count > 0) 
            {
                await Task.Delay(100);
                InvLaser();
                CheckCollision();
            }
           
        }

        private void RemoveInvLaser()
        {
            if (Invlaser != null)
            {
                MyGrid.Children.Remove(Invlaser);
                Invlaser.Visibility = Visibility.Hidden;
            }
        }

        public void InvLaser() //to do skapa handler för att skapa ny Image
        {
         
            if (InvFire && SpI.List.Count > 0)

            {
                InvFire = false;
                Invlaser = new Image(); Invlaser.Name = "il";
                string path = "/wpfapp1;component/Images/laser.png";
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                Invlaser.Source = image;
                Invlaser.Width = 30; Invlaser.Height = 30;

                MyGrid.Children.Add(Invlaser);
               
                Random random = new Random(); int randomsi = random.Next(0, SpI.List.Count);
                InvFireY = SpI.List[randomsi].PosY + 1;
                InvFireX = SpI.List[randomsi].PosX;

                Grid.SetColumn(Invlaser, SpI.List[randomsi].PosX);
                
            }
          

            if (InvFireY <= ShipY)
            {
                Grid.SetRow(Invlaser, InvFireY++);
            }
            if(InvFireY > ShipY)
            {
                    
                MyGrid.Children.Remove(Invlaser);
                InvFire = true;
            }
           
                    
        }
        private async void Update()
        {
            while (SpI.List.Count!=0)
            {
                invadertoggle = !invadertoggle;
                await Task.Delay(800);
                UpdateInvaders(invadertoggle);


                if (SpI.List.Count == 0) 
                {
                    RemoveInvLaser();
                    RemoveShipLaser();
                    myPoints.Text = "Success!";
                }
                //textblock.Text = DateTime.Now.ToLongTimeString();

            }
        }
      
     
        

        public void UpdateInvaders(bool toggle) //toggle is for image
        {
            string path;
            int count = 1;
            string name;
            object sitoerase = null;

            
            SpI.UpdateInvaders(); //uppdaterar x och y horisontellt och radbyte
         
            foreach (var item in SpI.List)
                {
                    
                    name = "si" + count;  

                    Image foundimage = Images[SpI.List.IndexOf(item)];
                  
                    Grid.SetRow(foundimage, item.PosY);
                    Grid.SetColumn(foundimage, item.PosX);
                  
                    path = toggle == true ? "/wpfapp1;component/Images/si1_2.png" : "/wpfapp1;component/Images/si1_1.png";
                    BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                    foundimage.Source = image;
                                 
                    count++;

                }
                   
        }

        public void CheckCollision()
        {
               
                Toerase = null;
                int count = 0;
                foreach (var item in SpI.List)
                {
                    count++;

                    if (LaserY == item.PosY && LaserX == item.PosX )//&& Toerase == null) //Nextmove ska bara köra igen om spi rört sig
                    {
                       Toerase = Images[count - 1];

                        if (Toerase != null)
                        {
                                                          
                            Fire = false;
                            Points++;
                         
                           SpI.List.Remove((GameObject)item); //tar bort object i invaders klassListan
                           Toerase.Visibility = Visibility.Hidden; 
                           Images.Remove(Toerase);//to do remove element image tar bort elementet men ritas ändå ut där den blir skjuten löst det temporärt med Visibliity hidden. tas bort från listan och Image collection i Update. 
                           myPoints.Text = Points.ToString(); //to do points visar bara 4 bokstäver fick ha 2 textblocks ist.
                           break;   
                        }
                    
                }

            }

                if(InvFireY == ShipY && InvFireX == ShipX)
                {
                    RemoveShip();
                }



        }

        private void RemoveShip()
        {
            MyGrid.Children.Remove(myShip);
        }

        public void UpdateInvadersGrid() //obs bilder högerklicka > välj properties build action > ändra None > resources 
        {
            //skapa rows and columns 
            string path = "/wpfapp1;component/Images/si1_2.png";
            SpI = new SpaceInvaders(22, SpaceInvaders.Typeui.WpfApp);
            SpI.InitEnemies();
            int count = 0;
            int row = 0;
            TextBlock myTextBlock = new TextBlock();
            myTextBlock.Name = "myPointPlayer";
            Grid.SetColumn(myTextBlock, 2); 
            Grid.SetRow(myTextBlock, 2);
            myTextBlock.FontSize = 14;
            //Color ? måste sättas
            
            
            myTextBlock.Text = "jfksjajkfdska";
            Images = new List<Image>();
            foreach (var item in SpI.List.ToArray())
            {
                count++;
                var contrtag = "si" + count; //Name prop
                var ImageControl = new Image(); ImageControl.Name = contrtag;
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                ImageControl.Source = image;
                ImageControl.Width = 30; ImageControl.Height = 30;
                Images.Add(ImageControl); // to do skriv in i SpaceInvaders class istället?
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
