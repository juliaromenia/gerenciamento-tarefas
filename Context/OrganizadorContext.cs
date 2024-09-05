using gerenciamento_tarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace gerenciamento_tarefas.Context;

public class OrganizadorContext : DbContext
{
    public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas {  get; set; }
}