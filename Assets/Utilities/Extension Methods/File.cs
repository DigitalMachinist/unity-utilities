using System.IO;
using UnityEngine;

public static partial class UnityExtensionMethods
{
    /// <summary>
    /// Check if this string is a valid path within Unity's persistent data path on this 
    /// machine's file system.
    /// </summary>
    /// <param name="candidatePath">A string that may be a valid filesystem path.</param>
    public static bool IsValidPersistentDataPath( this string candidatePath )
    {
        // On Windows machines, using the forward-slash is illegal.
        if ( Path.DirectorySeparatorChar == '\\' && candidatePath.IndexOf( '/' ) > -1 )
        {
            return false;
        }

        // Make sure that candidatePath is actually inside the persistent data folder.
        string fullPath = Path.GetFullPath( candidatePath );
        if ( fullPath.IndexOf( Application.persistentDataPath ) < 0 )
        {
            return false;
        }

        // If we got this far, the path is valid.
        return true;
    }

    /// <summary>
    /// Check if this string is a valid path within the Unity static resource bundle included 
    /// in this package.
    /// </summary>
    /// <param name="candidatePath">A string that may be a valid resource bundle path.</param>
    public static bool IsValidResourceBundlePath( this string candidatePath )
    {
        // Resource bundle paths use forward-slashes, so reject it if it uses backslashes.
        if ( candidatePath.IndexOf( '\\' ) > -1 )
        {
            return false;
        }

        // If this path is absolute, then it is pointing to a location of the file system, not 
        // the resource bundle, so it should be rejected.
        if ( Path.IsPathRooted( candidatePath ) )
        {
            return false;
        }

        // If we got this far, the path is valid.
        return true;
    }

    /// <summary>
    /// Check if this string is a path to an existing file or directory.
    /// </summary>
    /// <param name="candidatePath">A string that may be a valid resource bundle path.</param>
    public static bool PathExists( this string candidatePath )
    {
        string directoryPath = Path.GetDirectoryName( candidatePath );
        if ( !Directory.Exists( directoryPath ) )
        {
            return false;
        }

        string filePath = Path.GetFileName( candidatePath );
        if ( filePath != "" && !File.Exists( filePath ) )
        {
            return false;
        }

        // If we got this far, the path exists.
        return true;
    }

    /// <summary>
    /// Convert a file or folder path into a valid path to Unity's persistent data folder.
    /// </summary>
    /// <param name="path">A string file or folder path.</param>
    public static string ToPersistentDataPath( this string path )
    {
        // Transform Windows filepaths into OS-specific filepaths and make sure the persistent 
        // data path has be prefixed onto the path so that files are saved into the appropriate
        // directory for Quench on the host machine.
        path = path.Replace( '\\', Path.DirectorySeparatorChar );
        path = path.Replace( Application.persistentDataPath, "" );
        path = Path.Combine( Application.persistentDataPath, path );

        return path;
    }

    /// <summary>
    /// Convert a file or folder path into a valid path within Unity's static resrouce bundle.
    /// </summary>
    /// <param name="path">A string file or folder path.</param>
    public static string ToResourceBundlePath( this string path )
    {
        // Transform Windows filepaths into Unity resource bundle filepaths.
        path = path.Replace( '\\', '/' );

        return path;
    }
}
