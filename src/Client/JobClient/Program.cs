using JobClient.Services;

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri("https://localhost:44382/api/")
};

var jobService = new JobService(httpClient);

while (true)
{
    Console.WriteLine("\n=== MENÚ CLIENTE JOB SERVER ===");
    Console.WriteLine("1. Crear un nuevo job");
    Console.WriteLine("2. Crear lote de 5 jobs");
    Console.WriteLine("3. Cancelar un job");
    Console.WriteLine("4. Ver estado de un job");
    Console.WriteLine("5. Listar todos los jobs");
    Console.WriteLine("0. Salir");
    Console.Write("Selecciona una opción: ");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.Write("JobType: ");
            var type = Console.ReadLine();
            Console.Write("JobName: ");
            var name = Console.ReadLine();
            var jobId = await jobService.StartJobAsync(type!, name!);
            Console.WriteLine(jobId != null ? $"✅ Job creado: {jobId}" : "❌ Error al crear job.");
            break;

        case "2":
            Console.Write("JobType para el lote: ");
            var batchType = Console.ReadLine();
            for (int i = 1; i <= 5; i++)
            {
                var batchId = await jobService.StartJobAsync(batchType!, $"BatchJob-{i}");
                Console.WriteLine(batchId != null ? $"✅ Job {i} creado: {batchId}" : $"❌ Job {i} falló.");
            }
            break;

        case "3":
            Console.Write("ID del job a cancelar: ");
            if (Guid.TryParse(Console.ReadLine(), out var cancelId))
            {
                var result = await jobService.CancelJobAsync(cancelId);
                Console.WriteLine(result ? "✅ Job cancelado." : "⚠️ No se pudo cancelar.");
            }
            else Console.WriteLine("❌ ID inválido.");
            break;

        case "4":
            Console.Write("ID del job: ");
            if (Guid.TryParse(Console.ReadLine(), out var statusId))
            {
                var status = await jobService.GetJobStatusAsync(statusId);
                Console.WriteLine(status != null ? $"📊 Estado: {status}" : "❌ Job no encontrado.");
            }
            else Console.WriteLine("❌ ID inválido.");
            break;

        case "5":
            var jobs = await jobService.GetAllJobsAsync();
            if (jobs.Count == 0)
            {
                Console.WriteLine("📭 No hay jobs registrados.");
            }
            else
            {
                foreach (var job in jobs)
                {
                    Console.WriteLine($"- {job.Id} | {job.JobType} | {job.JobName} | {job.Status}");
                }
            }
            break;

        case "0":
            Console.WriteLine("👋 Saliendo...");
            return;

        default:
            Console.WriteLine("❌ Opción inválida.");
            break;
    }
}
