using LibraryApp.Controllers;
using LibraryApp.Data.Model;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LibraryApp.Test
{
    public class BooksControllerTest
    {
        private readonly IBookService _service;
        private readonly BooksController _controller;
        public BooksControllerTest()
        {
            _service = new BookService();
            _controller = new BooksController(_service);
        }

        [Fact]
        public void GetAllTest()
        {
            var result = _controller.Get();

            //assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;
            Assert.IsType<List<Book>>(list.Value);

            var listBooks = list.Value as List<Book>;
            Assert.Equal(5, listBooks.Count);
        }

        [Theory]
        [InlineData("117366b8-3541-4ac5-8732-860d698e26a2", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c111")]
        public void GetBookByIdTest(string string1, string string2)
        {
            //arrange
            var validGuid = new Guid(string1);
            var invalidGuid = new Guid(string2);

            //act
            var result1 = _controller.Get(validGuid);
            var result2 = _controller.Get(invalidGuid);

            //assert
            Assert.IsType<OkObjectResult>(result1.Result);
            Assert.IsType<NotFoundResult>(result2.Result);

            var bookValid = result1.Result as OkObjectResult;
            Assert.IsType<Book>(bookValid.Value);

            var bookItem = bookValid.Value as Book;
            Assert.Equal(validGuid, bookItem.Id);
            Assert.Equal("Evolutionary Psychology", bookItem.Title);


        }

        [Fact]
        public void AddBookItem()
        {
            //arrange
            var book = new Book()
            {
                Author = "Peter James",
                Description = "Harmony with man and man in harmony is just a description",
                Title = "Man and Harmony",
                Id = Guid.NewGuid()
            };

            //act
            var addItem = _controller.Post(book);


            //assert
            Assert.IsType<CreatedAtActionResult>(addItem);

            var addedBook = addItem as CreatedAtActionResult;
            Assert.IsType<Book>(addedBook.Value);

            var bookItem = addedBook.Value as Book;
            Assert.Equal(book.Author, bookItem.Author);
            Assert.Equal(book.Description, bookItem.Description);
            Assert.Equal(book.Title, bookItem.Title);
            Assert.Equal(book.Id, bookItem.Id);

            //arrange
            var book2 = new Book
            {
                Author = "Ausburg",
                Id = Guid.NewGuid()
            };

            //act
            _controller.ModelState.AddModelError("Title", "Title field is required");
            var badResponse = _controller.Post(book2);

            Assert.IsType<BadRequestObjectResult>(badResponse);

        }
        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c111")]
        public void RemoveBookIdTest(string guid1, string guid2)
        {
            //arrange
            var validGuid = new Guid(guid1);
            var invalidGuid = new Guid(guid2);

            //act
            var notFoundResult = _controller.Remove(invalidGuid);
            //assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.Equal(5, _service.GetAll().Count());

            //act
            var okResult = _controller.Remove(validGuid);
            //assert
            Assert.IsType<OkResult>(okResult);
            Assert.Equal(4, _service.GetAll().Count());
        }
    }
}
