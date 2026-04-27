using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.IO;

namespace Notepad_avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OpenFile_Click(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open txt document",
                FileTypeFilter = new[] { FilePickerFileTypes.TextPlain },
                AllowMultiple = false
            });
            if (files.Count > 0)
            {
                await using var stream = await files[0].OpenReadAsync();
                using var reader = new StreamReader(stream);
                Editor.Text = await reader.ReadToEndAsync();
            }
        }

        private async void SaveFile_Click(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save txt document",
                DefaultExtension = ".txt",
                FileTypeChoices = new[] { FilePickerFileTypes.TextPlain }
            });

            if (files != null)
            {
                await using var stream = await files.OpenWriteAsync();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(Editor.Text);
            }
        }

        private void CloseFile_Click(object? sender, RoutedEventArgs e)
        {
                Close();
        }
    }
}