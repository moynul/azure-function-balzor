using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Student.Domain.DataContext;
using Student.Domain.Pagination;
using Student.Services.Implementation;
using Student.Services.Interface;

string sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
        .ConfigureServices(s =>
        {
            s.AddDbContext<StudentDataContext>(options => options.UseSqlServer(sqlConnectionString));
            s.AddScoped<IStudentService, StudentService>();
            s.AddHttpClient();
            s.AddHttpContextAccessor();
            s.AddSingleton<IUriService>(o =>
            {
                var uri = "http://localhost:7298/api/";
                return new UriService(uri);
            });
        })
    .Build();

host.Run();
