using JobClient.Models;
using System.Net.Http.Json;

namespace JobClient.Services;

public class JobService
{
    private readonly HttpClient _http;

    public JobService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Guid?> StartJobAsync(string jobType, string jobName)
    {
        var response = await _http.PostAsJsonAsync("jobs/start-job", new { JobType = jobType, JobName = jobName });
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
        return result?["jobId"];
    }

    public async Task<string?> GetJobStatusAsync(Guid jobId)
    {
        var response = await _http.GetAsync($"Jobs/job-status/{jobId}");
        if (!response.IsSuccessStatusCode) return null;

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return result?["status"];
    }

    public async Task<bool> CancelJobAsync(Guid jobId)
    {
        var response = await _http.PostAsJsonAsync("Jobs/cancel-job", new { JobId = jobId });
        return response.IsSuccessStatusCode;
    }

    public async Task<List<JobDto>> GetAllJobsAsync()
    {
        var response = await _http.GetAsync("Jobs/jobs");
        if (!response.IsSuccessStatusCode) return new();

        var jobs = await response.Content.ReadFromJsonAsync<List<JobDto>>();
        return jobs ?? new();
    }
}
