using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilsMathFunctionTests
{
    [Test]
    public void TestWorldToPixels()
    {
        float original = 1;
        float converted = Utils.MapWorldToPixel(original, UtilsDirection.X);
        float reconverted = Utils.ConvertPixelToPosition(converted, UtilsDirection.X);

        Debug.Log("converted: " + converted + "\nreconverted: " + reconverted);

        Assert.AreEqual(original, reconverted, 0.0001f);
    }
}
