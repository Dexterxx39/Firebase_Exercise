using Firebase_Exercise.Services;
using Firebase_Exercise.ViewModels;

namespace Firebase_Exercise;

public partial class StudentsPage : ContentPage
{
	public StudentsPage()
	{
		InitializeComponent();
		var firestoreService = new FirestoreService();
		BindingContext = new StudentsViewModel(firestoreService);
	}
}