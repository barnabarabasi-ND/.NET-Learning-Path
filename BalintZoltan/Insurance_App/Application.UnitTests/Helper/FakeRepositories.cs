using Application.UnitTests.Fakes;

namespace Application.UnitTests.Helper;

public sealed class FakeRepositories
{
    public FakeBuildingRepository Building { get; } = new();
    public FakeClientRepository Client { get; } = new();
    public FakeGeographyRepository Geography { get; } = new();
}
