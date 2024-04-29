using BookStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookStoreContext>();
WebApplication app = builder.Build();

app.MapGet("/", () => "Created by Timur");
app.MapGet("/secret", () => "You find a secret!");
app.MapGet("/books", GetBooks);
app.MapGet("/book", GetBook);

app.MapPost("/book", AddBook);
app.MapPut("/book", EditBook);
app.MapDelete("/book/{id}", DeleteBook);

app.Run();

IResult GetBooks(BookStoreContext db)
{
    return Results.Ok(db.Books.ToList());
}

IResult GetBook(BookStoreContext db, int id)
{
    Book? book = db.Books.Find(id);
    if (book == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(book);
    }
}

IResult AddBook(BookStoreContext db, Book book)
{
    db.Add(book);
    db.SaveChanges();
    return Results.Ok();
}

IResult EditBook(BookStoreContext db, Book book)
{
    if (book == null)
    {
        return Results.BadRequest();
    }
    Book? editingBook = db.Books.Find(book.Id);
    if (editingBook == null)
    {
        return Results.NotFound();
    }
    editingBook.Title = book.Title;
    editingBook.Author = book.Author;
    editingBook.PagesCount = book.PagesCount;
    editingBook.Price = book.Price;
    db.SaveChanges();
    return Results.Ok();
}

IResult DeleteBook(BookStoreContext db, int id)
{
    Book? book = db.Books.Find(id);
    if (book == null)
    {
        return Results.BadRequest();
    }
    else
    {
        db.Books.Remove(book);
        db.SaveChanges();
        return Results.Ok();
    }
}