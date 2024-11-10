using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private TaskDbContext _context;
        private TaskItem _task;
        public AddTaskWindow(TaskDbContext context, TaskItem task = null)
        {
            InitializeComponent();
            _context = context;

            if (task != null)
            {
                _task = task;
                TaskNameTextBox.Text = _task.Name;
                CategoryComboBox.SelectedValue = _task.CaterogryId;
                PriorityComboBox.SelectedItem = _task.PriorityLevel;
                StartDatePicker.SelectedDate = _task.StartTime.Date;
                StartTimeComboBox.SelectedItem = _task.StartTime.ToString("HH:mm");
                EndDatePicker.SelectedDate = _task.EndTime.Date;
                EndTimeComboBox.SelectedItem = _task.EndTime.ToString("HH:mm");
            }

            else
            {
                _task = new TaskItem();
            }

            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(new List<Category>
                {
                    new Category { Name = "Munka" },
                    new Category { Name = "Személyes" },
                    new Category { Name = "Tanulás" }
                });
                _context.SaveChanges();
            }

            LoadCategories();

            for (int hour = 8; hour <= 23; hour++)
            {
                for (int min = 0; min < 60; min += 15)
                {
                    string time = $"{hour:D2}:{min:D2}";
                    StartTimeComboBox.Items.Add(time);
                    EndTimeComboBox.Items.Add(time);
                }
            }
        }
        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource = _context.Categories.ToList();
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
        }
        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            string newTask = TaskNameTextBox.Text;

            if (!string.IsNullOrWhiteSpace(newTask) && PriorityComboBox.SelectedItem != null && CategoryComboBox.SelectedValue != null)
            {
                var selectedPriority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                PriorityLevel priorityLevel = Enum.Parse<PriorityLevel>(selectedPriority);

                if (StartTimeComboBox.SelectedItem == null || EndTimeComboBox.SelectedItem == null ||
                    !StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Kérjük, válassza ki a kezdési és befejezési időpontot!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedStartTime = StartTimeComboBox.SelectedItem.ToString().Split(':');
                var selectedEndTime = EndTimeComboBox.SelectedItem.ToString().Split(':');

                int startHour = int.Parse(selectedStartTime[0]);
                int startMin = int.Parse(selectedStartTime[1]);
                int endHour = int.Parse(selectedEndTime[0]);
                int endMin = int.Parse(selectedEndTime[1]);

                if (_task != null)
                {
                    _task.Name = newTask;
                    _task.CaterogryId = (int)CategoryComboBox.SelectedValue;
                    _task.PriorityLevel = priorityLevel;
                    _task.StartTime = StartDatePicker.SelectedDate.Value
                        .AddHours(startHour)
                        .AddMinutes(startMin);
                    _task.EndTime = EndDatePicker.SelectedDate.Value
                        .AddHours(endHour)
                        .AddMinutes(endMin);

                    _context.Tasks.Update(_task);
                }
                else
                {
                    var task = new TaskItem
                    {
                        Name = newTask,
                        CaterogryId = (int)CategoryComboBox.SelectedValue,
                        PriorityLevel = priorityLevel,
                        StartTime = StartDatePicker.SelectedDate.Value
                            .AddHours(startHour)
                            .AddMinutes(startMin),
                        EndTime = EndDatePicker.SelectedDate.Value
                            .AddHours(endHour)
                            .AddMinutes(endMin)
                    };

                    _context.Tasks.Add(task);
                }

                _context.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kérjük, töltsön ki minden szükséges mezőt!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
