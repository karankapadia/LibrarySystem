using System.Collections.Generic;
using System.IO;
using LibrarySystem.Models;
using Newtonsoft.Json;

namespace LibrarySystem.Services;

public class BooksService : IBooksService
{
    private readonly string _dataFilePath;

    public BooksService(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }

    public List<Book> GetAllBooks()
    {
        string jsonData = File.ReadAllText(_dataFilePath);
        return JsonConvert.DeserializeObject<List<Book>>(jsonData);
    }

    public Book GetBookById(int id)
    {
        List<Book> books = GetAllBooks();
        return books.Find(book => book.Id == id);
    }

    public void AddBook(Book book)
    {
        List<Book> books = GetAllBooks();
        int maxId = books.Any() ? books.Max(b => b.Id) : 0;
        book.Id = maxId + 1;
        books.Add(book);
        SaveBooks(books);
    }

    public void UpdateBook(Book book)
    {
        List<Book> books = GetAllBooks();
        int index = books.FindIndex(b => b.Id == book.Id);
        if (index != -1)
        {
            books[index] = book;
            SaveBooks(books);
        }
    }

    public void DeleteBook(int id)
    {
        List<Book> books = GetAllBooks();
        books.RemoveAll(book => book.Id == id);
        SaveBooks(books);
    }

    private void SaveBooks(List<Book> books)
    {
        string jsonData = JsonConvert.SerializeObject(books, Formatting.Indented);
        File.WriteAllText(_dataFilePath, jsonData);
    }
}