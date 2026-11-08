using ECMA.Web.Services;
using Xunit;

namespace ECMA.Domain.Tests;

public sealed class VietnamLocationCatalogTests
{
    private static VietnamLocationCatalog Catalog()
        => new(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../src/ECMA.Web")));

    [Fact]
    public void Catalog_HasCurrent34ProvincesAnd3321Wards()
    {
        var catalog = Catalog();

        Assert.Equal(34, catalog.Provinces.Count);
        Assert.Equal(3321, catalog.Provinces.Sum(x => x.Wards.Count));
    }

    [Fact]
    public void TryCompose_RequiresWardToBelongToProvince_AndBuildsCanonicalLocation()
    {
        var catalog = Catalog();
        var province = catalog.Provinces.Single(x => x.Name == "Thành phố Hà Nội");
        var ward = province.Wards[0];
        var otherWard = catalog.Provinces.Single(x => x.Name == "Tỉnh Cà Mau").Wards[0];

        Assert.True(catalog.TryCompose("  12 Tràng Tiền  ", province.Code, ward.Code, out var location));
        Assert.Equal($"12 Tràng Tiền, {ward.Name}, {province.Name}", location);
        Assert.False(catalog.TryCompose("12 Tràng Tiền", province.Code, otherWard.Code, out _));
    }

    [Fact]
    public void TryCompose_CountsUnicodeScalars_AndParsePreservesLegacyLocation()
    {
        var catalog = Catalog();
        var province = catalog.Provinces[0];
        var ward = province.Wards[0];
        var suffixLength = $", {ward.Name}, {province.Name}".EnumerateRunes().Count();

        Assert.True(catalog.TryCompose(new string('a', 200 - suffixLength), province.Code, ward.Code, out var location));
        Assert.Equal(200, location.EnumerateRunes().Count());
        Assert.False(catalog.TryCompose(new string('a', 201 - suffixLength), province.Code, ward.Code, out _));
        Assert.Equal(("Địa điểm cũ", "", ""), catalog.Parse("Địa điểm cũ"));
    }
}
