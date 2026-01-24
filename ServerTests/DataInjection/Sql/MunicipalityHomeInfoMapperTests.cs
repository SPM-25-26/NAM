using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class MunicipalityHomeInfoMapperTests
    {
        [Test]
        public void MapToEntity_Throws_WhenDtoNull()
        {
            var mapper = new MunicipalityHomeInfoMapper();

            NUnitAssert.Throws<ArgumentNullException>(() => mapper.MapToEntity(null!));
        }

        [Test]
        public void MapToEntity_MapsContactsAndCollections()
        {
            var mapper = new MunicipalityHomeInfoMapper();

            var dto = new MunicipalityHomeInfoDto
            {
                LegalName = " Legal ",
                Name = " Name ",
                Description = " Desc ",
                Contacts = new MunicipalityHomeContactInfoDto
                {
                    Email = " mail ",
                    Telephone = " tel ",
                    Website = " web ",
                    Facebook = " fb ",
                    Instagram = " insta "
                },
                Latitude = 1.2,
                Longitude = 3.4,
                LogoPath = " logo ",
                HomeImages = new List<string> { " img1 ", " " },
                PanoramaPath = " pano ",
                PanoramaWidth = 100,
                VirtualTourUrls = new List<string> { " tour ", " " },
                NameAndProvince = " NameProv "
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.LegalName, Is.EqualTo("Legal"));
            NUnitAssert.That(result.Contacts, Is.Not.Null);
            NUnitAssert.That(result.Contacts!.Email, Is.EqualTo("mail"));
            NUnitAssert.That(result.HomeImages, Has.Count.EqualTo(1));
            NUnitAssert.That(result.VirtualTourUrls, Has.Count.EqualTo(1));
            NUnitAssert.That(result.NameAndProvince, Is.EqualTo("NameProv"));
        }
    }
}
