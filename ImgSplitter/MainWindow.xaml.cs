using ImgSplitter.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ImgSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage imageToCut;
        private string imageExtension;

        public MainWindow()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            this.FontFamily = new FontFamily("Tw Cen MT");
        }

        private bool _isMouseDown = false;

        private void Drag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            this.DragMove();
        }

        private void Drag_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }
        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown && this.WindowState == WindowState.Maximized)
            {
                _isMouseDown = false;
                this.WindowState = WindowState.Normal;
            }
        }

        public void GetImageButton(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|JPEG Files (*.jpeg)|*.jpeg";

            bool? result = openFileDialog.ShowDialog();
            if (result == true){
                BitmapImage uploadedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                int imgWidth = uploadedImage.PixelWidth;
                int imgHeight = uploadedImage.PixelHeight;

                if (imgWidth != imgHeight){
                    MessageBox.Show("Obraz nie jest kwadratem!");
                    image.Source = null;
                    fileName.Text = "";
                    tmp.IsEnabled = true;
                }
                else {
                    image.Source = uploadedImage;
                    imageToCut = uploadedImage;
                    imageExtension = System.IO.Path.GetExtension(openFileDialog.FileName);
                    fileName.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
                    tmp.IsEnabled = false;
                }
            }
        }

        public void SetMeasures(object sender, RoutedEventArgs args)
        {
            string value = measures.Text;
            int measureValue;

            if (canvas.Children.OfType<Line>() != null){
                DeleteMeasures();
            }

            if (int.TryParse(value, out measureValue) && measureValue>1 && measureValue<17)
            {
                double part = (double)300 / measureValue;

                for (double i = 0; i <= 300; i += part){
                    if (i + part > 300){
                        i = 300;
                    }

                    canvas.Children.Add(new Line
                    {
                        X1 = i,
                        Y1 = 0,
                        X2 = i,
                        Y2 = 300,
                        Stroke = Brushes.White,
                        StrokeThickness = 1,
                    });

                    canvas.Children.Add(new Line
                    {
                        X1 = 0,
                        Y1 = i,
                        X2 = 300,
                        Y2 = i,
                        Stroke = Brushes.White,
                        StrokeThickness = 1,
                    });
                }
            }
        }

        public void DeleteMeasures()
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i += -1){
                UIElement Child = canvas.Children[i];
                if (Child is Line)
                    canvas.Children.Remove(Child);
            }
        }

        public void SaveButton(object sender, RoutedEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Wybieranie folderu.";

            bool? result = openFileDialog.ShowDialog();
            if (result == true){
                int measureValue = int.Parse(measures.Text);
                int part = imageToCut.PixelHeight / measureValue;
                if (part <= 0){
                    MessageBox.Show("Nie można podzielić obrazu!");
                }
                else{
                    CroppedBitmap[] croppedBitmaps = CutImage(measureValue, part);
                    string savePath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    SaveFiles(croppedBitmaps, savePath);
                    MessageBox.Show("Pomyślnie zapisano do plików.");
                }
            } 
        }
        public CroppedBitmap[] CutImage(int measureValue, int part)
        {
            CroppedBitmap[] croppedBitmaps = new CroppedBitmap[measureValue * measureValue];
            int counter = 0;

            for (int i=0; i< measureValue; i++){
                for (int j=0; j< measureValue; j++){
                    if (part*i > imageToCut.PixelHeight){
                        part = imageToCut.PixelHeight - part * i;
                    }
                    croppedBitmaps[counter++] = new CroppedBitmap(imageToCut, new Int32Rect(j * part, i * part, part, part));
                }
            }
            return croppedBitmaps;
        }

        public void SaveFiles(CroppedBitmap[] croppedBitmaps, string directoryPath)
        {
            int fileNameStart = int.Parse(startNumber.Text);
            for (int i = 0; i < croppedBitmaps.Length; i++){
                string filePath = directoryPath + "\\" + fileNameStart + imageExtension;
                fileNameStart++;

                using (var fileStream = new FileStream(filePath, FileMode.Create)){
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(croppedBitmaps[i]));
                    encoder.Save(fileStream);
                }  
            }
                
        }
    }
}
