using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Datos;
using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
        public ActionResult Index()
        {
            Data_Books dao = new Data_Books();
            List<Ent_Books> books = dao.GetBooks();
            return View("Index", books);
        }

        public ActionResult GoAdd()
        {
            return View("ViewAddBook");
        }

        public ActionResult FormAddBook(Ent_Books book)
        {
            // Validation for empty or null fields
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                TempData["mensaje"] = "Title is required and cannot be empty.";
                return RedirectToAction("GoAdd");
            }

            if (string.IsNullOrWhiteSpace(book.Autor))
            {
                TempData["mensaje"] = "Author is required and cannot be empty.";
                return RedirectToAction("GoAdd");
            }

            if (string.IsNullOrWhiteSpace(book.Genre))
            {
                TempData["mensaje"] = "Genre is required and cannot be empty.";
                return RedirectToAction("GoAdd");
            }

            // Validation for copies (must be positive)
            if (book.Copies <= 0)
            {
                TempData["mensaje"] = "Number of copies must be greater than zero.";
                return RedirectToAction("GoAdd");
            }

            Data_Books datos = new Data_Books();
            datos.AddBook(book);
            TempData["mensaje"] = $"The book '{book.Title}' was added successfully.";
            return RedirectToAction("Index");
        }

        public ActionResult GoDelete(int idBook)
        {
            // Validation for valid ID
            if (idBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("Index");
            }

            Data_Books dao = new Data_Books();
            Ent_Books book = dao.GetBookByID(idBook);

            // Check if book exists
            if (book == null || book.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("Index");
            }

            return View("ViewDeleteBook", book);
        }

        public ActionResult FormDeleteBook(int idBook)
        {
            // Validation for valid ID
            if (idBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("Index");
            }

            Data_Books dao = new Data_Books();
            Ent_Books book = dao.GetBookByID(idBook);

            // Check if book exists
            if (book == null || book.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("Index");
            }

            TempData["mensaje"] = $"The book '{book.Title}' was deleted.";
            dao.DeleteBook(idBook);
            return RedirectToAction("Index");
        }

        public ActionResult GoLoan()
        {
            return View("ViewBorrowBook");
        }

        public ActionResult FormGoLoan(int idBook, int numberCopies)
        {
            // Validation for valid book ID
            if (idBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("GoLoan");
            }

            // Validation for number of copies to borrow
            if (numberCopies <= 0)
            {
                TempData["mensaje"] = "Number of copies to borrow must be greater than zero.";
                return RedirectToAction("GoLoan");
            }

            Data_Books dao = new Data_Books();
            Ent_Books book = dao.GetBookByID(idBook);

            // Check if book exists
            if (book == null || book.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("GoLoan");
            }

            // Check if there are enough copies available
            if (book.Copies < numberCopies)
            {
                TempData["mensaje"] = $"Not enough copies available. Only {book.Copies} copies in stock.";
                return RedirectToAction("GoLoan");
            }

            dao.BorrowBooks(idBook, numberCopies, book.Copies);
            TempData["mensaje"] = $"{numberCopies} copies of the book '{book.Title}' were borrowed.";
            return RedirectToAction("Index");
        }

        public ActionResult GoReturn()
        {
            return View("ViewReturnBook");
        }

        public ActionResult FormGoReturn(int idBook, int returnedCopies)
        {
            // Validation for valid book ID
            if (idBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("GoReturn");
            }

            // Validation for number of copies to return
            if (returnedCopies <= 0)
            {
                TempData["mensaje"] = "Number of copies to return must be greater than zero.";
                return RedirectToAction("GoReturn");
            }

            Data_Books dao = new Data_Books();
            Ent_Books book = dao.GetBookByID(idBook);

            // Check if book exists
            if (book == null || book.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("GoReturn");
            }

            dao.ReturnBooks(idBook, returnedCopies, book.Copies);
            TempData["mensaje"] = $"{returnedCopies} copies of the book '{book.Title}' were returned.";
            return RedirectToAction("Index");
        }

        public ActionResult GoUpdate(int idBook)
        {
            // Validation for valid ID
            if (idBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("Index");
            }

            Data_Books dao = new Data_Books();
            Ent_Books book = dao.GetBookByID(idBook);

            // Check if book exists
            if (book == null || book.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("Index");
            }

            return View("ViewUpdateBook", book);
        }

        public ActionResult FormUpdateBook(Ent_Books book)
        {
            // Validation for valid book ID
            if (book.IdBook <= 0)
            {
                TempData["mensaje"] = "Invalid book ID.";
                return RedirectToAction("Index");
            }

            // Validation for empty or null fields
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                TempData["mensaje"] = "Title is required and cannot be empty.";
                return RedirectToAction("GoUpdate", new { idBook = book.IdBook });
            }

            if (string.IsNullOrWhiteSpace(book.Autor))
            {
                TempData["mensaje"] = "Author is required and cannot be empty.";
                return RedirectToAction("GoUpdate", new { idBook = book.IdBook });
            }

            if (string.IsNullOrWhiteSpace(book.Genre))
            {
                TempData["mensaje"] = "Genre is required and cannot be empty.";
                return RedirectToAction("GoUpdate", new { idBook = book.IdBook });
            }

            // Validation for copies (must be positive)
            if (book.Copies < 0)
            {
                TempData["mensaje"] = "Number of copies cannot be negative.";
                return RedirectToAction("GoUpdate", new { idBook = book.IdBook });
            }

            Data_Books dao = new Data_Books();

            // Check if book exists before updating
            Ent_Books existingBook = dao.GetBookByID(book.IdBook);
            if (existingBook == null || existingBook.IdBook == 0)
            {
                TempData["mensaje"] = "Book not found.";
                return RedirectToAction("Index");
            }

            dao.UpdateBook(book);
            TempData["mensaje"] = $"The book '{book.Title}' was updated successfully.";
            return RedirectToAction("Index");
        }
    }
}