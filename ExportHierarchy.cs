using UnityEngine;
using System.IO;

public class ExportHierarchy : MonoBehaviour
{
    void Start()
    {
        string path = Path.Combine(Application.dataPath, "SceneHierarchy.txt");
        StreamWriter writer = new StreamWriter(path);
        
        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            WriteObject(obj, writer, 0);
        }
        
        writer.Close();
        Debug.Log("Hierarchy exported at: " + path);
    }

    void WriteObject(GameObject obj, StreamWriter writer, int indent)
    {
        writer.WriteLine(new string(' ', indent * 2) + obj.name);
        foreach (Transform child in obj.transform)
        {
            WriteObject(child.gameObject, writer, indent + 1);
        }
    }
}
