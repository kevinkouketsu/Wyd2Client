using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Wyd2.Client.Component
{
    public struct MiniMapPositionName
    {
        public Point Minimum { get; }
        public Point Maximum { get; }
        public string Name { get; }

        public MiniMapPositionName(Point minimum, Point maximum, string name)
        {
            Minimum = minimum;
            Maximum = maximum;

            Name = name;
        }

        public MiniMapPositionName(string name)
        {
            Minimum = new Point(0, 0);
            Maximum = new Point(4096, 4096);

            Name = name;
        }
    }
    /// <summary>
    /// Interação lógica para MiniMap.xam
    /// </summary>
    public partial class MiniMap : UserControl
    {
        public static readonly DependencyProperty MiniMapCroppedProperty =
            DependencyProperty.Register("MiniMapCropped", typeof(CroppedBitmap), typeof(MiniMap), new PropertyMetadata(null));

        public static readonly DependencyProperty MiniMapImageProperty =
            DependencyProperty.Register("MiniMapImage", typeof(ImageSource), typeof(MiniMap), new PropertyMetadata(null));

        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.Register("PositionX", typeof(int), typeof(MiniMap), new PropertyMetadata(0, new PropertyChangedCallback(new PropertyChangedCallback(PositionXChanged))));

        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.Register("PositionY", typeof(int), typeof(MiniMap), new PropertyMetadata(0, new PropertyChangedCallback(new PropertyChangedCallback(PositionYChanged))));

        public static readonly DependencyProperty MinimapZoomProperty =
            DependencyProperty.Register("MinimapZoom", typeof(int), typeof(MiniMap), new PropertyMetadata(50, new PropertyChangedCallback(new PropertyChangedCallback(MiniMapZoomChanged))));

        public static readonly DependencyProperty PositionTextProperty =
            DependencyProperty.Register("PositionText", typeof(string), typeof(MiniMap), new PropertyMetadata(""));

        public static readonly DependencyProperty PositionNamesProperty =
            DependencyProperty.Register("PositionNames", typeof(IList<MiniMapPositionName>), typeof(MiniMap), new PropertyMetadata(null));

        public static readonly DependencyProperty PositionNameProperty =
            DependencyProperty.Register("PositionName", typeof(MiniMapPositionName), typeof(MiniMap), new PropertyMetadata(null));

        public MiniMapPositionName PositionName
        {
            get { return (MiniMapPositionName)GetValue(PositionNameProperty); }
            set { SetValue(PositionNameProperty, value); }
        }

        public IList<MiniMapPositionName> PositionNames
        {
            get { return (IList<MiniMapPositionName>)GetValue(PositionNamesProperty); }
            set { SetValue(PositionNamesProperty, value); }
        }

        public CroppedBitmap MiniMapCropped
        {
            get { return (CroppedBitmap)GetValue(MiniMapCroppedProperty); }
            set { SetValue(MiniMapCroppedProperty, value); }
        }

        public int MinimapZoom
        {
            get { return (int)GetValue(MinimapZoomProperty); }
            set { SetValue(MinimapZoomProperty, value); }
        }

        public ImageSource MiniMapImage
        {
            get { return (ImageSource)GetValue(MiniMapImageProperty); }
            set { SetValue(MiniMapImageProperty, value); }
        }

        public int PositionX
        {
            get { return (int)GetValue(PositionXProperty); }
            set { SetValue(PositionXProperty, value); }
        }
        public int PositionY
        {
            get { return (int)GetValue(PositionYProperty); }
            set { SetValue(PositionYProperty, value); }
        }

        public string PositionText
        {
            get { return (string)GetValue(PositionTextProperty); }
            set { SetValue(PositionTextProperty, value); }
        }

        private static void PositionYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MiniMap;
            obj.CropMiniMap(obj.PositionX, (int)e.NewValue);
            obj.RefreshPositionName();
        }

        private static void PositionXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MiniMap;
            obj.CropMiniMap((int)e.NewValue, obj.PositionY);

            obj.RefreshPositionName();
        }

        private void RefreshPositionName()
        {
            foreach (var i in PositionNames)
            {
                if (PositionX >= i.Minimum.X && PositionX <= i.Maximum.X && PositionY >= i.Minimum.Y && PositionY <= i.Maximum.Y)
                {
                    PositionName = i;

                    return;
                }
            }

            PositionName = new MiniMapPositionName("Desconhecido");
        }

        private void CropMiniMap(int posX, int posY)
        {
            posX -= (MinimapZoom / 2);
            posY -= (MinimapZoom / 2);

            if (posX < 0)
                posX = 0;
            if (posY < 0)
                posY = 0;

            MiniMapCropped = new CroppedBitmap((BitmapSource)MiniMapImage, new Int32Rect(posX, posY, MinimapZoom, MinimapZoom));
            var target = new RenderTargetBitmap(MiniMapCropped.PixelWidth, MiniMapCropped.PixelHeight, MiniMapCropped.DpiX, MiniMapCropped.DpiY, PixelFormats.Pbgra32);
            var visual = new DrawingVisual();

            using (var r = visual.RenderOpen())
            {
                r.DrawLine(new Pen(Brushes.Red, 10.0), new Point(0, 0), new Point(MiniMapCropped.Width, MiniMapCropped.Height));
            }

            target.Render(visual);

            MiniMapCropped.Source = target;
        }

        private static void MiniMapZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MiniMap;

            obj.CropMiniMap(obj.PositionX, obj.PositionY);
        }

        public static DependencyProperty MoveCommandProperty = DependencyProperty.Register(
                "MoveCommand",
                typeof(ICommand),
                typeof(MiniMap));

        public ICommand MoveCommand
        {
            get => (ICommand)GetValue(MoveCommandProperty);
            set => SetValue(MoveCommandProperty, value);
        }

        public MiniMap()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Point teste = Mouse.GetPosition(sender as Button);
            double height = btn.ActualHeight;
            double width = btn.ActualWidth;

            double heightByZoom = height / MinimapZoom;
            double widthByZoom = width / MinimapZoom;

            double getPixelSizeHeight = heightByZoom;
            double getPixelSizeWidth = widthByZoom;

            Point basedOnMouse = new Point();
            basedOnMouse.X = teste.X / getPixelSizeHeight;
            basedOnMouse.Y = teste.Y / getPixelSizeWidth;
            basedOnMouse.X -= (height / 2.0d / getPixelSizeHeight);
            basedOnMouse.Y -= (width / 2.0d / getPixelSizeWidth);

            var finalPosition = new Point(PositionX + basedOnMouse.X, PositionY + basedOnMouse.Y);
            MoveCommand.Execute(finalPosition);
        }

        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            Point teste = Mouse.GetPosition(sender as Button);

            double height = btn.ActualHeight;
            double width = btn.ActualWidth;

            double heightByZoom = height / MinimapZoom;
            double widthByZoom = width / MinimapZoom;

            double getPixelSizeHeight = heightByZoom;
            double getPixelSizeWidth = widthByZoom;

            Point basedOnMouse = new Point();
            basedOnMouse.X = teste.X / getPixelSizeHeight;
            basedOnMouse.Y = teste.Y / getPixelSizeWidth;
            basedOnMouse.X -= (height / 2.0d / getPixelSizeHeight);
            basedOnMouse.Y -= (width / 2.0d / getPixelSizeWidth);

            var finalPosition = new Point(PositionX + basedOnMouse.X, PositionY + basedOnMouse.Y);
            PositionText = $"{ (int)finalPosition.X }x { (int)finalPosition.Y}y";
        }
    }
}
