using System.Collections;
using BepInEx;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace CustomSkyBoxMod
{
    [BepInPlugin("Lupeydev.CustomSkyBoxMod", "CustomSkyBoxMod", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Start()
        {
            string imagePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "BepInEx/Plugins/CustomSkyBox/skybox.png");

            GameObject sky = GameObject.Find("Standard Sky");
            
            if (sky != null)
            {
                StartCoroutine(LoadImage(sky, imagePath));
            }
        }

        IEnumerator LoadImage(GameObject skyObj, string path)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path))
            {
                yield return request.SendWebRequest();
                
                if (request.result != UnityWebRequest.Result.Success)
                {
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                MeshRenderer renderer = skyObj.GetComponentInChildren<MeshRenderer>(true);
                if (renderer != null)
                {
                    Material material = new Material(Shader.Find("Unlit/Texture"));
                    material.mainTexture = texture;
                    renderer.material = material;
                }
            }
        }
    }
}