using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using StealAllTheCats.Data;
using StealAllTheCats.Entities;
using StealAllTheCats.Models;
using StealAllTheCats.Services;
using Xunit;

public class CatServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CatsDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private HttpClient GetMockHttpClient(List<CatApiModel> apiCats)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(apiCats) // Create a new instance each time
            });

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task FetchCatsAsync_AddsNewCats_AndAvoidsDuplicates()
    {
        var apiCats = new List<CatApiModel>
        {
            new CatApiModel { Id = "abc", Url = "url1", Width = 100, Height = 100, Breeds = new List<CatApiModel.Breed>() },
            new CatApiModel { Id = "def", Url = "url2", Width = 200, Height = 200, Breeds = new List<CatApiModel.Breed>() }
        };
        var httpClient = GetMockHttpClient(apiCats);
        using var dbContext = GetInMemoryDbContext();
        var service = new CatService(httpClient, dbContext);

        var firstAdd = await service.FetchCatsAsync();
        var secondAdd = await service.FetchCatsAsync();

        Assert.Equal(2, await dbContext.Cats.CountAsync());
        Assert.Equal(2, firstAdd.Count);
        Assert.Empty(secondAdd); // No new cats on second fetch
    }

    [Fact]
    public async Task GetCatByCatIdAsync_ReturnsCorrectCat()
    {
        using var dbContext = GetInMemoryDbContext();
        dbContext.Cats.Add(new CatEntity { CatId = "cat123", ImageUrl = "url", Width = 1, Height = 1 });
        dbContext.SaveChanges();
        var service = new CatService(new HttpClient(), dbContext);
        var cat = await service.GetCatByCatIdAsync("cat123");

        Assert.NotNull(cat);
        Assert.Equal("cat123", cat.CatId);
    }

    [Fact]
    public async Task GetCatsAsync_ReturnsPagedCats()
    {
        using var dbContext = GetInMemoryDbContext();
        for (int i = 0; i < 15; i++)
        {
            dbContext.Cats.Add(new CatEntity { CatId = $"cat{i}", ImageUrl = "url", Width = 1, Height = 1 });
        }
        dbContext.SaveChanges();
        var service = new CatService(new HttpClient(), dbContext);

        var catsPage1 = await service.GetCatsAsync(null, 1, 10);
        var catsPage2 = await service.GetCatsAsync(null, 2, 10);

        Assert.Equal(10, catsPage1.Count);
        Assert.Equal(5, catsPage2.Count);
    }
}