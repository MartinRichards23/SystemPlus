using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SystemPlus.Windows.Controls
{
    public delegate void NumberChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Control for inputing numeric value
    /// </summary>
    public partial class NumericUpDown
    {
        #region Fields

        public event NumberChangedEventHandler? ValueChanged;

        //static string decimalSeparator;
        //static string negativeSign;
        bool allowDecimal = true;
        bool allowNegative = true;
        string numberFormat = string.Empty;
        double maxValue = double.MaxValue;
        double minValue = double.MinValue;

        bool updateOnTextChanged = true;
        bool valid = true;
        double increment = 1;

        double? currentValue;

        #endregion

        public NumericUpDown()
        {
            InitializeComponent();

            //NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            //decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            //negativeSign = numberFormatInfo.NegativeSign;
        }

        #region Properties

        public Visibility ButtonsVisibility
        {
            get { return spButtons.Visibility; }
            set { spButtons.Visibility = value; }
        }

        public double Increment
        {
            get { return increment; }
            set { increment = value; }
        }

        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        public bool AllowDecimal
        {
            get { return allowDecimal; }
            set { allowDecimal = value; }
        }

        public bool AllowNegative
        {
            get { return allowNegative; }
            set { allowNegative = value; }
        }

        public string NumberFormat
        {
            get { return numberFormat; }
            set { numberFormat = value; }
        }

        public bool AllowTextInput
        {
            get { return !txtValue.IsReadOnly; }
            set { txtValue.IsReadOnly = !value; }
        }

        public bool UpdateOnTextChanged
        {
            get { return updateOnTextChanged; }
            set { updateOnTextChanged = value; }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(NumericUpDown), new PropertyMetadata(null, OnValueChanged, CoerceValue));

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double? value = (double?)e.NewValue;
            NumericUpDown control = (NumericUpDown)d;
            control.UpdateValue(value);
        }

        static object CoerceValue(DependencyObject d, object value)
        {
            double? newVal = (double?)value;
            NumericUpDown control = (NumericUpDown)d;

            if (newVal == null)
                return null;

            double val = (double)newVal;
            val = Math.Max(val, control.MinValue);
            val = Math.Min(val, control.MaxValue);
            return val;
        }

        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double ValueSafe
        {
            get
            {
                if (Value == null)
                    return 0;

                return (double)Value;
            }
            set { Value = value; }
        }

        #endregion

        void UpdateValue(double? val)
        {
            if (val == null)
            {
                txtValue.Text = string.Empty;
                return;
            }

            double newVal = (double)val;

            if (currentValue != newVal)
            {
                currentValue = newVal;

                if (string.IsNullOrEmpty(numberFormat))
                    txtValue.Text = newVal.ToString(CultureInfo.InvariantCulture);
                else
                    txtValue.Text = newVal.ToString(numberFormat, CultureInfo.InvariantCulture);
            }
        }

        void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            double? origVal = Value;

            ValueSafe += Increment;

            if (Value != origVal)
                OnValueChanged();
        }

        void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            double? origVal = Value;

            ValueSafe -= Increment;

            if (Value != origVal)
                OnValueChanged();
        }

        void OnValueChanged()
        {
            if (!IsLoaded)
                return;

            ValueChanged?.Invoke(this, new EventArgs());
        }

        void NumericTextbox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isNumPadNumeric = (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);
            bool isNumeric = (e.Key >= Key.D0 && e.Key <= Key.D9);
            valid = false;

            if (Keyboard.Modifiers != ModifierKeys.None)
            {
                e.Handled = true;
                return;
            }

            if (isNumeric || isNumPadNumeric)
            {
                valid = true;
                return;
            }

            if (allowDecimal && (e.Key == Key.OemPeriod || e.Key == Key.Decimal))
            {
                valid = true;
                return;
            }

            if (allowNegative && e.Key == Key.OemMinus)
            {
                valid = true;
                return;
            }

            if (e.Key == Key.Return)
            {
                valid = false;
                Apply();
                return;
            }

            bool isControl = (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Tab || e.Key == Key.PageDown || e.Key == Key.PageUp || e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Escape || e.Key == Key.Home || e.Key == Key.End);

            if (isControl)
            {
                valid = true;
                return;
            }

            e.Handled = true;
        }

        void TxtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UpdateOnTextChanged && valid)
            {
                Apply();
                valid = false;
            }
        }

        void Apply()
        {
            double? origVal = Value;
            if (double.TryParse(txtValue.Text, out double newVal))
            {
                Value = newVal;
            }

            if (Value != origVal)
                OnValueChanged();
        }
    }
}