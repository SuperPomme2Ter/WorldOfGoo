using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GravityMap : MonoBehaviour
{
    [SerializeField] GameObject pixelPrefab;
    [SerializeField] GameObject pixelPointParent;
    [SerializeField] Gradient forceGradient;
    [SerializeField] Gradient forceGradient2;
    [SerializeField] private bool createPixels;
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] string jsonSavePath;
    int width = 256;
    int height = 240;
    public static List<List<GameObject>> pixels = new ();
    List<List<float>> pixelsColorValue= new ();
    public static Vector2[] pixelsForce;
    
    
    [ContextMenu("Generate Pixels")]
    private void GeneratePixels()
    {
        if (File.Exists(jsonSavePath))
        {
            
            string jsonFile = File.ReadAllText(jsonSavePath);
            if (jsonFile != "")
            {
                File.WriteAllText(jsonSavePath, String.Empty);
            }
            createPixels = false;
            PixelsCalculation();
            float[][] pixelArray= new float[width*height][];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    pixelArray[i*height + j] = new float[2] { pixelsForce[i*height + j].x, pixelsForce[i*height + j].y };
                }
            }
            

            string pixelDataJson = JsonConvert.SerializeObject(pixelArray);
            
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                writer.Formatting = Formatting.None;
                
                serializer.Serialize(writer, pixelArray);
            }
            File.WriteAllText(jsonSavePath, sw.ToString());
        }
        else
        {
            Debug.Log("aezeqfrdzsq");
        }
        

    }

    private void ReadJsonPixels(Vector2[] pixelsList)
    {
        if (File.Exists(jsonSavePath))
        {
            
            using (var stream = File.OpenRead(jsonSavePath))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                if (reader.Peek() <= -1)
                {
                    PixelsCalculation();
                }
                
            }
            float[][] pixelArray;
            
            pixelArray = JsonConvert.DeserializeObject<float[][]>(File.ReadAllText(jsonSavePath));
            
            
            for (int i = 0; i < pixelArray.Length; i++)
            {

                    pixelsList[i].x = pixelArray[i][0];
                    pixelsList[i].y = pixelArray[i][1];
                
            }

        }
    }
    // private string GetPath(string filename)
    //  {
    //      return Application.dataPath + "/Saves/" + filename;
    //      File.WriteAllText(jsonSavePath,pixelsForce.ToString());
    //  }
     

     void Start()
     {

         // for (int i = 0; i < width; i++)
         // {
         //     for (int j = 0; j < height; j++)
         //     {
         //        Debug.Log(i*height + j);
         //     }
         // }
        //
        jsonSavePath=Application.dataPath + "/Saves/" + SceneManager.GetActiveScene().name+" save.json";
        pixelsForce = new Vector2[width*height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                pixelsForce[i*height + j]=(new Vector2(0, 0));
            }
            
        }
        ReadJsonPixels(pixelsForce);
        //PixelsCalculation();
        //GeneratePixels();
        Debug.Log(Time.realtimeSinceStartup);
        // string aaa=ReadFile(jsonSavePath);
        // if (aaa == "")
        // {
        //     Debug.Log("oscour");
        // }
        // else
        // {
        //     Debug.Log(aaa);
        // }
        //stationMass=BeginningCell.instance.stationPrefab.GetComponent<Rigidbody2D>().mass;
        //StartCoroutine(PixelsCalculation()); 
    }

    private void PixelsCalculation()
    {
        List<GameObject> planets = new List<GameObject>();
        pixelsForce = new Vector2[width*height];
        foreach (Transform child in transform)
        {
            planets.Add(child.gameObject);
        }

        for (int i = 0; i < width; i++)
        {
            if (createPixels)
            {
                pixels.Add(new List<GameObject>());
                pixelsColorValue.Add(new List<float>());
            }
            
            for (int j = 0; j < height; j++)
            {
                if (createPixels)
                {
                    GameObject newPixel = Instantiate(pixelPrefab, pixelPointParent.transform, false);
                    newPixel.GetComponent<S_PixelInfo>().indexX = i;
                    newPixel.GetComponent<S_PixelInfo>().indexY = j;
                    newPixel.transform.localPosition = new Vector2(i, j);
                    pixels[i].Add(newPixel);
                    pixelsColorValue[i].Add(0);
                }
                pixelsForce[i+j*width]=Vector2.zero;

            }
        }
        foreach (GameObject planet in planets)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector2 pixelPosition= new Vector2(i,j);
                    float dist = Vector2.Distance(pixelPosition, planet.transform.position);
                    //dist *= 1000;
                    Vector2 direction =  (Vector2)planet.transform.position-pixelPosition;
                    direction.Normalize();
                    pixelsForce[i+j*width] += direction * (Mathf.Pow(10,-11)*6.67430f*(planet.GetComponentInChildren<CelestialBody>().preciseWeight*Mathf.Pow(10,planet.GetComponentInChildren<CelestialBody>().weight)*(stationPrefab.GetComponent<CellAttraction>().preciseWeight*Mathf.Pow(10,stationPrefab.GetComponent<Rigidbody2D>().mass)))/Mathf.Pow(dist,2));
                    
                    if (createPixels)
                    {
                        
                        pixels[i][j].GetComponent<S_PixelInfo>().force=pixelsForce[i+j*width];
                        float rslt = pixelsForce[i+j*width].magnitude/1000;
                        pixelsColorValue[i][j] += rslt;
                    }
                }
            }
            Debug.Log("AAAAAA");
        }
        
        if (createPixels)
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                for (int j = 0; j < pixels[i].Count; j++)
                {
                    if (pixelsColorValue[i][j] > 50)
                    {
                        pixels[i][j].GetComponent<SpriteRenderer>().color =
                            forceGradient2.Evaluate(pixelsColorValue[i][j]-50);
                        pixels[i][j].GetComponent<S_PixelInfo>().colorValue = pixelsColorValue[i][j];
                        continue;
                    }
                    pixels[i][j].GetComponent<SpriteRenderer>().color =
                        forceGradient.Evaluate(pixelsColorValue[i][j]);
                    pixels[i][j].GetComponent<S_PixelInfo>().colorValue = pixelsColorValue[i][j];
                }
            }
        }
        
    }
}
//template<NoiseFunc N>
//void createNoiseImage(const char *filename) 
//{ 
//    unsigned imageWidth = 512, imageHeight = 512;
//float invImageWidth = 1.f / imageWidth;
//float invImageHeight = 1.f / imageHeight;
//float noiseFrequency = 5;
//float* imageBuffer = new float[imageWidth * imageHeight];
//float* currPixel = imageBuffer;
//for (unsigned j = 0; j < imageHeight; ++j)
//{
//    for (unsigned i = 0; i < imageWidth; ++i)
//    {
//        Vec2f P(i* invImageWidth, j* imvImageHeight) *noiseFrequency;
//        *currPixel = (*N)(P);
//        currPixel++;
//    }
//}
//saveImage(filename, imageBuffer, imageWidth, imageHeight);
//delete[] imageBuffer; 
//} 





// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEngine;
//
// public class PromptReader : MonoBehaviour
// {
//     public static List<T> ReadListFromJSON<T>(string filename)
//     {
//         string content = ReadFile(GetPath(filename));
//
//         if (string.IsNullOrEmpty(content) || content == "{}")
//         {
//             Debug.Log("FILE IS NULL OR EMPTY");
//             return new List<T>();
//         }
//
//         Debug.Log(content);
//         List<T> res = JsonHelper.FromJson<T>(content).ToList();
//
//         return res;
//
//     }
//
//     public static T ReadFromJSON<T>(string filename)
//     {
//         string content = ReadFile(GetPath(filename));
//
//         if (string.IsNullOrEmpty(content) || content == "{}")
//         {
//             return default(T);
//         }
//
//         T res = JsonUtility.FromJson<T>(content);
//
//         return res;
//
//     }
//
//     private static string GetPath(string filename)
//     {
//         return Application.dataPath + "/Scripts/" + filename;
//     }
//     private static string ReadFile(string path)
//     {
//         if (File.Exists(path))
//         {
//             using (StreamReader reader = new StreamReader(path))
//             {
//                 string content = reader.ReadToEnd();
//                 return content;
//             }
//         }
//         return "";
//     }
//     public static class JsonHelper
//     {
//         public static T[] FromJson<T>(string json)
//         {
//             Wrapper<T> wrapper = new Wrapper<T>();
//             wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//             if (wrapper == null) Debug.Log("WRAPPER IS NULL");
//             return wrapper.Items;
//         }
//
//         public static string ToJson<T>(T[] array)
//         {
//             Wrapper<T> wrapper = new Wrapper<T>();
//             wrapper.Items = array;
//             return JsonUtility.ToJson(wrapper);
//         }
//
//         public static string ToJson<T>(T[] array, bool prettyPrint)
//         {
//             Wrapper<T> wrapper = new Wrapper<T>();
//             wrapper.Items = array;
//             return JsonUtility.ToJson(wrapper, prettyPrint);
//         }
//
//         [Serializable]
//         private class Wrapper<T>
//         {
//             public T[] Items;
//         }
//     }
// }
