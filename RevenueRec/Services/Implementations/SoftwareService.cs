using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Services.Implementations
{
    public class SoftwareService : ISoftwareService
    {
        private readonly s28786Context _context;

        public SoftwareService(s28786Context context)
        {
            _context = context;
        }

        public async Task<SoftwareSystemResponseDto> CreateSoftwareSystemWithInitVersion(SoftwareSystemRequestDto softwareSystemRequestDto, SoftwareVersionRequestDto softwareVersionRequestDto)
        {
            //check if category exists
            if (!_context.Categories.Any(x => x.IdCategory == softwareSystemRequestDto.IdCategory))
            {
                throw new Exception("Category does not exist");
            }
            SoftwareSystem software = new SoftwareSystem
            {
                Name = softwareSystemRequestDto.Name,
                Description = softwareSystemRequestDto.Description,
                CurrentVersionInfo = softwareSystemRequestDto.CurrentVersionInfo,
                YearlyCost = softwareSystemRequestDto.YearlyCost,
                IdCategory = softwareSystemRequestDto.IdCategory,
                SoftwareVersions = [],
                UpFrontContracts = []
            };

            _context.SoftwareSystems.Add(software);
            await _context.SaveChangesAsync();
            SoftwareVersion version = new SoftwareVersion
            {
                Version = softwareVersionRequestDto.Version,
                ReleaseDate = softwareVersionRequestDto.ReleaseDate,
                Description = softwareVersionRequestDto.Description,
                IdSoftwareSystem = software.IdSoftwareSystem
            };

            _context.SoftwareVersions.Add(version);
            await _context.SaveChangesAsync();

            return new SoftwareSystemResponseDto
            {
                IdSoftwareSystem = software.IdSoftwareSystem,
                Name = software.Name,
                Description = software.Description,
                CurrentVersionInfo = software.CurrentVersionInfo,
                YearlyCost = software.YearlyCost,
                Category = _context.Categories.Find(software.IdCategory).Name,
                SoftwareVersions = software.SoftwareVersions.Select(x => new SoftwareVersionResponseDto
                {
                    IdSoftwareVersion = x.IdSoftwareVersion,
                    Version = x.Version,
                    ReleaseDate = x.ReleaseDate,
                    Description = x.Description
                }).ToList()
            };
        }

        public async Task<SoftwareSystemResponseDto> CreateSoftwareSystem(SoftwareSystemRequestDto softwareSystemRequestDto)
        {
            //check if category exists
            if (!_context.Categories.Any(x => x.IdCategory == softwareSystemRequestDto.IdCategory))
            {
                throw new Exception("Category does not exist");
            }
            SoftwareSystem software = new SoftwareSystem
            {
                Name = softwareSystemRequestDto.Name,
                Description = softwareSystemRequestDto.Description,
                CurrentVersionInfo = softwareSystemRequestDto.CurrentVersionInfo,
                YearlyCost = softwareSystemRequestDto.YearlyCost,
                IdCategory = softwareSystemRequestDto.IdCategory,
                SoftwareVersions = [],
                UpFrontContracts = []
            };

            _context.SoftwareSystems.Add(software);
            await _context.SaveChangesAsync();

            return new SoftwareSystemResponseDto
            {
                IdSoftwareSystem = software.IdSoftwareSystem,
                Name = software.Name,
                Description = software.Description,
                CurrentVersionInfo = software.CurrentVersionInfo,
                YearlyCost = software.YearlyCost,
                Category = _context.Categories.Find(software.IdCategory).Name,
                SoftwareVersions = software.SoftwareVersions.Select(x => new SoftwareVersionResponseDto
                {
                    IdSoftwareVersion = x.IdSoftwareVersion,
                    Version = x.Version,
                    ReleaseDate = x.ReleaseDate,
                    Description = x.Description
                }).ToList()
            };
        }

        public async Task<SoftwareVersionResponseDto> CreateSoftwareVersion(SoftwareVersionRequestDto softwareVersionRequestDto)
        {
            //check if software exists
            if (!_context.SoftwareSystems.Any(x => x.IdSoftwareSystem == softwareVersionRequestDto.IdSoftwareSystem))
            {
                throw new Exception("Software does not exist");
            }

            SoftwareVersion version = new SoftwareVersion
            {
                Version = softwareVersionRequestDto.Version,
                ReleaseDate = softwareVersionRequestDto.ReleaseDate,
                Description = softwareVersionRequestDto.Description,
                IdSoftwareSystem = softwareVersionRequestDto.IdSoftwareSystem
            };

            _context.SoftwareVersions.Add(version);
            await _context.SaveChangesAsync();

            return new SoftwareVersionResponseDto
            {
                IdSoftwareVersion = version.IdSoftwareVersion,
                Version = version.Version,
                ReleaseDate = version.ReleaseDate,
                Description = version.Description,
                IdSoftwareSystem = version.IdSoftwareSystem
            };
        }
    }
}