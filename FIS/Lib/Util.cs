using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace FIS.Lib
{
  internal static class Util
  {
    static readonly Encoding DefaultEncoding = Encoding.UTF8;
    static readonly HashAlgorithm DefaultHashAlgoritm = SHA512.Create();

    /// <summary>
    /// Computes the hash of given data. Auto generated salt (Guid), using SHA512 Hashing Algorithm, and UTF8 Encoding of data.
    /// </summary>
    /// <param name="data">Data to be hashed.</param>
    /// <returns>(Hash * Salt)</returns>
    internal static Tuple<byte[], Guid> DataHash(string data)
    {
      var salt = Guid.NewGuid();
      return Tuple.Create(DefaultHashAlgoritm.ComputeHash(salt.ToByteArray().Concat(DefaultEncoding.GetBytes(data)).ToArray()), salt);
    }

    /// <summary>
    /// Computes the hash of given data. Uses SHA512 Hashing Algorithm, and UTF8 Encoding of data.
    /// </summary>
    /// <param name="data">Data to be hashed.</param>
    /// <param name="salt">Guid Salt.</param>
    /// <returns>(Hash * Salt)</returns>
    internal static Tuple<byte[], Guid> DataHash(string data, Guid salt)
        => Tuple.Create(DefaultHashAlgoritm.ComputeHash(salt.ToByteArray().Concat(DefaultEncoding.GetBytes(data)).ToArray()), salt);

    /// <summary>
    /// Computes the hash of given data.
    /// </summary>
    /// <param name="salt">Guid salt.</param>
    /// <param name="data">Data to be hashed.</param>
    /// <param name="encoding">Data encoding used.</param>
    /// <param name="algo">The Hashing algorithm.</param>
    /// <returns>(Hash * Salt)</returns>
    internal static Tuple<byte[], Guid> DataHash(string data, Guid salt, Encoding encoding, HashAlgorithm algo)
        => Tuple.Create(algo.ComputeHash(salt.ToByteArray().Concat(encoding.GetBytes(data)).ToArray()), salt);

    /// <summary>
    /// Serves as a wrapper for condition statement.
    /// </summary>
    /// <param name="Condition">The Condition to be evaluated.</param>
    /// <param name="OnFail">Action to be executed when the Condition has failed.</param>
    /// <param name="OnSuccess">Action to be executed when the Condition has succedded.</param>
    /// <returns>The value of a condition.</returns>
    internal static bool OnConditional(bool Condition, Action OnFail, Action OnSuccess)
    {
      if (Condition) { OnSuccess(); return true; }
      OnFail(); return false;
    }

    /// <summary>
    /// Serves as a template for hiding the first Modern Window, and showing the second Modern Window.
    /// </summary>
    internal static Func<ModernWindow, Action<ModernWindow>> HideAndShow = x => y => { x.Hide(); y.Show(); };
    /// <summary>
    /// Serves as a template for hiding the first Modern Window, and showing dialog the second Modern Window.
    /// </summary>
    internal static Func<ModernWindow, Action<ModernWindow>> HideAndShowDialog = x => y => { x.Hide(); y.ShowDialog(); };
  }
}