namespace CopyDirectoryAsyncProviderNS;

public static class CopyDirectoryAsyncProvider
{
    public static Task CopyDirectoryAsync(
        DirectoryInfo sourceDirectoryInfo,
        DirectoryInfo targetDirectoryInfo,
        CancellationToken cancellationToken
    )
    {
        if (!sourceDirectoryInfo.Exists)
        {
            throw new DirectoryNotFoundException(message: $"{sourceDirectoryInfo.FullName} was not found");
        }

        // If the destination directory doesn't exist, create it
        targetDirectoryInfo.Create();

        return Task.WhenAll(
            Task.WhenAll(
                tasks: sourceDirectoryInfo
                    .EnumerateFiles()
                    .Select(selector: childFileInfo =>
                        CopyFileWithoutCreatingDirectoryAsync(
                            sourceFileInfo: childFileInfo,
                            targetFileInfo: new FileInfo(
                                fileName: Path.Combine(
                                    path1: targetDirectoryInfo.FullName,
                                    path2: childFileInfo.Name
                                )
                            ),
                            cancellationToken: cancellationToken
                        )
                    )
            ),
            Task.WhenAll(
                tasks: sourceDirectoryInfo
                    .EnumerateDirectories()
                    .Select(selector: childDirectoryInfo =>
                        CopyDirectoryAsync(
                            sourceDirectoryInfo: childDirectoryInfo,
                            targetDirectoryInfo: new DirectoryInfo(
                                path: Path.Combine(
                                    path1: targetDirectoryInfo.FullName,
                                    path2: childDirectoryInfo.Name
                                )
                            ),
                            cancellationToken: cancellationToken
                        )
                    )
            )
        );
    }

    public static Task CopyFileAsync(
        FileInfo sourceFileInfo,
        FileInfo targetFileInfo,
        CancellationToken cancellationToken
    )
    {
        targetFileInfo.Directory!.Create();
        return CopyFileWithoutCreatingDirectoryAsync(
            sourceFileInfo: sourceFileInfo,
            targetFileInfo: targetFileInfo,
            cancellationToken: cancellationToken
        );
    }

    private static async Task CopyFileWithoutCreatingDirectoryAsync(
        FileInfo sourceFileInfo,
        FileInfo targetFileInfo,
        CancellationToken cancellationToken
    )
    {
        await using var readStream = sourceFileInfo.OpenRead();
        await using var writeStream = targetFileInfo.Create();
        await readStream.CopyToAsync(
            destination: writeStream,
            cancellationToken: cancellationToken
        );
    }
}