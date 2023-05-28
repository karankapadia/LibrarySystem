using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarySystem.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Services;

namespace LibrarySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase 
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public ActionResult<List<Book>> GetAllBooks()
    {
        return _booksService.GetAllBooks();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBookById(int id)
    {
        Book book = _booksService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return book;
    }

    [HttpPost]
    public IActionResult AddBook(Book book)
    {
        _booksService.AddBook(book);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _booksService.UpdateBook(book);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _booksService.GetBookById(id);
        if (book == null) 
        {
            return NotFound();
        }
        _booksService.DeleteBook(id);
        return NoContent();
    }
}