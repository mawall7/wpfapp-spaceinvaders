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
using System.Threading;

namespace wpfapp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double Left { get; set; } = -5;
        public double Top { get; set; } = -20;
        public double Right { get; set; } = 0;
        public double Bottom { get; set; } = 0;
        public bool KeyLeft { get; set; } = false;
        public bool KeyRight { get; set; } = false;
        public bool ReadyForKey { get; set; } = true;
        public int Margincount { get; set; } = 0;
        public bool Toogle { get; set; } = false;
        public bool GameIsRunning = true;
        public bool invadertoggle = false;

        public int ShipX = 10;
        public int ShipY = 20;
        public bool Shiptoggle = false;
        public int count = 0;
        public bool isintro = true;
        public int introcount = 10;
        public int Hp { get; set; } = 3;
        public int InvX = 0;
        public int InvY = 0;
        public int LaserX;
        public int LaserY = 19;
        public bool Fire = false;
        public bool isHit { get; set; } = false;
        public bool InvFire = true;
        int InvFireX { get; set; }
        int InvFireY { get; set; }
        
        public Image Toerase { get; set; } = null;
        public object Sitoerase { get; set; }
        public int test;
     
        public int Points { get; set; } = 0;
        public Timer T { get; set; }
        public SpaceInvaders2 SpI { get; set; }
        public List<Image> Images { get; set; }
      
        private Image Invlaser { get; set; }
        public int ShipTopMargin { get; private set; } = 200;

        public MainWindow()
        {
               
                InitializeComponent();
                
                UpdateInvadersGrid();
                CreateInvLaser();
               
               
                if (GameIsRunning && SpI.List.Count > 0)  //to do InvLaser2 list när alla inv är ner skjutna
                {
                // InvLaser2();
                 
                    Update();
                    InvLaser2();
                    UpdateInvadersFireTasks(); //problem med collision
                    UpdateShipFireTasks();
                    ShipAnimation();
                    KeyAnim();
                    ShipHitUpdate(Shiptoggle);
                
                }
    
                
        }

        private async void KeyAnim() //todo gör knapptrycktning som task istället eller flera events så det går att skjuta samtidigt som man flyger höger vänster skeppet ej stannar
        {
            while (true)
            {
                await Task.Delay(1);
                {
                    if(!ReadyForKey)
                    {
                        Margincount++;
                        //gör nedan höger "smoothflying" som en animation istället vid ett knapptryck ska animationen köras innan knapptryck lagras kanske dessutom och handler som kör animationen först och sedan tar nästa lagrade knapptryck och hanterar det

                        myShip.Margin = new Thickness(Left, Top, Right, Bottom);
                        if (Margincount == 18)
                        {
                            Margincount = 0; Left = -10; Top = -20; Right = -10; Bottom = 0;
                            ReadyForKey = true; //animation slutar köras och knapptryck blir möjligt igen
                            myShip.Margin = new Thickness(Left, Top, Right, Bottom);
                            if (KeyRight) ShipX++;
                            if (KeyLeft) ShipX--;
                            KeyRight = false; KeyLeft = false;
                        }
                        else if (KeyRight){ Left = Left +2; Right = Right -2; }
                        else if (KeyLeft) { Left = Left -2; Right = Right + 2; }
                        Grid.SetColumn(myShip, ShipX);
                      
                    }
                }
            }
        }

        private void RemoveShip()
        {
            MyGrid.Children.Remove(myShip);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)

        {

     
            if (e.Key == Key.Left && ShipX > 0 && ReadyForKey)
            {
                ReadyForKey = false;
                KeyLeft = true; 
                //ShipX--;
                //Grid.SetColumn(myShip, ShipX);


            }
            if (e.Key == Key.Right && ShipX <= 200 && ReadyForKey) //to do gör knapptryck till olika tasks för att inte stanna skeppet vid fire
            {
                ReadyForKey = false;
                KeyRight = true;
                //ShipX++;
                //Grid.SetColumn(myShip, ShipX);
              // ta bort skapa handler ist. som lagrar knapptryck? eller blir det bra rörelseflöde ändå?
                
               
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
                if(Fire ==  false || LaserY == 0)//if(LaserY == 0)* ? behövs if Fire == false? kan sätta LaserY till 19 från start ist?
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

            //Invlaser below


            ////samma sak som 28 här blir det fel ? och man kan bli träffad än då ShipY är mindre än skeppet det har åkt förbi.
            ///*if(InvFireY == ShipY)*/   //to do? då blir det collision men det händer då även då skeppet är över skottet ser ut som att ShipY < InvFireY men det kanske bara ser ut så
            //if (InvFireY < 28)    //? fungerade igen efter nedanstående ändringar ShipY 
            //{
            //    Grid.SetRow(Invlaser, InvFireY++);
            //    Grid.SetColumn(Invlaser, InvFireX);
            //}

            //if ( (InvFireY == ShipY && InvFireX == ShipX ) || InvFireY >= 28) {

            //    if (InvFireY == ShipY && InvFireX == ShipX ) { isHit = true; } //invader collision detection
                
            //    Random random = new Random(); int randomsi = random.Next(0, SpI.List.Count);
            //    InvFireY = SpI.List[randomsi].PosY + 1;
            //    InvFireX = SpI.List[randomsi].PosX;

            //    Grid.SetColumn(Invlaser, InvFireX);
            //    Grid.SetRow(Invlaser, InvFireY);
            //    //Invlaser.Visibility = Visibility.Visible;
            //}

           



        }

        private async void UpdateShipFireTasks()
        {
            while (true)
            {
                await Task.Delay(50); //50
                if (SpI.List.Count == 0) 
                {

                    RemoveInvLaser(); //flytta till UpdateInvaderTasks
                    RemoveShipLaser();
                    GameOverTxt.Text = "Success!";
                }

                if (SpI.List.Count > 0)
                {
                    AfterBurner();
                    //CheckCollision(); // to do gör som delegat lägg till metoder collision för både skepp och invs
                    Laser();
                   
                }

               
            }
        }

        private void AfterBurner()
        {
            Toogle = !Toogle;
            string path = Toogle ? "/wpfapp1;component/Images/niceship2.png" : "/wpfapp1;component/Images/niceship1.png";
            myShip.BeginInit();
            BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
            myShip.EndInit();
            myShip.Source = image;
        }

        private async void UpdateInvadersFireTasks() //gjort! obs invfire uppdateras inte tillräckligt är felet här?
        {
            
            while (SpI.List.Count > 0) 
            {
                await Task.Delay(50); 
                
                CheckCollision(); // kollarinvader laser collison i Laser(); to do använda klass istället Collision class med en funktion för invader en för ship?
                
            }
            
            if (SpI.List.Count == 0)
            {

                RemoveInvLaser(); 
             
            }

        }

        private void RemoveInvLaser()
        {
            if (Invlaser != null)
            {
                Grid.SetRow(Invlaser, 2);
                //MyGrid.Children.Remove(Invlaser);
                //Invlaser.Visibility = Visibility.Hidden;
            }
        }

        public async void InvLaser2() //används inte för tillfället koden finns ist  i Laser
        {
            while (true)
            {
                //InvFire = true;
                await Task.Delay(75);

                //samma sak som 28 här blir det fel ? och man kan bli träffad än då ShipY är mindre än skeppet det har åkt förbi.
                /*if(InvFireY == ShipY)*/   //to do? då blir det collision men det händer då även då skeppet är över skottet ser ut som att ShipY < InvFireY men det kanske bara ser ut så
                if (InvFireY < 28)    //? fungerade igen efter nedanstående ändringar ShipY 
                {
                    Grid.SetRow(Invlaser, InvFireY++);
                    Grid.SetColumn(Invlaser, InvFireX);
                }

                if ((InvFireY == ShipY && InvFireX == ShipX) || InvFireY >= 28)
                {

                    if (InvFireY == ShipY && InvFireX == ShipX) { isHit = true; } //invader collision detection

                    Random random = new Random(); int randomsi = random.Next(0, SpI.List.Count);
                    InvFireY = SpI.List[randomsi].PosY + 1;
                    InvFireX = SpI.List[randomsi].PosX;

                    Grid.SetColumn(Invlaser, InvFireX);
                    Grid.SetRow(Invlaser, InvFireY);
                    //Invlaser.Visibility = Visibility.Visible;
                }



            }
        }
        public void CreateInvLaser() //to do skapa handler för att skapa ny Image
        {

            //if (InvFire && SpI.List.Count > 0)

            //{
                InvFire = true;//InvFire = false;
                Invlaser = new Image(); Invlaser.Name = "il";
                string path = "/wpfapp1;component/Images/laser.png";
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                Invlaser.Source = image;
                Invlaser.Width = 30; Invlaser.Height = 30;
                Invlaser.Visibility = Visibility.Visible;  

                MyGrid.Children.Add(Invlaser);

                Random random = new Random(); int randomsi = random.Next(0, SpI.List.Count);
                //InvFireY = SpI.List[randomsi].PosY + 1;
                InvFireX = SpI.List[randomsi].PosX;
                InvFireY = SpI.List[randomsi].PosY + 1;
                Grid.SetColumn(Invlaser, InvFireX);
                Grid.SetRow(Invlaser, InvFireY);

                //Random random = new Random(); int randomsi = random.Next(0, SpI.List.Count);
                //InvFireY = ShipY + 3;
                //InvFireY = SpI.List[randomsi].PosY + 1;
                //InvFireX = SpI.List[randomsi].PosX;

                //Grid.SetColumn(Invlaser, SpI.List[randomsi].PosX);

            //}

            //if (InvFireY <= ShipY)
            //{
            //    Grid.SetRow(Invlaser, InvFireY++);
            //}
            //if (InvFireY >= ShipY)
            //{
            //    Grid.SetRow(Invlaser, 0);
            //    RemoveInvLaser();
            //    InvFire = true; // InvFire = t
            //}

        }
        private async void Update()
        {
            while(true)//while (SpI.List.Count!=0)
            {
                await Task.Delay(500);
                if (SpI.List.Count != 0)
                {
                    //myPoints.Text = SpI.List[0].PosY.ToString();
                    invadertoggle = !invadertoggle;
                    UpdateInvaders(invadertoggle);

                    if (!GameIsRunning)
                    {
                        GameOverTxt.Visibility = Visibility.Visible;
                        MyGrid.Children.Remove(myShip);
                        break;
                    }
                }
                else if (SpI.List.Count == 0)
                {
                    RemoveInvLaser();
                    RemoveShipLaser();
                    GameOverTxt.Text = "Success!";
                    GameOverTxt.Visibility = Visibility.Visible;
                }
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

        public void CheckCollision() //invaderlaser detection hits ship flyttad till Laser fungerar nu ändrade pga
        {
            //if (InvFireY == ShipY && InvFireX == ShipX) //collision invader laser hit
            //{
            //    isHit = true; 
            //    myPoints.Text = "tookHit";
            //}

            Toerase = null; //collision ship laser hit invader
            int count = 0;
            foreach (var item in SpI.List)
            {
                count++;

                if (LaserY == item.PosY && LaserX == item.PosX)//&& Toerase == null) //Nextmove ska bara köra igen om spi rört sig
                {
                    Toerase = Images[count - 1];

                    if (Toerase != null)
                    {
                        Fire = false;
                        Points++;

                        SpI.List.Remove((GameObject)item); //tar bort object i invaders klassListan
                        Toerase.Visibility = Visibility.Hidden;
                        Images.Remove(Toerase);//to do remove element image tar bort elementet men ritas ändå ut där den blir skjuten löst det temporärt med Visibliity hidden. tas bort från listan och Image collection i Update. 
                        myPoints.Text = "Points:" + Points.ToString(); //to do points visar bara 4 bokstäver fick ha 2 textblocks ist.

                        break;
                    }
                }

                if(item.PosY == 18  && item.PosX == ShipX ) { isHit = true;} //to do fungerar ej kolla invaders
               
            }
        }   

        private async void ShipHitUpdate(bool toggle) //ship takes hit animation
        {
                while (true)
                {
                    await Task.Delay(500);
                
                if (isHit && count < 4) 
                    {
                        count++;
                        
                        myShip.Visibility = toggle ? Visibility.Visible : Visibility.Hidden;
                        toggle = !toggle;
                    }
                    
                    if (count == 4 && isHit == true)
                    {
                        Hp--; 
                        isHit = false; myShip.Visibility = Visibility.Visible; Health.Text = $"Power:{Hp.ToString()}"; 
                        count = 0;
                   
                    if (SpI.List.Any(inv => inv.PosY > ShipY)) { GameIsRunning = false; }
                    }
                 
                    if(Hp == 0) GameIsRunning = false;
            }
        }

        public async void ShipAnimation() //introanimation
        {
            while (true)
            {
                await Task.Delay(100);

                if (introcount > 0)
                {
                    introcount--;
                   
                    ShipTopMargin = ShipTopMargin - 25;
                    Thickness t = new Thickness(-20, ShipTopMargin, 0, 0);
                    myShip.Margin = t;
                    
                }
                if (introcount == 0) isintro = false;
                
               
            }
        }

            public void WriteInvaders(Grid My_Grid, List<Image> Imagecontrs, string path, int count = 0)
        {
            foreach (var item in SpI.List.ToArray())
            {
                count++; //? = noll i klass
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

            public void UpdateInvadersGrid() //obs bilder högerklicka > välj properties build action > ändra None > resources 
        {
            
            SpI = new SpaceInvaders2(22, SpaceInvaders.Typeui.WpfApp);
           
            SpI.InitEnemies(); //ok
            SpI.List.Count();
            int count = 0; // ?
            int row = 0; // ?
            TextBlock myTextBlock = new TextBlock();
            myTextBlock.Name = "myPointPlayer";
            Grid.SetColumn(myTextBlock, 2); 
            Grid.SetRow(myTextBlock, 2);
            myTextBlock.FontSize = 14;
            //Color ? måste sättas
            
            myTextBlock.Text = "jfksjajkfdska";
            Images = new List<Image>(); //ok ?  

            SpI.DrawInvaders(MyGrid, Images, "/wpfapp1;component/Images/si1_2.png");
            Images = SpI.Images;

            Grid.SetRow(myShip, ShipY); Grid.SetColumn(myShip, ShipX);
            //to do flytta till initmetod eller byt namn tillhör ej spaceinvaders utan ship
            //myShip.Height = 500; myShip.Width = 500;

            myShip.Margin = new Thickness(Left, Top, Right, Bottom); // för margin movement right
        }
           
    }

}
