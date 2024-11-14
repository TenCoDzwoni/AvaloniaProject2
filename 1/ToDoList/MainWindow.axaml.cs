using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
namespace SurveyApp
{
    public partial class MainWindow : Window
    {
        public class QuestionItem
        {
            public string Question { get; set; }
            public bool IsMultipleChoice { get; set; }
            public ObservableCollection<string> Answers { get; set; } = new ObservableCollection<string>();
            public string Category { get; set; }
            public override string ToString()
            {
                return $"{Question} ({Category})";
            }
        }
        public ObservableCollection<QuestionItem> Questions { get; set; } = new ObservableCollection<QuestionItem>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            var questionText = questionTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(questionText))
            {
                var question = new QuestionItem
                {
                    Question = questionText,
                    IsMultipleChoice = multipleChoiceRadioButton.IsChecked == true,
                    Category = categoryComboBox.SelectedItem as string
                };
                foreach (var answerItem in answerListBox.Items)
                {
                    question.Answers.Add(answerItem.ToString());
                }
                Questions.Add(question);
                
                questionTextBox.Clear();
                answerListBox.Items.Clear();
            }
        }
        private void SaveSurvey(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            var filePath = saveDialog.ShowAsync(this).Result;

            if (!string.IsNullOrEmpty(filePath))
            {
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var question in Questions)
                    {
                        writer.WriteLine($"Pytanie: {question.Question} ({question.Category})");
                        writer.WriteLine($"Typ: {(question.IsMultipleChoice ? "Wielokrotny wybór" : "Jednokrotny wybór")}");
                        writer.WriteLine("Odpowiedzi:");
                        foreach (var answer in question.Answers)
                        {
                            writer.WriteLine($" - {answer}");
                        }
                        writer.WriteLine();
                    }    
                }
            }
        }
        private void AddAnswer(object sender, RoutedEventArgs e)
        {
            var answerText = answerTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(answerText))
            {
                answerListBox.Items.Add(answerText);
                answerTextBox.Clear();
            }
        }
    }
}