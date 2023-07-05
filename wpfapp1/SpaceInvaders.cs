using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using spaceinvaders;
namespace wpfapp1
{
    public class SpaceInvaders2:SpaceInvaders
    {
        public List<Image> Images { get; set; }

        public SpaceInvaders2(int windowwidth, Typeui ui = Typeui.WpfApp) : base(windowwidth, (SpaceInvaders.Typeui)ui)
        {
        }
        public void DrawInvaders(Grid My_Grid, List<Image> Imagecontrs, string path, int count = 0)
        {
            Images = new List<Image>();
            foreach (var item in List.ToArray())
            {
                count++; 
                var contrtag = "si" + count; 
                var ImageControl = new Image(); ImageControl.Name = contrtag;
                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Relative));
                ImageControl.Source = image;
                ImageControl.Width = 30; ImageControl.Height = 30;
                Images.Add(ImageControl); 
                
                Grid.SetRow(ImageControl, item.PosY);
                Grid.SetColumn(ImageControl, item.PosX);
                My_Grid.RowDefinitions.Add(new RowDefinition());
                My_Grid.ColumnDefinitions.Add(new ColumnDefinition());
                My_Grid.Children.Add(ImageControl);

            }
        }

    }
}
