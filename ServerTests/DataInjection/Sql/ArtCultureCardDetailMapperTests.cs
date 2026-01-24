using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ArtCultureCardDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new ArtCultureCardDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.OfficialName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MapToEntity_MapsCollectionsAndNestedData()
        {
            var mapper = new ArtCultureCardDetailMapper();
            var identifier = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            var dto = new ArtCultureNatureDetailDto
            {
                Identifier = identifier.ToString(),
                OfficialName = " Name ",
                PrimaryImagePath = "img.png",
                FullAddress = " Address ",
                Type = "Type",
                SubjectDiscipline = "Sub",
                Description = " Desc ",
                Email = "mail",
                Telephone = "phone",
                Website = "site",
                Instagram = "insta",
                Facebook = "fb",
                Latitude = 1.2,
                Longitude = 3.4,
                Services = new List<CulturalSiteServiceDto> { new() { Name = "Service", Description = "Desc" } },
                CulturalProjects = new List<CulturalProjectDto> { new() { Name = "Project", Url = "url" } },
                Catalogues = new List<CatalogueDto> { new() { Name = "Cat", WebsiteUrl = "web", Description = "desc" } },
                CreativeWorks = new List<CreativeWorkMobileDto> { new() { Type = "Work", Url = "link" } },
                Gallery = new List<string> { "img1", null! },
                VirtualTours = new List<string> { "tour" },
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = "Park", Distance = 3 },
                Site = new SiteCardDto { Identifier = Guid.Empty.ToString(), OfficialName = "Site", ImagePath = "site.png", Category = "Cat" },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" },
                Neighbors = new List<FeatureCardDto>
                {
                    new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services }
                },
                AssociatedServices = new List<AssociatedServiceDto>
                {
                    new() { Identifier = Guid.Empty.ToString(), Name = "Assoc", ImagePath = "assoc.png" }
                }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.OfficialName, Is.EqualTo(" Name "));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(2));
            NUnitAssert.That(result.Services, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Services.First().Name, Is.EqualTo("Service"));
            NUnitAssert.That(result.NearestCarPark, Is.Not.Null);
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices.First().Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Site, Is.Not.Null);
            NUnitAssert.That(result.Site!.Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
        }
    }
}
