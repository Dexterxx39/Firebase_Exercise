using System;
using Google.Cloud.Firestore;

namespace Firebase_Exercise.Models;

public class StudentsModel
{
    [FirestoreProperty]
    public string Id { get; set; }

    [FirestoreProperty]
    public string Code { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

}
