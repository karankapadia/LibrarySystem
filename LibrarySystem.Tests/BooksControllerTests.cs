using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using LibrarySystem.Controllers;
using LibrarySystem.Models;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Tests;

public class BooksControllerTests
{
    private readonly BooksController _controller;
    private readonly Mock<IBooksService> _mockBooksService;

    public BooksControllerTests()
    {
        _mockBooksService = new Mock<IBooksService>();
        _controller = new BooksController(_mockBooksService.Object);
    }

    [Fact]
    public void GetAllBooksTest()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "Author 1", Genre = "Genre 1", PublicationYear = 2021 },
            new Book { Id = 2, Title = "Book 2", Author = "Author 2", Genre = "Genre 2", PublicationYear = 2022 }
        };

        _mockBooksService.Setup(service => service.GetAllBooks()).Returns(books);

        // Act
        var result = _controller.GetAllBooks();

        // Assert
        var okResult = Assert.IsType<ActionResult<List<Book>>>(result);
        var returnedBooks = Assert.IsAssignableFrom<List<Book>>(okResult.Value);
        Assert.Equal(2, returnedBooks.Count());
    }

    [Fact]
    public void GetBookByIdOkTest()
    {
        // Arrange
        int bookId = 1;
        var book = new Book { Id = bookId, Title = "Book 1", Author = "Author 1", Genre = "Genre 1", PublicationYear = 2021 };

        _mockBooksService.Setup(service => service.GetBookById(bookId)).Returns(book);

        // Act
        var result = _controller.GetBookById(bookId);

        // Assert
        var okResult = Assert.IsType<ActionResult<Book>>(result);
        var returnedBook = Assert.IsAssignableFrom<Book>(okResult.Value);
        Assert.Equal(bookId, returnedBook.Id);
    }

    [Fact]
    public void GetBookByIdFailedTest()
    {
        // Arrange
        int bookId = 1;
        _mockBooksService.Setup(service => service.GetBookById(bookId)).Returns((Book)null);

        // Act
        var result = _controller.GetBookById(bookId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void AddBookTest()
    {
        var book = new Book { Title = "New Book", Author = "New Author", Genre = "New Genre", PublicationYear = 2023 };

        _mockBooksService.Setup(service => service.AddBook(book)).Verifiable();

        // Act
        var result = _controller.AddBook(book);

        // Assert
        Assert.NotNull(result);
    } 

     [Fact]
    public void UpdateBookBadRequestTest()
    {
        // Arrange
        int bookId = 3;
        var updatedBook = new Book { Id = 1, Title = "Updated Book", Author = "Author 1", Genre = "Genre 1", PublicationYear = 2021 };

        // Act
        var result = _controller.UpdateBook(bookId, updatedBook);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }  

    [Fact]
    public void UpdateBookTest()
    {
        // Arrange
        int bookId = 1;
        var updatedBook = new Book { Id = bookId, Title = "Updated Book", Author = "Author 1", Genre = "Genre 1", PublicationYear = 2021 };

        // Act
        var result = _controller.UpdateBook(bookId, updatedBook);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void DeleteBookFailedTest()
    {
        // Arrange
        int bookId = 10;

        // Act
        var result = _controller.DeleteBook(bookId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteBookTest()
    {
        // Arrange
        int bookId = 1;
        _mockBooksService.Setup(service => service.GetBookById(bookId)).Returns(new Book());

        // Act
        var result = _controller.DeleteBook(bookId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}

