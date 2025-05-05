using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class S_GravityMap : MonoBehaviour
{
    [SerializeField] GameObject pixelPrefab;
    [SerializeField] GameObject pixelPointParent;
    [SerializeField] Gradient forceGradient;
    [SerializeField] Gradient forceGradient2;
    [SerializeField] private bool createPixels;
    [SerializeField] private GameObject stationPrefab;
    int width = 256;
    int height = 240;
    public static List<List<GameObject>> pixels = new ();
    List<List<float>> pixelsColorValue= new ();
    public static List< List<Vector2>> pixelsForce = new ();



    void Start()
    {
        //stationMass=BeginningCell.instance.stationPrefab.GetComponent<Rigidbody2D>().mass;
        StartCoroutine(PixelsCalculation());
    }

    private IEnumerator PixelsCalculation()
    {
        float aaa = 0;
        Debug.Log("Beginning");
        yield return new WaitForEndOfFrame();
        List<GameObject> planets = new List<GameObject>();
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
            pixelsForce.Add(new List<Vector2>());
            
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
                pixelsForce[i].Add(Vector2.zero);

            }
        }
        Debug.Log("pixels instantiated");
        Debug.Log("calculating pixels value");
        yield return new WaitForEndOfFrame();
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
                    pixelsForce[i][j] += direction * (Mathf.Pow(10,-11)*6.67430f*(planet.GetComponent<CelestialBody>().preciseWeight*Mathf.Pow(10,planet.GetComponent<CelestialBody>().weight)*(stationPrefab.GetComponent<CellAttraction>().preciseWeight*Mathf.Pow(10,stationPrefab.GetComponent<Rigidbody2D>().mass)))/Mathf.Pow(dist,2));
                    
                    if (createPixels)
                    {
                        
                        pixels[i][j].GetComponent<S_PixelInfo>().force=pixelsForce[i][j];
                        float rslt = (pixelsForce[i][j].magnitude/(planet.GetComponent<CelestialBody>().preciseWeight*Mathf.Pow(10,4)));
                        pixelsColorValue[i][j] += rslt;
                    }


                }
            }
        }
        
        Debug.Log("done");
        Debug.Log("apply value to pixels");
        yield return new WaitForEndOfFrame();
        if (createPixels)
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                for (int j = 0; j < pixels[i].Count; j++)
                {
                    // if (pixelsColorValue[i][j] > 50)
                    // {
                    //     pixels[i][j].GetComponent<SpriteRenderer>().color =
                    //         forceGradient2.Evaluate(pixelsColorValue[i][j]);
                    //     continue;
                    // }
                    if ((pixelsColorValue[i][j]) > aaa)
                    {
                        aaa=pixelsColorValue[i][j];
                    }
                    pixels[i][j].GetComponent<SpriteRenderer>().color =
                        forceGradient.Evaluate(pixelsColorValue[i][j]);
                    pixels[i][j].GetComponent<S_PixelInfo>().colorValue = pixelsColorValue[i][j];
                }
            }
        }
        Debug.Log(aaa);
        Debug.Log("done");
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
