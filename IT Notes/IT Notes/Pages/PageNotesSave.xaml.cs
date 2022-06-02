using IT_Notes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IT_Notes.Pages
{
    public partial class PageNotesSave : ContentPage
    {
        public Note note = new Note();
        public string fullPath;
        public PageNotesSave(Note _note)
        {
            InitializeComponent();

            if (_note != null)
                note = _note;
            BindingContext = note;
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;
            note.Date = DateTime.UtcNow;
            note.Picture = fullPath;
            if (!string.IsNullOrWhiteSpace(note.Text))
                await App.DataBase.SaveNote(note);
            await Navigation.PopAsync();
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;
            if (note.IDNote == 0) return;
            await App.DataBase.DeleteNote(note);
            await Navigation.PopAsync();
        }

        private async void btnGalery_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                imagePhoto.Source = ImageSource.FromFile(photo.FullPath);
                fullPath = photo.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        private async void btnCamera_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
                imagePhoto.Source = ImageSource.FromFile(photo.FullPath);
                fullPath = photo.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
    }
}