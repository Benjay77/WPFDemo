using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace JP.HCZZP.WPFApp.Infrastructure.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectorPicker : ComboBox
    {
        #region Private属性
        //private TextBlock PART_TextBox;
        private DateSelector PART_DateSelector;
        private Button PART_PreviousButton;
        private Button PART_NextButton;
        private Popup PART_Popup;
        //private System.Windows.Controls.TextBox PART_TextBox;
        private NumbericTextBox PART_TextBox;
        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #region Value

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(SelectorPicker), new PropertyMetadata(0, ValueChangedCallback, OnValueCoerceValueCallback));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectorPicker timePicker = d as SelectorPicker;
            if (timePicker.DateMode == DateMode.None)
                timePicker.SelectedTimeString = e.NewValue.ToString();
        }
        private static object OnValueCoerceValueCallback(DependencyObject d, object baseValue)
        {
            //不让超出边界暂时没有相关计算
            SelectorPicker timePicker = d as SelectorPicker;
            int value = Convert.ToInt32(baseValue);
            if (value > timePicker.MaxValue)
            {
                return timePicker.MaxValue;
            }
            else if (value < timePicker.MinValue)
            {
                return timePicker.MinValue;
            }
            else return baseValue;
        }
        #endregion

        #region MaxValue

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(SelectorPicker), new PropertyMetadata(int.MaxValue));
        #endregion

        #region MinValue

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(SelectorPicker), new PropertyMetadata(int.MinValue));
        #endregion

        #region DateMode

        public DateMode DateMode
        {
            get { return (DateMode)GetValue(DateModeProperty); }
            set { SetValue(DateModeProperty, value); }
        }
        public static readonly DependencyProperty DateModeProperty =
            DependencyProperty.Register("DateMode", typeof(DateMode), typeof(SelectorPicker), new PropertyMetadata(DateMode.YearMonthDay, DateModeChangedCallback));

        private static void DateModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SelectorPicker timePicker = d as SelectorPicker;
            DateMode dateMode = (DateMode)e.NewValue;
            if (dateMode == DateMode.Day)
            {
                timePicker.StringFormat = "dd日";
            }
            else if (dateMode == DateMode.Month)
            {
                timePicker.StringFormat = "MM月";
            }
            else if (dateMode == DateMode.Year)
            {
                timePicker.StringFormat = "yyyy年";
            }
            else if (dateMode == DateMode.YearMonth)
            {
                timePicker.StringFormat = "yyyy年MM月";
            }
            else if (dateMode == DateMode.YearMonthDay)
            {
                timePicker.StringFormat = "yyyy年MM月dd日";
            }
            else if (dateMode == DateMode.MonthDay)
            {
                timePicker.StringFormat = "MM月dd日";
            }
            if (dateMode == DateMode.None)
            {
                timePicker.SelectedTimeString = timePicker.Value.ToString();
            }
            else if (timePicker.SelectedTime != null)
            {
                timePicker.SelectedTimeString = timePicker.SelectedTime.Value.ToString(timePicker.StringFormat);
            }
        }
        #endregion


        #region SelectedTimeString
        public string SelectedTimeString
        {
            get { return (string)GetValue(SelectedTimeStringProperty); }
            set { SetValue(SelectedTimeStringProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeStringProperty =
            DependencyProperty.Register("SelectedTimeString", typeof(string), typeof(SelectorPicker), new PropertyMetadata(""));
        #endregion

        #region StringFormat
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(SelectorPicker), new PropertyMetadata("yyyy年MM月"));
        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(SelectorPicker), new PropertyMetadata(null
                , SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                SelectorPicker timePicker = d as SelectorPicker;
                DateTime dt = (DateTime)e.NewValue;
                if (timePicker.DateMode != DateMode.None)
                {
                    timePicker.SelectedTimeString = dt.ToString(timePicker.StringFormat);
                }
                if (timePicker.PART_DateSelector != null)
                    timePicker.PART_DateSelector.SelectedTime = dt;
                timePicker.OnSelectedTimeChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
            }

        }
        #endregion

        #region DropDownHeight

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }

        public static readonly DependencyProperty DropDownHeightProperty =
            DependencyProperty.Register("DropDownHeight", typeof(double), typeof(SelectorPicker), new PropertyMetadata(168d));

        #endregion

        #region SelectedTimeChanged
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(SelectorPicker));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add
            {
                this.AddHandler(SelectedTimeChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(SelectedTimeChangedEvent, value);
            }
        }

        public virtual void OnSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedTimeChangedEvent);
            this.RaiseEvent(arg);
        }
        #endregion


        #region NumbericTextBoxChanged
        public static readonly RoutedEvent NumbericTextBoxChangedEvent = EventManager.RegisterRoutedEvent("NumbericTextBoxChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(SelectorPicker));

        public event RoutedPropertyChangedEventHandler<object> NumbericTextBoxChanged
        {
            add
            {
                this.AddHandler(NumbericTextBoxChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(NumbericTextBoxChangedEvent, value);
            }
        }

        public virtual void OnNumbericTextBoxChanged(int? oldValue, int? newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, NumbericTextBoxChangedEvent);
            this.RaiseEvent(arg);
        }
        #endregion
        #endregion

        #region Constructors
        public SelectorPicker()
        {

        }
        static SelectorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectorPicker), new FrameworkPropertyMetadata(typeof(SelectorPicker)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            //this.PART_TextBox = this.GetTemplateChild("PART_TextBox") as TextBlock;
            this.PART_DateSelector = this.GetTemplateChild("PART_DateSelector") as DateSelector;
            this.PART_Popup = this.GetTemplateChild("PART_Popup") as Popup;
            if (DateMode == DateMode.None)
            {
                PART_PreviousButton = this.GetTemplateChild("PART_NextButton") as Button;
                PART_NextButton = this.GetTemplateChild("PART_PreviousButton") as Button;
                PART_TextBox = this.GetTemplateChild("PART_TextBox") as NumbericTextBox;
                PART_TextBox.TextChanged += PART_TextBox_TextChanged;
            }
            else
            {
                PART_PreviousButton = this.GetTemplateChild("PART_PreviousButton") as Button;
                PART_NextButton = this.GetTemplateChild("PART_NextButton") as Button;
            }
            if (this.PART_DateSelector != null)
            {
                this.PART_DateSelector.Owner = this;
                this.PART_DateSelector.SelectedTimeChanged += PART_DateSelector_SelectedTimeChanged;
                if (PART_PreviousButton != null)
                {
                    this.PART_PreviousButton.AddHandler(MouseLeftButtonDownEvent, new RoutedEventHandler(PART_PreviousButton_Click), true);

                }
                if (PART_NextButton != null)
                {
                    this.PART_NextButton.AddHandler(MouseLeftButtonDownEvent, new RoutedEventHandler(PART_NextButton_Click), true);
                }
            }
            if (this.PART_Popup != null)
            {
                this.PART_Popup.Opened += PART_Popup_Opened;
            }
            if (SelectedTime == null)
                SelectedTime = Global.SystemTime;
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumbericTextBox textBox = e.OriginalSource as NumbericTextBox;
            SelectorPicker selectorPicker = GetParentObject<SelectorPicker>(textBox, "");
            if (selectorPicker!=null)
            {
                //System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^[1-9]\d{0,2}$");
                textBox.Text = textBox.Text.Length > 1 ? textBox.Text.TrimStart('0'): textBox.Text;
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    int newValue = int.Parse(textBox.Text.Trim());
                    if (newValue > textBox.MaxValue)
                    {
                        selectorPicker.Value = int.Parse(textBox.MaxValue.ToString());
                    }
                    else if (newValue < textBox.MinValue)
                    {
                        selectorPicker.Value = int.Parse(textBox.MinValue.ToString());
                    }
                    else
                    {
                        selectorPicker.Value = newValue;
                    }
                }
                selectorPicker.OnNumbericTextBoxChanged(null, null);
                //else
                //{
                //    selectorPicker.Value = selectorPicker.MinValue;
                //}
            }
        }

        public T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        private void PART_PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateMode == DateMode.None)
            {
                Value--;
            }
            else
            {
                if (SelectedTime != null)
                {
                    if (DateMode == DateMode.Day || DateMode == DateMode.YearMonthDay || DateMode == DateMode.MonthDay)
                    {
                        SelectedTime = SelectedTime.Value.AddDays(-1);
                    }
                    else if (DateMode == DateMode.Month || DateMode == DateMode.YearMonth)
                    {
                        SelectedTime = SelectedTime.Value.AddMonths(-1);
                    }
                    else
                    {
                        SelectedTime = SelectedTime.Value.AddYears(-1);
                    }
                }
            }


        }
        private void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateMode == DateMode.None)
            {
                Value++;
            }
            else
            {
                if (SelectedTime != null)
                {
                    if (DateMode == DateMode.Day || DateMode == DateMode.YearMonthDay || DateMode == DateMode.MonthDay)
                    {
                        SelectedTime = SelectedTime.Value.AddDays(1);
                    }
                    else if (DateMode == DateMode.Month || DateMode == DateMode.YearMonth)
                    {
                        SelectedTime = SelectedTime.Value.AddMonths(1);
                    }
                    else
                    {
                        SelectedTime = SelectedTime.Value.AddYears(1);
                    }
                }
            }
        }
        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            this.PART_DateSelector.SetButtonSelected();
        }
        #endregion

        #region Private方法
        private void PART_DateSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (e.NewValue != null)
            {
                if (SelectedTime == null || SelectedTime.Value != e.NewValue)
                    this.SelectedTime = e.NewValue;
            }
        }
        #endregion
    }

    public class DateSelector : Control
    {
        private ObservableCollection<DateButton> YearButtons = new ObservableCollection<DateButton>();
        private ObservableCollection<DateButton> MonthButtons = new ObservableCollection<DateButton>();
        private ObservableCollection<DateButton> DayButtons = new ObservableCollection<DateButton>();
        private ObservableCollection<DateButton> NoneButtons = new ObservableCollection<DateButton>();

        #region 控件内部元素
        private ListBox PART_Year;
        private ListBox PART_Month;
        private ListBox PART_Day;
        private ListBox PART_None;

        #endregion

        #region 私有属性
        private int Year = 1;
        private int Month = 1;
        private int Day = 1;
        #endregion

        #region Fields
        public SelectorPicker Owner { get; set; }
        #endregion

        #region 事件定义

        #region SelectedTimeChanged
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(DateSelector));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add
            {
                this.AddHandler(SelectedTimeChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(SelectedTimeChangedEvent, value);
            }
        }

        public virtual void OnSelectedTimeChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedTimeChangedEvent);
            this.RaiseEvent(arg);
        }
        #endregion

        #endregion

        #region 依赖属性set get

        #region DateMode

        public DateMode DateMode
        {
            get { return (DateMode)GetValue(DateModeProperty); }
            set { SetValue(DateModeProperty, value); }
        }
        public static readonly DependencyProperty DateModeProperty =
            DependencyProperty.Register("DateMode", typeof(DateMode), typeof(DateSelector), new PropertyMetadata(DateMode.YearMonthDay));
        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(DateSelector), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateSelector timeSelector = d as DateSelector;
            DateTime dt = (DateTime)e.NewValue;

            timeSelector.SetButtonSelected();

            timeSelector.OnSelectedTimeChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        public void SetButtonSelected()
        {
            if (!this.SelectedTime.HasValue)
            {
                return;
            }

            this.Year = this.SelectedTime.Value.Year;
            this.Month = this.SelectedTime.Value.Month;
            this.Day = this.SelectedTime.Value.Day;

            if (this.PART_Year != null)
            {
                for (int i = 0; i < this.PART_Year.Items.Count; i++)
                {
                    DateButton timeButton = this.PART_Year.Items[i] as DateButton;
                    if (Convert.ToString(timeButton.DataContext).Equals(Convert.ToString(this.SelectedTime.Value.Year)))
                    {
                        this.PART_Year.SelectedIndex = i;
                        this.PART_Year.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (this.PART_Month != null)
            {
                for (int i = 0; i < this.PART_Month.Items.Count; i++)
                {
                    DateButton timeButton = this.PART_Month.Items[i] as DateButton;
                    if (Convert.ToString(timeButton.DataContext).Equals(Convert.ToString(this.SelectedTime.Value.Month)))
                    {
                        this.PART_Month.SelectedIndex = i;
                        this.PART_Month.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }

            if (this.PART_Day != null)
            {
                for (int i = 0; i < this.PART_Day.Items.Count; i++)
                {
                    DateButton timeButton = this.PART_Day.Items[i] as DateButton;
                    if (Convert.ToString(timeButton.DataContext).Equals(Convert.ToString(this.SelectedTime.Value.Day)))
                    {
                        this.PART_Day.SelectedIndex = i;
                        this.PART_Day.AnimateScrollIntoView(timeButton);
                        break;
                    }
                }
            }
        }
        #endregion

        #region SelectedYear
        public int? SelectedYear
        {
            get { return (int?)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }

        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear", typeof(int?), typeof(DateSelector), new PropertyMetadata(null, SelectedYearChanged, CoerceYearValueCallback));

        private static void SelectedYearChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateSelector dateSelector = d as DateSelector;
            dateSelector.Year = e.NewValue == null ? 0 : (int)e.NewValue;
            dateSelector.SetSelectedDate();
        }
        private static object CoerceYearValueCallback(DependencyObject d, object baseValue)
        {
            if ((int)baseValue < Global.SystemTime.AddYears(-30).Year)
            {
                return Global.SystemTime.AddYears(-30).Year;
            }
            return baseValue;
        }
        #endregion

        #region SelectedMonth

        public int? SelectedMonth
        {
            get { return (int?)GetValue(SelectedMonthProperty); }
            set { SetValue(SelectedMonthProperty, value); }
        }

        public static readonly DependencyProperty SelectedMonthProperty =
            DependencyProperty.Register("SelectedMonth", typeof(int?), typeof(DateSelector), new PropertyMetadata(null, SelectedMonthChanged, CoerceMonthValueCallback));

        private static void SelectedMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateSelector timeSelector = d as DateSelector;

            timeSelector.Month = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedDate();
        }

        private static object CoerceMonthValueCallback(DependencyObject d, object baseValue)
        {
            if ((int)baseValue < 1)
            {
                DateSelector timeSelector = d as DateSelector;
                timeSelector.Year--;
                return 12;
            }
            return baseValue;
        }
        #endregion

        #region SelectedDay

        public int? SelectedDay
        {
            get { return (int?)GetValue(SelectedDayProperty); }
            set { SetValue(SelectedDayProperty, value); }
        }

        public static readonly DependencyProperty SelectedDayProperty =
            DependencyProperty.Register("SelectedDay", typeof(int?), typeof(DateSelector), new PropertyMetadata(null, SelectedDayChanged));

        private static void SelectedDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateSelector timeSelector = d as DateSelector;

            timeSelector.Day = e.NewValue == null ? 0 : (int)e.NewValue;
            timeSelector.SetSelectedDate();
        }

        #endregion

        #region ItemHeight
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(DateSelector), new PropertyMetadata(28.0));
        #endregion

        #endregion

        #region Constructors
        static DateSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateSelector), new FrameworkPropertyMetadata(typeof(DateSelector)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Year = this.GetTemplateChild("PART_Year") as ListBox;
            this.PART_Month = this.GetTemplateChild("PART_Month") as ListBox;
            this.PART_Day = this.GetTemplateChild("PART_Day") as ListBox;
            if (this.PART_Year != null)
            {
                this.CreateYearButtons();
                this.PART_Year.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(YearButton_Click), true);
            }

            if (this.PART_Month != null)
            {
                this.CreateMonthButtons();
                this.PART_Month.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(MonthButton_Click), true);
            }

            if (this.PART_Day != null)
            {
                this.CreateDayButtons();
                this.PART_Day.AddHandler(ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(DayButton_Click), true);
            }
            if (SelectedTime == null || SelectedTime.Value <= DateTime.MinValue)
            {
                SelectedTime = Global.SystemTime;
            }
        }
        #endregion

        #region Private方法

        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton selectedItem = this.PART_Month.SelectedItem as DateButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedMonth = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Month.AnimateScrollIntoView(selectedItem);
        }
        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton selectedItem = this.PART_Day.SelectedItem as DateButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedDay = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Day.AnimateScrollIntoView(selectedItem);
        }

        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            DateButton selectedItem = this.PART_Year.SelectedItem as DateButton;
            if (selectedItem == null)
            {
                return;
            }
            this.SelectedYear = Convert.ToInt32(selectedItem.DataContext);
            this.PART_Year.AnimateScrollIntoView(selectedItem);
        }

        private void CreateYearButtons()
        {

            this.CreateItems(60, this.YearButtons, "{0}年", Global.SystemTime.AddYears(-30).Year);
            this.CreateExtraItem(this.YearButtons);
            this.PART_Year.ItemsSource = this.YearButtons;
        }

        private void CreateMonthButtons()
        {
            this.CreateItems(12, this.MonthButtons, "{0}月");
            this.CreateExtraItem(this.MonthButtons);
            this.PART_Month.ItemsSource = this.MonthButtons;
        }

        private void CreateDayButtons()
        {
            this.CreateItems(DateTime.DaysInMonth(Year, Month), this.DayButtons, "{0}日");
            this.CreateExtraItem(this.DayButtons);
            this.PART_Day.ItemsSource = this.DayButtons;
        }

        private void CreateItems(int itemsCount, ObservableCollection<DateButton> list, string StringFormat, int Start = 1)
        {
            for (int i = 0; i < itemsCount; i++)
            {
                int value = i + Start;
                DateButton timeButton = new DateButton();
                timeButton.SetValue(DateButton.HeightProperty, this.ItemHeight);
                timeButton.SetValue(DateButton.DataContextProperty, value);
                timeButton.SetValue(DateButton.ContentProperty, string.Format(StringFormat, value));
                timeButton.SetValue(DateButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }

        private void CreateExtraItem(ObservableCollection<DateButton> list)
        {
            double height = this.ItemHeight;
            if (this.Owner != null)
            {
                height = this.Owner.DropDownHeight;
            }
            else
            {
                height = double.IsNaN(this.Height) ? height : this.Height;
            }

            for (int i = 0; i < (height - this.ItemHeight) / this.ItemHeight; i++)
            {
                DateButton timeButton = new DateButton();
                timeButton.SetValue(DateButton.HeightProperty, this.ItemHeight);
                timeButton.SetValue(DateButton.IsEnabledProperty, false);
                timeButton.SetValue(DateButton.IsSelectedProperty, false);
                list.Add(timeButton);
            }
        }



        /// <summary>
        /// 设置选中的时间
        /// </summary>
        private void SetSelectedDate()
        {
            SelectedTime = new DateTime(Year, Month, Day);
        }
        #endregion
    }

    public class DateButton : ListBoxItem
    {
        #region Private属性
        #endregion

        #region 依赖属性定义

        #endregion

        #region 依赖属性set get

        #endregion

        #region Constructors
        static DateButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateButton), new FrameworkPropertyMetadata(typeof(DateButton)));
        }
        #endregion

        #region Override方法
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #endregion

        #region Private方法

        #endregion
    }

    public enum DateMode
    {
        Year, Month, Day, YearMonth, MonthDay, YearMonthDay, None
    }
}
