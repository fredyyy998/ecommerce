using Microsoft.AspNetCore.Mvc;
using Offering.Dtos;
using Offering.Models;
using Offering.Repositories;

namespace Offering.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class OfferController : Controller
{
    private readonly IOfferRepository _offerRepository;

    private readonly IProductRepository _productRepository;

    public OfferController(IOfferRepository offerRepository, IProductRepository productRepository)
    {
        _offerRepository = offerRepository;
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListOffers()
    {
        var offerResult = await _offerRepository.FindAll();
        return Ok(offerResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOffer(Guid id)
    {
        var offerResult = await _offerRepository.FindById(id);
        return Ok(offerResult);
    }


    [HttpPost("single")]
    public async Task<IActionResult> CreateSingleOffer([FromBody] CreateSingleOfferRequestDto createSingleOfferRequestDto)
    {
        var price = Price.CreateFromGross(createSingleOfferRequestDto.grossPrice, createSingleOfferRequestDto.taxValue,
            createSingleOfferRequestDto.currency);
        var product = await _productRepository.FindById(createSingleOfferRequestDto.productId);
        var offer = SingleOffer.Create(createSingleOfferRequestDto.name, price, createSingleOfferRequestDto.startDate,
            createSingleOfferRequestDto.endDate, product);

        var createdOffer = await _offerRepository.Add(offer);
        return Ok(createdOffer);
    }
    
    [HttpPost("package")]
    public async Task<IActionResult> CreatePackageOffer([FromBody] CreatePackageOfferRequestDto createPackageOfferRequestDto)
    {
        var price = Price.CreateFromGross(createPackageOfferRequestDto.grossPrice, createPackageOfferRequestDto.taxValue,
            createPackageOfferRequestDto.currency);

        var products = new List<Product>();
        foreach (var productId in createPackageOfferRequestDto.productIds)
        {
            var product = await _productRepository.FindById(productId);
            products.Add(product);
        }
        var offer = PackageOffer.Create(createPackageOfferRequestDto.name, price, createPackageOfferRequestDto.startDate,
            createPackageOfferRequestDto.endDate, products);

        var createdOffer = await _offerRepository.Add(offer);
        return Ok(createdOffer);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOffer(Guid id)
    {
        var offer = await _offerRepository.FindById(id);
        await _offerRepository.Delete(offer);
        return Ok();
    }
}

