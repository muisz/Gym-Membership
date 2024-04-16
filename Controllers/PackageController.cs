using GymMembership.Data;
using GymMembership.Exceptions;
using GymMembership.Models;
using GymMembership.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMembership.Controllers
{
    [Route("/api/v1/packages")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult<ICollection<ListPackageData>>> GetPackages()
        {
            try
            {
                ICollection<ListPackageData> listPackages = new List<ListPackageData>();
                ICollection<Package> packages = await _packageService.GetPackages();
                foreach (Package package in packages)
                {
                    listPackages.Add(new ListPackageData { Id = package.Id, Name = package.Name, Price = package.Price });
                }
                return Ok(listPackages);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PackageData>> GetPackage(int id)
        {
            try
            {
                Package? package = await _packageService.GetPackage(id);
                if (package == null)
                    throw new HttpException("Package not found", StatusCodes.Status404NotFound);
                
                return Ok(new PackageData
                {
                    Id = package.Id,
                    Name = package.Name,
                    Price = package.Price,
                    ActiveDays = package.ActiveDays,
                });
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }
    }
}