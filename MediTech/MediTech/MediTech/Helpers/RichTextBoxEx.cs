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

namespace MediTech.Helpers
{
    public class RichTextBoxEx : RichTextBox
    {
        bool IsSettingText;

        public static readonly DependencyProperty RtfTextProperty =
            DependencyProperty.Register("RtfText", typeof(string), typeof(RichTextBoxEx),
            new FrameworkPropertyMetadata(string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRtfTextChanged));

        static void OnRtfTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RichTextBoxEx)d).LoadText();
        }

        public string RtfText
        {
            get { return GetValue(RtfTextProperty) as string; }
            set { SetValue(RtfTextProperty, value); }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            Binding binding = BindingOperations.GetBinding(this, RtfTextProperty);
            if (binding != null && binding.Mode != BindingMode.OneWay)
            {
                SaveText();
            }
            base.OnTextChanged(e);
        }

        void LoadText()
        {
            if (IsSettingText)
                return;

            string rtfText = RtfText;
            Document.Blocks.Clear();

            if (!string.IsNullOrEmpty(rtfText))
            {
                TextRange tr = new TextRange(Document.ContentStart, Document.ContentEnd);

                using (MemoryStream rtfMemoryStream =
                new MemoryStream(Encoding.ASCII.GetBytes(rtfText)))
                {

                    tr.Load(rtfMemoryStream, DataFormats.Rtf);
                }
            }
        }

        void SaveText()
        {
            TextRange tr = new TextRange(Document.ContentStart, Document.ContentEnd);

            string CurrentText = tr.Text;

            using (MemoryStream ms = new MemoryStream(CurrentText.Length * 2))
            {
                tr.Save(ms, DataFormats.Rtf);
                string rtfText = ASCIIEncoding.Default.GetString(ms.ToArray());
                SetRtfText(rtfText);
            }
        }

        void SetRtfText(string value)
        {
            IsSettingText = true;
            try
            {
                RtfText = value;
            }
            finally
            {
                IsSettingText = false;
            }
        }
    }
}
