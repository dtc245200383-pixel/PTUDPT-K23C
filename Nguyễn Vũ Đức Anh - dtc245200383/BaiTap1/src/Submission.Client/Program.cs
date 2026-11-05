using System.Net.Http.Json;

var apiBaseUrl = args.Length > 0 ? args[0] : "http://localhost:5000";
using var http = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

var assignments = await http.GetFromJsonAsync<List<Assignment>>("/api/assignments");
Console.WriteLine("Danh sach bai tap:");
foreach (var assignment in assignments ?? new List<Assignment>())
{
    Console.WriteLine($"- {assignment.Id}: {assignment.Title}");
}

var request = new CreateSubmissionRequest(
    AssignmentId: 1,
    StudentCode: "dtc245200383",
    RepositoryUrl: "https://github.com/USERNAME/EduSubmit-dtc245200383");

var response = await http.PostAsJsonAsync("/api/submissions", request);
response.EnsureSuccessStatusCode();
var result = await response.Content.ReadFromJsonAsync<SubmissionResult>();
if (result is null)
{
    throw new InvalidOperationException("API returned an empty submission response.");
}

Console.WriteLine($"Da nop bai. Ma luot nop: {result.Id}");
Console.WriteLine($"Trang thai: {result.Status}");

var lookupResponse = await http.GetAsync($"/api/submissions/{result.Id}");
lookupResponse.EnsureSuccessStatusCode();
var lookedUp = await lookupResponse.Content.ReadFromJsonAsync<SubmissionResult>();
Console.WriteLine($"Tra cuu lai: {lookedUp?.Id}, trang thai: {lookedUp?.Status}");

record Assignment(int Id, string Title, string Description);
record CreateSubmissionRequest(int AssignmentId, string StudentCode, string RepositoryUrl);
record SubmissionResult(Guid Id, int AssignmentId, string StudentCode, string Status, DateTimeOffset ReceivedAt);
