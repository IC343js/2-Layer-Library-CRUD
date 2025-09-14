using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary.Models;

namespace WebLibrary.Datos
{
    public class Data_Books
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        public List<Ent_Books> GetBooks()
        {
            List<Ent_Books> books = new List<Ent_Books>();
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "SELECT idBooks, title, autor, genre, copies FROM Books";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Ent_Books book = new Ent_Books();
                book.IdBook = Convert.ToInt32(reader["idBooks"]);
                book.Title = Convert.ToString(reader["title"]);
                book.Autor = Convert.ToString(reader["autor"]);
                book.Genre = Convert.ToString(reader["genre"]);
                book.Copies = Convert.ToInt32(reader["copies"]);

                books.Add(book);
            }
            connection.Close();
            return books;
        }
        public Ent_Books GetBookByID(int IdBook)
        {
            Ent_Books book = new Ent_Books();
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "SELECT idBooks, title, autor, genre, copies FROM Books WHERE idBooks = @idBooks";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("idBooks", IdBook);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                book.IdBook = Convert.ToInt32(reader["idBooks"]);
                book.Title = Convert.ToString(reader["title"]);
                book.Autor = Convert.ToString(reader["autor"]);
                book.Genre = Convert.ToString(reader["genre"]);
                book.Copies = Convert.ToInt32(reader["copies"]);
            }
            connection.Close();
            return book;
        }
        public void AddBook(Ent_Books book)
        {
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "INSERT INTO Books(title, autor, genre, copies) VALUES(@title, @autor, @genre, @copies)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@autor", book.Autor);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@copies", book.Copies);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateBook(Ent_Books book)
        {
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "UPDATE Books SET title=@title, autor=@autor, genre=@genre, copies=@copies WHERE idBooks=@idBooks";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@idBooks", book.IdBook);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@autor", book.Autor);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@copies", book.Copies);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteBook(int idBook)
        {
            SqlConnection connection = new SqlConnection (cadenaConexion);
            connection.Open();
            string query = "DELETE Books WHERE idBooks = @idBooks";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@idBooks", idBook);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void BorrowBooks(int idBook, int numberCopies, int existingCopies)
        {
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "UPDATE Books SET copies=@copies WHERE idBooks=@idBooks";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@copies", existingCopies - numberCopies);
            cmd.Parameters.AddWithValue("@idBooks", idBook);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void ReturnBooks(int idBook, int returnedCopies, int existingCopies)
        {
            SqlConnection connection = new SqlConnection(cadenaConexion);
            connection.Open();
            string query = "UPDATE Books SET copies=@copies WHERE idBooks=@idBooks";
            SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@copies", existingCopies + returnedCopies);
            cmd.Parameters.AddWithValue("@idBooks", idBook);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}