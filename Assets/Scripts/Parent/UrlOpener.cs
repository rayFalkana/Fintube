using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class UrlOpener : MonoBehaviour
{
    public string url;

    public void GoToURL()
    {
        Process.Start(url);
    }
}
