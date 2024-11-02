using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Beadando_2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TaskDbContext _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new TaskDbContext();
            _context.Database.EnsureCreated();
            LoadTasks();
        }

        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            string newTask = TaskTextBox.Text;
            if (!string.IsNullOrWhiteSpace(newTask))
            {
                var task = new TaskItem { Name = newTask };
                _context.Tasks.Add(task);
                _context.SaveChanges();
                LoadTasks();
                TaskTextBox.Clear();
            }
        }

        private void editTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                TaskTextBox.Text = selectedTask.Name;
                _context.Tasks.Remove(selectedTask);
                _context.SaveChanges();
                LoadTasks();
            }
        }

        private void loadTask_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            TaskListBox.Items.Clear();
            foreach (var task in _context.Tasks.ToList())
            {
                TaskListBox.Items.Add(task);
            }
        }

        private void deleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                _context.Tasks.Remove(selectedTask);
                _context.SaveChanges();
                LoadTasks();
            }
        }
    }
}