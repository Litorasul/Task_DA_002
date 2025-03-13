using API_Template.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API_Template.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly Mock<DbSet<Person>> _dbSetMock;
        private readonly Repository<Person> _repository;

        public RepositoryTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _dbSetMock = new Mock<DbSet<Person>>();
            _dbContextMock.Setup(m => m.Set<Person>()).Returns(_dbSetMock.Object);
            _repository = new Repository<Person>(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var person = new Person { Id = 1, Name = "John Doe", Address = new Address { Street = "123 Main St", City = "Anytown" } };
            _dbSetMock.Setup(m => m.FindAsync(1)).ReturnsAsync(person);

            var result = await _repository.GetByIdAsync(1);

            Assert.Equal(person, result);
        }

        [Fact]
        public async Task AddAsync_AddsEntity()
        {
            var person = new Person { Id = 1, Name = "John Doe", Address = new Address { Street = "123 Main St", City = "Anytown" } };

            await _repository.AddAsync(person);

            _dbSetMock.Verify(m => m.AddAsync(person, It.IsAny<CancellationToken>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntity()
        {
            var person = new Person { Id = 1, Name = "John Doe", Address = new Address { Street = "123 Main St", City = "Anytown" } };

            await _repository.UpdateAsync(person);

            _dbSetMock.Verify(m => m.Update(person), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntity()
        {
            var person = new Person { Id = 1, Name = "John Doe", Address = new Address { Street = "123 Main St", City = "Anytown" } };
            _dbSetMock.Setup(m => m.FindAsync(1)).ReturnsAsync(person);

            await _repository.DeleteAsync(1);

            _dbSetMock.Verify(m => m.Remove(person), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
