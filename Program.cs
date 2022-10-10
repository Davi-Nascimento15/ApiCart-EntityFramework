using ApiCart.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<APIContextDB>( option => option.UseInMemoryDatabase("Carrinhos"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/listacarrinhos", (APIContextDB db) => { return Results.Ok(db.Carrinhos.Include(a => a.itens).ToList());}).WithName("listadeCarrinhos");

app.MapGet("/itenscarrinhos", (APIContextDB db, int id) =>
{
    var carrinho = db.Carrinhos.Include(a => a.itens).Where(c => c.Id == id).FirstOrDefault();
    if (carrinho != null)
        return Results.Ok(carrinho.itens);
    else
        return Results.NotFound();
});

app.MapPost("/carrinhoadd/{idcarrinho}", async (APIContextDB db, int idcarrinho, Produto produto)  =>
{
    var Car = db.Carrinhos.Include(a => a.itens).Where(c => c.Id == idcarrinho).FirstOrDefault();
    if (Car == null)
    {
        Carrinho carrinho = new Carrinho();
        carrinho.Id = idcarrinho;
        carrinho.itens.Add(produto);
        db.Carrinhos.Add(carrinho);
        await db.SaveChangesAsync();
    }
    else
    {
        db.produtos.Add(produto);
        Car.itens.Add(produto);
        db.Carrinhos.Update(Car);
        await db.SaveChangesAsync();
    }
    Results.Ok();
});

app.MapDelete("/removeritem/{idcarrinho}/{iditem}", async (APIContextDB db, int idcarrinho, int iditem) => {
    var Car = db.Carrinhos.Include(a => a.itens).Where(c => c.Id == idcarrinho).FirstOrDefault();
    var Prod = db.produtos.Where(p => p.codigo == iditem).FirstOrDefault();
    if (Car != null && Prod != null)
    {
        if (Car.itens.Contains(Prod))
        {
            Car.itens.Remove(Prod);
            db.Carrinhos.Update(Car);
            db.produtos.Remove(Prod);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        else
        {
            return Results.NotFound();
        }
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/removercarrinho/{idcarrinho}", async (APIContextDB db, int idcarrinho) => {
    var Car = db.Carrinhos.Include(a => a.itens).Where(c => c.Id == idcarrinho).FirstOrDefault();
    if (Car != null)
    {
        foreach (Produto i in Car.itens)
        {
            var prod = db.produtos.Where(p => p.codigo == i.codigo).FirstOrDefault();
            if(prod!=null)
            db.produtos.Remove(prod);
        }
        db.Carrinhos.Remove(Car);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapGet("/finalizar/{idcarrinho}", (APIContextDB db, int idcarrinho) =>
{
    var Car = db.Carrinhos.Include(a => a.itens).Where(c => c.Id == idcarrinho).FirstOrDefault();
    if (Car != null)
    {
        int quant = Car.itens.Count();
        double total = Car.itens.Sum(c=>c.valor*c.quantidade);
        return Results.Ok("Quantidade de itens: "+quant+" /Valor Total: "+ total);
    }
    else
    {
        return Results.NotFound();
    }
});

app.Run();
