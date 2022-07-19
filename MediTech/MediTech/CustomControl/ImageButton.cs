using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace MediTech.CustomControl
{
    public class ImageButton : Button
    {
        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton));

        public ImageSource SelectedSource
        {
            get { return base.GetValue(SelectedSourceProperty) as ImageSource; }
            set { base.SetValue(SelectedSourceProperty, value); }
        }
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(ImageSource), typeof(ImageButton));

        public ImageSource HighlightSource
        {
            get { return base.GetValue(HighlightSourceProperty) as ImageSource; }
            set { base.SetValue(HighlightSourceProperty, value); }
        }
        public static readonly DependencyProperty HighlightSourceProperty = DependencyProperty.Register("HighlightSource", typeof(ImageSource), typeof(ImageButton));


        public ImageButton()
        {
            //InitializeComponent();

            #region event handler setup
            this.MouseEnter += new MouseEventHandler(ImageButton_MouseEnter);
            this.MouseLeave += new MouseEventHandler(ImageButton_MouseLeave);
            #endregion

            //add adorner

            //AdornerLayer.GetAdornerLayer(this).Add(new ApplicationButtonAdorner(this));


        }

        private void ImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Source = SelectedSource;
        }

        private void ImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Source = HighlightSource;
        }
    }
}
