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

namespace Beadando_2024
{
    /// <summary>
    /// Interaction logic for TaskDetailsWindow.xaml
    /// </summary>
    public partial class TaskDetailsWindow : Window
    {
        public TaskDetailsWindow(TaskItem task)
        {
            InitializeComponent();

            TaskNameTextBlock.Text = task.Name;
            CategoryTextBlock.Text = task.CaterogryId.ToString();
            PriorityTextBlock.Text = task.PriorityLevel.ToString();
            StartTimeTextBlock.Text = task.StartTime.ToString("yyyy.MM.dd HH:mm");
            EndTimeTextBlock.Text = task.EndTime.ToString("yyyy.MM.dd HH:mm");
        }
    }
}
