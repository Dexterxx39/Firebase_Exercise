using System;
using Firebase_Exercise.Models;
using Google.Cloud.Firestore;

namespace Firebase_Exercise.Services;

public class FirestoreService
{
    private FirestoreDb db;
    public string StatusMessage;

    public FirestoreService()
    {
        this.SetupFireStore();
    }
    private async Task SetupFireStore()
    {
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("fb-exercise-d560f-firebase-adminsdk-jy31t-b76b3df2c7.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
            db = new FirestoreDbBuilder
            {
                ProjectId = "fb-exercise-d560f",

                JsonCredentials = contents
            }.Build();
        }
    }
    public async Task<List<StudentsModel>> GetAllStudent()
    {
        try
        {
            await SetupFireStore();
            var data = await db.Collection("Students").GetSnapshotAsync();
            var samples = data.Documents.Select(doc =>
            {
                var sample = new StudentsModel();
                sample.Id = doc.Id;
                sample.Code = doc.GetValue<string>("Code");
                sample.Name = doc.GetValue<string>("Name");
                return sample;
            }).ToList();
            return samples;
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
        return null;
    }

    public async Task InsertStudent(StudentsModel sample)
    {
        try
        {
            await SetupFireStore();
            var sampleData = new Dictionary<string, object>
            {
                { "Code", sample.Code },
                { "Name", sample.Name }
                // Add more fields as needed
            };

            await db.Collection("Students").AddAsync(sampleData);
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task UpdateStudent(StudentsModel sample)
    {
        try
        {
            await SetupFireStore();

            // Manually create a dictionary for the updated data
            var sampleData = new Dictionary<string, object>
            {
                { "Code", sample.Code },
                { "Name", sample.Name }
                // Add more fields as needed
            };

            // Reference the document by its Id and update it
            var docRef = db.Collection("Students").Document(sample.Id);
            await docRef.SetAsync(sampleData, SetOptions.Overwrite);

            StatusMessage = "Student successfully updated!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    public async Task DeleteStudent(string id)
    {
        try
        {
            await SetupFireStore();

            // Reference the document by its Id and delete it
            var docRef = db.Collection("Students").Document(id);
            await docRef.DeleteAsync();

            StatusMessage = "Student successfully deleted!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

}
