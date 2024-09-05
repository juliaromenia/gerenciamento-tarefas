using gerenciamento_tarefas.Context;
using gerenciamento_tarefas.Models;
using Microsoft.AspNetCore.Mvc;

namespace gerenciamento_tarefas.Controllers;

[ApiController]
[Route("[controller]")]
public class TarefasController : ControllerBase
{
    private readonly OrganizadorContext _context;

    public TarefasController(OrganizadorContext context)
    {
        _context = context;
    }

    [HttpGet("buscar/{id}")]
    public IActionResult ObterPorId(int id)
    {
        // TODO: Buscar o Id no banco utilizando o EF
        var result = _context.Tarefas.Find(id);
        // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
        if (result == null) 
        { 
            return NotFound(); 
        }
        // caso contrário retornar OK com a tarefa encontrada
        return Ok(result);
    }

    [HttpGet("ObterTodos")]
    public IActionResult ObterTodos()
    {
        // TODO: Buscar todas as tarefas no banco utilizando o EF
        var result = _context.Tarefas.ToList();
        return Ok(result);
    }

    [HttpGet("ObterPorTitulo")]
    public IActionResult ObterPorTitulo(string titulo)
    {
        // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
        var result = _context.Tarefas.Where(x => x.Titulo == titulo).ToList();
        return Ok(result);
    }

    [HttpGet("ObterPorData")]
    public IActionResult ObterPorData(DateTime data)
    {
        var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
        return Ok(tarefa);
    }

    [HttpGet("ObterPorStatus")]
    public IActionResult ObterPorStatus(EnumStatusTarefa status)
    {
        // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
        var tarefa = _context.Tarefas.Where(x => x.Status == status);
        return Ok(tarefa);
    }

    [HttpPost("criar")]
    public IActionResult Criar(Tarefa tarefa)
    {
        if (tarefa.Data == DateTime.MinValue)
            return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });


        if (!Enum.IsDefined(typeof(EnumStatusTarefa), tarefa.Status))
            return BadRequest(new { Erro = "O status deve ser 0 (Pendente) ou 1 (Finalizado)." });

        // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
        _context.Tarefas.Add(tarefa);
        _context.SaveChanges();

        return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("atualizar/{id}")]
    public IActionResult Atualizar(int id, Tarefa tarefa)
    {
        var tarefaBanco = _context.Tarefas.Find(id);

        if (tarefaBanco == null)
            return NotFound(new { Erro = "Tarefa não encontrada" });

        if (tarefa.Data == DateTime.MinValue)
            return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

        if (tarefa.Status != default(EnumStatusTarefa) && !Enum.IsDefined(typeof(EnumStatusTarefa), tarefa.Status))
            return BadRequest(new { Erro = "Status inválido. O status deve ser 0 (Pendente) ou 1 (Finalizado)." });

        // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
        // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
        if (tarefa.Titulo != null)
            tarefaBanco.Titulo = tarefa.Titulo;

        if (tarefa.Descricao != null)
            tarefaBanco.Descricao = tarefa.Descricao;

        if (tarefa.Data != default(DateTime))
            tarefaBanco.Data = tarefa.Data;

        if (tarefa.Status != default(EnumStatusTarefa))
            tarefaBanco.Status = tarefa.Status;

        _context.SaveChanges();

        return Ok(tarefaBanco);
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Deletar(int id)
    {
        var tarefaBanco = _context.Tarefas.Find(id);

        if (tarefaBanco == null)
            return NotFound();

        // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)

        _context.Tarefas.Remove(tarefaBanco);
        _context.SaveChanges();

        return NoContent();
    }
}
