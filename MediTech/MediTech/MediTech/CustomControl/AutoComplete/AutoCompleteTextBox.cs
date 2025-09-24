using DevExpress.Xpf.Grid;
using MediTech.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace MediTech.CustomControl
{
    public class AutoCompleteTextBox : TextBox
    {
        Popup popup;
        GridControl gridControl;
        string textCache = "";
        //bool suppressEvent = false;
        // Binding hack - not really necessary.
        //DependencyObject dummy = new DependencyObject();
        FrameworkElement dummy = new FrameworkElement();

        #region ColumsSource Dependency Property


        public IEnumerable ColumnsSource
        {
            get { return (IEnumerable)GetValue(ColumnsSourceProperty); }
            set { SetValue(ColumnsSourceProperty, value); }
        }


        public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.Register("ColumnsSource", typeof(IEnumerable)
            , typeof(AutoCompleteTextBox), new UIPropertyMetadata(null, OnColumsSourceChanged));



        private static void OnColumsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnColumsSourceChanged(e.NewValue as IEnumerable);

        }

        protected void OnColumsSourceChanged(IEnumerable columsSource)
        {
            if (gridControl == null) return;

            if (columsSource is IEnumerable)
            {
                gridControl.ColumnsSource = columsSource;
            }
        }

        #endregion

        #region ItemsSource Dependency Property
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object)
            , typeof(AutoCompleteTextBox), new UIPropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (gridControl == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                gridControl.ItemsSource = new LimitedListCollectionView((IList)((ListCollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions }.SourceCollection;
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                gridControl.ItemsSource = new LimitedListCollectionView(((CollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions }.SourceCollection;
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                gridControl.ItemsSource = new LimitedListCollectionView((IList)itemsSource) { Limit = MaxCompletions }.SourceCollection;
                if ((itemsSource as IList).Count == 0)
                {
                    gridControl.ItemsSource = null;
                }
                Debug.Print("Was IList");
            }
            else
            {
                gridControl.ItemsSource = null;
            }

            if (!OnTextChangedEvent)
            {
                if (popup != null)
                {
                    if (gridControl.ItemsSource == null)
                        InternalClosePopup();
                    else
                        InternalOpenPopup();
                }
            }
            //if (OnTextChangedEvent)
            //{
            //    if (gridControl.ItemsSource == null) InternalClosePopup();
            //}

        }

        #endregion

        #region Binding Dependency Property

        public string Binding
        {
            get { return (string)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof(string), typeof(AutoCompleteTextBox), new UIPropertyMetadata(null));

        #endregion

        #region MaxCompletions Dependency Property

        public int MaxCompletions
        {
            get { return (int)GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxCompletions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCompletionsProperty =
            DependencyProperty.Register("MaxCompletions", typeof(int), typeof(AutoCompleteTextBox), new UIPropertyMetadata(int.MaxValue));

        #endregion

        #region OnTextChangedEvent Dependency Property

        public bool OnTextChangedEvent
        {
            get { return (bool)GetValue(OnTextChangedEventProperty); }
            set { SetValue(OnTextChangedEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnTextChangedEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnTextChangedEventProperty =
            DependencyProperty.Register("OnTextChanged", typeof(bool), typeof(AutoCompleteTextBox), new UIPropertyMetadata(false));

        #endregion

        #region SuppressEvent Dependency Property
        public bool SuppressEvent
        {
            get { return (bool)GetValue(SuppressEventProperty); }
            set { SetValue(SuppressEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuppressEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuppressEventProperty =
            DependencyProperty.Register("SuppressEvent", typeof(bool), typeof(AutoCompleteTextBox), new UIPropertyMetadata(false));
        #endregion

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        private void InternalOpenPopup()
        {
            popup.IsOpen = true;
            if (gridControl != null)
            {
                if (gridControl.ItemsSource != null)
                {
                    gridControl.RefreshData();
                    gridControl.SelectedItem = null;
                }
            }
        }

        private void InternalClosePopup()
        {
            if (popup != null)
                popup.IsOpen = false;
        }

        public void ShowPopup()
        {
            if (gridControl == null || popup == null) InternalClosePopup();
            else if (gridControl.ItemsSource == null) InternalClosePopup();
            else InternalOpenPopup();
        }

        public void ClosePopup()
        {
            InternalClosePopup();
        }

        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (popup != null)
            {
                InternalClosePopup();
                Dispatcher.Invoke(new Action(() =>
                {
                    Focus();
                    if (moveFocus)
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }), System.Windows.Threading.DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            var originalBinding = BindingOperations.GetBinding(this, BindingProperty);

            // Binding hack - not really necessary.
            //Binding newBinding = new Binding()
            //{
            //    Path = new PropertyPath(originalBinding.Path.Path, originalBinding.Path.PathParameters),
            //    XPath = originalBinding.XPath,
            //    Converter = originalBinding.Converter,
            //    ConverterParameter = originalBinding.ConverterParameter,
            //    ConverterCulture = originalBinding.ConverterCulture,
            //    StringFormat = originalBinding.StringFormat,
            //    TargetNullValue = originalBinding.TargetNullValue,
            //    FallbackValue = originalBinding.FallbackValue
            //};
            //newBinding.Source = obj;
            //BindingOperations.SetBinding(dummy, TextProperty, newBinding);

            // Set the dummy's DataContext to our selected object.
            if (originalBinding != null)
            {
                dummy.DataContext = obj;
                // Apply the binding to the dummy FrameworkElement.
                BindingOperations.SetBinding(dummy, TextProperty, originalBinding);
                SuppressEvent = true;
                Text = dummy.GetValue(TextProperty).ToString();

            }

            SelectedItem = obj;

            SuppressEvent = true;

            // Get the binding's resulting value.

            SuppressEvent = false;
            gridControl.SelectedItem = null;
            SelectAll();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (OnTextChangedEvent)
            {
                base.OnTextChanged(e);
                if (SuppressEvent) return;
                textCache = Text ?? "";
                Debug.Print("Text: " + textCache);
                if (popup != null && textCache == "")
                {
                    InternalClosePopup();
                }
                else if (gridControl != null)
                {


                    if (popup != null)
                    {
                        if (gridControl.ItemsSource == null)
                            InternalClosePopup();
                        else
                            InternalOpenPopup();
                    }
                }
            }

        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (SuppressEvent) return;
            if (popup != null)
            {
                InternalClosePopup();
            }
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);
            if (e.Key == Key.Escape)
            {
                InternalClosePopup();
                Focus();
            }
            else if (e.Key == Key.Down)
            {
                if (gridControl != null && o == this)
                {
                    SuppressEvent = true;
                    gridControl.Focus();
                    SuppressEvent = false;
                    //InternalOpenPopup();
                }
            }
        }


        void gridControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTextValueBySelection(gridControl.SelectedItem, false);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            popup = Template.FindName("PART_Popup", this) as Popup;
            gridControl = Template.FindName("PART_GridControl", this) as GridControl;
            if (gridControl != null)
            {
                gridControl.PreviewMouseDown += new MouseButtonEventHandler(gridControl_PreviewMouseDown);
                gridControl.PreviewKeyDown += gridControl_PreviewKeyDown;
                OnItemsSourceChanged(ItemsSource);
                OnColumsSourceChanged(ColumnsSource);
            }
        }

        void gridControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                SetTextValueBySelection(gridControl.SelectedItem, false);
            else if (e.Key == Key.Tab)
                SetTextValueBySelection(gridControl.SelectedItem, true);
            else if (e.Key == Key.Back)
                this.Focus();

        }



    }

}


