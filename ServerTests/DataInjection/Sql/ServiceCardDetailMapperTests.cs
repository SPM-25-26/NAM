using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using DomainDayOfWeek = Domain.Entities.MunicipalityEntities.DayOfWeek;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ServiceCardDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsDetailWithNewIdentifier_WhenDtoNull()
        {
            var mapper = new ServiceCardDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Name, Is.Null);
            NUnitAssert.That(result.OpeningHours, Is.Null);
        }

        [Test]
        public void MapToEntity_MapsNestedObjectsAndCollections()
        {
            var mapper = new ServiceCardDetailMapper();
            var identifier = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            var dto = new ServiceCardDetailDto
            {
                Identifier = identifier.ToString(),
                Name = " Service ",
                Address = " Address ",
                Description = " Description ",
                Latitude = 45.5,
                Longitude = 9.6,
                Typology = " Typology ",
                PrimaryImage = " Image ",
                Email = " Mail ",
                Telephone = " Phone ",
                Website = " Site ",
                Instagram = " Insta ",
                Facebook = " Face ",
                Price = " Price ",
                ReservationUrl = "https://example.test",
                Gallery = new List<string> { " img1 ", " " },
                NearestCarPark = new NearestCarParkDto
                {
                    Latitude = 1.2,
                    Longitude = 3.4,
                    Address = "Park",
                    Distance = 12.5
                },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto
                {
                    Name = "Milano",
                    LogoPath = "logo.png"
                },
                OpeningHours = new OpeningHoursSpecificationDto
                {
                    Opens = new TimeOnly(8, 0),
                    Closes = new TimeOnly(18, 0),
                    Description = " Hours ",
                    Day = DomainDayOfWeek.Monday,
                    AdmissionType = new AdmissionTypeDto
                    {
                        Name = AdmissionTypeName.Daily,
                        Description = " Admission "
                    },
                    TimeInterval = new TimeIntervalDto
                    {
                        Date = new DateTime(2024, 1, 10),
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2024, 1, 31)
                    }
                },
                TemporaryClosure = new TemporaryClosureDto
                {
                    ReasonForClosure = " Reason ",
                    Opens = new TimeOnly(9, 0),
                    Closes = new TimeOnly(12, 0),
                    Description = " Closure ",
                    Day = DomainDayOfWeek.Tuesday,
                    TimeInterval = new TimeIntervalDto
                    {
                        Date = new DateTime(2024, 2, 1),
                        StartDate = new DateTime(2024, 2, 2),
                        EndDate = new DateTime(2024, 2, 3)
                    }
                },
                Booking = new BookingDto
                {
                    Name = BookingType.Mandatory,
                    Description = " Booking ",
                    TimeIntervalDto = new TimeIntervalDto
                    {
                        Date = new DateTime(2024, 3, 1),
                        StartDate = new DateTime(2024, 3, 2),
                        EndDate = new DateTime(2024, 3, 3)
                    }
                },
                Neighbors = new List<FeatureCardDto?>
                {
                    new()
                    {
                        EntityId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd").ToString(),
                        Title = "Neighbor",
                        Category = MobileCategory.Services,
                        ImagePath = "neighbor.png",
                        ExtraInfo = " Extra "
                    },
                    null
                },
                AssociatedServices = new List<AssociatedServiceDto?>
                {
                    new()
                    {
                        Identifier = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee").ToString(),
                        Name = " Service ",
                        ImagePath = "service.png"
                    },
                    new()
                    {
                        Identifier = Guid.Empty.ToString(),
                        Name = null,
                        ImagePath = null
                    }
                },
                Locations = new List<ServiceLocationDto?>
                {
                    new()
                    {
                        Identifier = "loc1",
                        OfficialName = " Location ",
                        ImagePath = "loc.png",
                        Category = "Cat"
                    }
                }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.Name, Is.EqualTo("Service"));
            NUnitAssert.That(result.Address, Is.EqualTo("Address"));
            NUnitAssert.That(result.Description, Is.EqualTo("Description"));
            NUnitAssert.That(result.Price, Is.EqualTo("Price"));
            NUnitAssert.That(result.OpeningHours, Is.Not.Null);
            NUnitAssert.That(result.OpeningHours!.Description, Is.EqualTo("Hours"));
            NUnitAssert.That(result.OpeningHours.AdmissionType, Is.Not.Null);
            NUnitAssert.That(result.OpeningHours.AdmissionType!.Description, Is.EqualTo("Admission"));
            NUnitAssert.That(result.TemporaryClosure, Is.Not.Null);
            NUnitAssert.That(result.TemporaryClosure!.ReasonForClosure, Is.EqualTo("Reason"));
            NUnitAssert.That(result.Booking, Is.Not.Null);
            NUnitAssert.That(result.Booking!.Description, Is.EqualTo("Booking"));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Gallery.First(), Is.EqualTo("img1"));
            NUnitAssert.That(result.NearestCarPark, Is.Not.Null);
            NUnitAssert.That(result.NearestCarPark!.Address, Is.EqualTo("Park"));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
            NUnitAssert.That(result.MunicipalityData!.Name, Is.EqualTo("Milano"));
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Neighbors.First().FeatureCard!.Title, Is.EqualTo("Neighbor"));
            NUnitAssert.That(result.AssociatedServices, Has.Count.EqualTo(2));
            NUnitAssert.That(result.AssociatedServices.First().Identifier, Is.EqualTo(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee")));
            NUnitAssert.That(result.AssociatedServices.Last().Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Locations, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Locations.First().ServiceLocation!.OfficialName, Is.EqualTo(" Location "));
        }

        [Test]
        public void MapToEntity_UsesFallbackIdentifier_WhenDtoIdentifierEmpty()
        {
            var mapper = new ServiceCardDetailMapper();

            var dto = new ServiceCardDetailDto
            {
                Identifier = Guid.Empty.ToString(),
                Latitude = 1,
                Longitude = 2
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
