using System.Text.Json;

namespace ECMA.Web.Services;

public sealed class VietnamLocationCatalog
{
    public sealed record Ward(string Code, string Name);
    public sealed record Province(string Code, string Name, IReadOnlyList<Ward> Wards);

    public IReadOnlyList<Province> Provinces { get; }

    public VietnamLocationCatalog(string contentRootPath)
    {
        using var stream = File.OpenRead(Path.Combine(contentRootPath, "Data", "vietnam-administrative-units.json"));
        Provinces = JsonSerializer.Deserialize<List<Province>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Danh mục địa giới không hợp lệ");
        if (Provinces.Count != 34 || Provinces.SelectMany(x => x.Wards).Count() != 3321)
            throw new InvalidOperationException("Danh mục địa giới phải có 34 tỉnh/thành và 3.321 xã/phường");
    }

    public bool TryCompose(string? streetAddress, string? provinceCode, string? wardCode, out string location)
    {
        location = "";
        var province = Provinces.SingleOrDefault(x => x.Code == provinceCode);
        var ward = province?.Wards.SingleOrDefault(x => x.Code == wardCode);
        var street = streetAddress?.Normalize().Trim();
        if (province is null || ward is null || string.IsNullOrWhiteSpace(street)) return false;

        location = $"{street}, {ward.Name}, {province.Name}";
        return location.EnumerateRunes().Count() <= 200;
    }

    public (string StreetAddress, string ProvinceCode, string WardCode) Parse(string location)
    {
        foreach (var province in Provinces)
        foreach (var ward in province.Wards)
        {
            var suffix = $", {ward.Name}, {province.Name}";
            if (location.EndsWith(suffix, StringComparison.Ordinal) && location.Length > suffix.Length)
                return (location[..^suffix.Length], province.Code, ward.Code);
        }

        return (location, "", "");
    }
}
