using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SystemPlus.Windows.Media;

namespace SystemPlus.Windows.Controls
{
    /// <summary>
    /// A TextBox with a Watermark property
    /// </summary>
    public class TextBoxWatermarked : TextBox
    {
        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(TextBoxWatermarked), new PropertyMetadata(OnWatermarkChanged));

        bool _isWatermarked;
        Binding _textBinding;

        public string Watermark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        public TextBoxWatermarked()
        {
            Loaded += (s, ea) => ShowWatermark();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            HideWatermark();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            ShowWatermark();
        }

        static void OnWatermarkChanged(DependencyObject sender, DependencyPropertyChangedEventArgs ea)
        {
            //need to check IsLoaded so that we didn't dive into the ShowWatermark() routine before initial Bindings had been made
            if (sender is TextBoxWatermarked tbw && tbw.IsLoaded)
                tbw.ShowWatermark();

            return; 
        }

        void ShowWatermark()
        {
            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Watermark))
            {
                _isWatermarked = true;

                //save the existing binding so it can be restored
                _textBinding = BindingOperations.GetBinding(this, TextProperty);

                //blank out the existing binding so we can throw in our Watermark
                BindingOperations.ClearBinding(this, TextProperty);

                //set the signature watermark gray
                Foreground = BrushCache.GetBrush(Colors.LightGray);

                //display our watermark text
                Text = Watermark;
            }
        }

        void HideWatermark()
        {
            if (_isWatermarked)
            {
                _isWatermarked = false;
                ClearValue(ForegroundProperty);
                Text = "";
                if (_textBinding != null) SetBinding(TextProperty, _textBinding);
            }
        }
    }
}