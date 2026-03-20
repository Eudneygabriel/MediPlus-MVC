using Microsoft.EntityFrameworkCore;
using MediPlusApp.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

// A configuração de data tem de vir DEPOIS de todos os 'using' 
// e ANTES do 'var builder'
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configuração da ligação ao PostgreSQL
builder.Services.AddDbContext<MediPlusContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Bloco para criar dados automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MediPlusContext>();
    
    // Garante que a base de dados existe
    context.Database.EnsureCreated();

    // 1. Criar Especialidades
    if (!context.Especialidade.Any())
    {
        context.Especialidade.AddRange(
            new Especialidade { EspecialidadeId = 1, Nome = "Pediatria" },
            new Especialidade { EspecialidadeId = 2, Nome = "Cardiologia" },
            new Especialidade { EspecialidadeId = 3, Nome = "Ginecologia" },
            new Especialidade { EspecialidadeId = 4, Nome = "Ortopedia" },
            new Especialidade { EspecialidadeId = 5, Nome = "Medicina Geral e Familiar" },
            new Especialidade { EspecialidadeId = 6, Nome = "Dermatologia" },
            new Especialidade { EspecialidadeId = 7, Nome = "Psiquiatria" },
            new Especialidade { EspecialidadeId = 8, Nome = "Obstetrícia" }
        );
        context.SaveChanges();
    }

    // 2. Criar Médicos
    if (!context.Medico.Any())
    {
        context.Medico.AddRange(
            new Medico { 
                Nome = "Dra. Carolina Cardoso", 
                Cedula = "CP-77123", 
                Email = "carolina.cardoso@mediplus.pt", 
                EspecialidadeId = 5,
                Bio = "Especialista em Pediatria e Medicina Geral." 
            },
            new Medico { 
                Nome = "Dr. Eudney Gabriel", 
                Cedula = "CP-88234", 
                Email = "eudney.gabriel@mediplus.pt", 
                EspecialidadeId = 2,
                Bio = "Especialista em Cardiologia e Psiquiatria."
            },
            new Medico { 
                Nome = "Dra. Lara Lourenço", 
                Cedula = "CP-99345", 
                Email = "lara.lourenco@mediplus.pt", 
                EspecialidadeId = 3,
                Bio = "Especialista em Ginecologia e Obstetrícia."
            },
            new Medico { 
                Nome = "Dr. Miguel Correia", 
                Cedula = "CP-66456", 
                Email = "miguel.correia@mediplus.pt", 
                EspecialidadeId = 4,
                Bio = "Especialista em Ortopedia e Dermatologia."
            }
        );
        context.SaveChanges();
    }

    // 3. Criar Pacientes
    if (!context.Paciente.Any())
    {
        context.Paciente.AddRange(
            new Paciente { Nome = "Carolina Silva", Email = "carolina@email.com", Telemovel = "912345678", SNS = "111222333" },
            new Paciente { Nome = "Gabriel Sousa", Email = "gabriel@email.com", Telemovel = "922333444", SNS = "444555666" },
            new Paciente { Nome = "Lara Ferreira", Email = "lara@email.com", Telemovel = "933444555", SNS = "777888999" },
            new Paciente { Nome = "Miguel Oliveira", Email = "miguel@email.com", Telemovel = "966777888", SNS = "000111222" }
        );
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Classe de Contexto (Base de Dados)
public class MediPlusContext : DbContext
{
    public MediPlusContext(DbContextOptions<MediPlusContext> options) : base(options) { }
    public DbSet<Marcacao> Marcacao { get; set; } = null!;
    public DbSet<Medico> Medico { get; set; } = null!;
    public DbSet<Paciente> Paciente { get; set; } = null!;
    public DbSet<Especialidade> Especialidade { get; set; } = null!;
}