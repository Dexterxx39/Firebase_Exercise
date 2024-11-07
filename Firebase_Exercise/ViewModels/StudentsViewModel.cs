using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Firebase_Exercise.Models;
using Firebase_Exercise.Services;
using PropertyChanged;

namespace Firebase_Exercise.ViewModels;

[AddINotifyPropertyChangedInterface]
public class StudentsViewModel
{
    FirestoreService _firestoreService;

    public ObservableCollection<StudentsModel> Students { get; set; } = [];
    public StudentsModel CurrentStudent { get; set; }

    public ICommand Reset { get; set; }
    public ICommand AddOrUpdateCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public StudentsViewModel(FirestoreService firestoreService)
    {
        this._firestoreService = firestoreService;
        this.Refresh();
        Reset = new Command(async () =>
        {
            CurrentStudent = new StudentsModel();
            await this.Refresh();
        }
        );
        AddOrUpdateCommand = new Command(async () =>
        {
            await this.Save();
            await this.Refresh();
        });
        DeleteCommand = new Command(async () =>
        {
            await this.Delete();
            await this.Refresh();
        });

    }
    public async Task GetAll()
    {
        Students = [];
        var items = await _firestoreService.GetAllStudent();
        foreach (var item in items)
        {
            Students.Add(item);
        }
    }

    public async Task Save()
    {
        if (string.IsNullOrEmpty(CurrentStudent.Id))
        {
            await _firestoreService.InsertStudent(this.CurrentStudent);
        }
        else
        {
            await _firestoreService.UpdateStudent(this.CurrentStudent);
        }
    }

    private async Task Refresh()
    {
        CurrentStudent = new StudentsModel();
        await this.GetAll();
    }

    private async Task Delete()
    {
        await _firestoreService.DeleteStudent(this.CurrentStudent.Id);
    }

}