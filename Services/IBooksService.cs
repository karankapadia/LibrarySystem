using System.Collections.Generic;
using LibrarySystem.Models;

namespace LibrarySystem.Services;

public interface IBooksService
{
    List<Book> GetAllBooks();
    Book GetBookById(int bookId);
    void AddBook(Book newBook);
    void UpdateBook(Book updatedBook);
    void DeleteBook(int bookId);
}
