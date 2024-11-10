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

        private void openAddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow(_context);
            if (addTaskWindow.ShowDialog() == true)
            {
                LoadTasks();
            }
            LoadTasks();
        }

        private void FilterPriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (FilterPriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();


            if (selectedFilter == "All")
            {
                TaskListBox.ItemsSource = _context.Tasks.ToList();
            }
            else
            {
                PriorityLevel priorityLevel = Enum.Parse<PriorityLevel>(selectedFilter);
                TaskListBox.ItemsSource = _context.Tasks.Where(t => t.PriorityLevel == priorityLevel).ToList();
            }
        }


        private void editTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                var editTaskWindow = new AddTaskWindow(_context, selectedTask);
                if (editTaskWindow.ShowDialog() == true)
                {
                    LoadTasks();
                }
            }
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _context.Tasks.ToList();
            TaskListBox.ItemsSource = tasks;

            foreach (var item in tasks)
            {
                var container = TaskListBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (container != null)
                {
                    
                    if (item.EndTime.Date >= DateTime.Now)
                    {
                        item.Deadline = true;
                    }
                    else
                    {
                        switch (item.PriorityLevel)
                        {
                            case PriorityLevel.Low:
                                container.Tag = Brushes.Green;
                                break;
                            case PriorityLevel.Medium:
                                container.Tag = Brushes.Orange;
                                break;
                            case PriorityLevel.High:
                                container.Background = Brushes.Red;
                                break;
                        }
                    }
                    
                }
                
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dueTasks = _context.Tasks
                .Where(t => t.EndTime.Date == DateTime.Now.Date)
                .Select(task => task.Name)
                .ToList();

            if (dueTasks.Any())
            {
                string message = "Reminder: The following tasks are due today:\n" + string.Join("\n", dueTasks);
                MessageBox.Show(message);
            }
        }

        private void detailsTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskListBox.SelectedItem as TaskItem;
            if (selectedTask != null)
            {
                TaskDetailsWindow taskDetailsWindow = new TaskDetailsWindow(selectedTask);
                taskDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy feladatot!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}