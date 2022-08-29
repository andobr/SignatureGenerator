namespace SignatureGenerator.Models;

public record EnvironmentParameters(int BackgroundThreadCount)
{
    public int OverallThreadCount => BackgroundThreadCount + 1;
}