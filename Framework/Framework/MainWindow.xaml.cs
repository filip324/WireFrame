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

namespace Framework
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        //Zde definuji proměné které potřebuji globálně
        public int state;
        public Line relationBound = new Line();
        public bool linestate = false;


        SolidColorBrush gray = new SolidColorBrush(Colors.Gray);
        public MainWindow()
        {
            
        }
        UIElement dragObject = null;
        Point offset;

        private void UserCTRL_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            //UserCTRL_PreviewMousDown umožnuje drag and drop pro objekty
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                this.dragObject = sender as UIElement;
                this.offset = e.GetPosition(this.CanvasMain);
                this.offset.Y -= Canvas.GetTop(this.dragObject);
                this.offset.X -= Canvas.GetLeft(this.dragObject);
                this.CanvasMain.CaptureMouse();
            }
           
        }   
        private void CanvasMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //CanvasMain_PreviewMouseMove umožnuje preview drag and dropu
            if (this.dragObject == null)
                return;
            var position = e.GetPosition(sender as IInputElement);
            Canvas.SetTop(this.dragObject, position.Y - this.offset.Y);
            Canvas.SetLeft(this.dragObject, position.X - this.offset.X);
        }
        private void CanvasMain_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Zde se drag and drop releasuje
            this.dragObject = null;
            this.CanvasMain.ReleaseMouseCapture();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Zde se tvoří komponenta userCTRL, která lze táhnout a představuje prvek.
            //userCTRL se odkazuje na OnMouseRightButtonDown aby mohl posouvat objekt po canvasu.
            InitializeComponent();
            Ellipse userCTRL = new Ellipse();
            userCTRL.MouseRightButtonUp += OnMouseRightButtonDown;
            userCTRL.Fill = Brushes.Purple;
            userCTRL.Width = 100;
            userCTRL.Height = 100;
            Canvas.SetTop(userCTRL, 20);
            Canvas.SetLeft(userCTRL, 150);
            Canvas.SetZIndex(userCTRL, 1);
            userCTRL.PreviewMouseDown += UserCTRL_PreviewMouseDown;
            CanvasMain.Children.Add(userCTRL);

        }
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) 
        {
            //OnMouseRightButtonDown má trigger na stisknutí pravého tlačítka.
            //Zde se kontroluje zda myš není nad nějakým objektem(v tomto případě elipse neboli userCTRL).
            if (e.OriginalSource is Ellipse)
            {
                //Tento kód ostraní daný objekt.
                Ellipse activeEl = (Ellipse)e.OriginalSource;
                CanvasMain.Children.Remove(activeEl);
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //OnMouseLeftButtonDown má trigger na stistknutí levého tlačítka.
            //Zde se definují body pro linku RelationBound
            if(linestate == false)
            {
                double locationX1 = e.GetPosition(this.CanvasMain).X;
                double locationY1 = e.GetPosition(this.CanvasMain).Y;

                relationBound.X1 = locationX1;
                relationBound.Y1 = locationY1;
                relationBound.X2 = locationX1;
                relationBound.Y2 = locationY1;

            }

        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //OnMouseLeftButtonUp má trigger na puštení levého tlačítka.
            //Zde se definují koncové body linky RelationBound
            if (linestate == false) { 
                relationBound.X2 = e.GetPosition(this.CanvasMain).X;
                relationBound.Y2 = e.GetPosition(this.CanvasMain).Y;
                linestate = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Trigger tlačítka pro tvorbu RelationBound
            SolidColorBrush relationBoundColor = new SolidColorBrush();
            relationBoundColor.Color = Colors.Red;
            relationBound.StrokeThickness = 4;
            relationBound.Stroke = relationBoundColor;

            relationBound.X1 = 0;
            relationBound.X2 = 0;
            relationBound.Y1 = 0;
            relationBound.Y2 = 0;

            InitializeComponent();
            CanvasMain.MouseLeftButtonDown += OnMouseLeftButtonDown;
            CanvasMain.MouseLeftButtonUp += OnMouseLeftButtonUp;
            CanvasMain.Children.Add(relationBound);   
        }
    }
}
