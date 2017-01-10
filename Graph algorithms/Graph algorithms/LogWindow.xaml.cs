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
using System.Windows.Shapes;

namespace Graph_algorithms
{
    public partial class LogWindow : Window
    {
        public LogWindow(bool enabled)
        {
            InitializeComponent();
            Enabled = enabled;
        }
        public bool Enabled { get; set; }
        public void Clear()
        {
            textBlock.Text = string.Empty;
            sw.ScrollToTop();
        }
        public void Add(string s, bool newLine)
        {
            if (!Enabled) return;
            textBlock.Text += (newLine) ? s + "\n" : s;
            sw.ScrollToBottom();
        }
        async public Task<bool>Add(string s)
        {
            if (!Enabled) return true;
            textBlock.Text += s + "\n";
            await Task.Delay(1000);
            sw.ScrollToBottom();
            return true;
        }
        
    }
}
