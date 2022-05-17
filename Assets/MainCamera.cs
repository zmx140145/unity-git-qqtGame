using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Material DieMaterial;
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
    public IEnumerator PlayerDie()
    {
    float value=0f;
    while (value<=1f)
    {
    DieMaterial.SetVector("_Data",new Vector4(0.30f,0.59f,0.11f,value));
    value+=Time.deltaTime;
    yield return 0;
    }
     DieMaterial.SetVector("_Data",new Vector4(0.30f,0.59f,0.11f,1f));
    yield return new WaitForSeconds(3f);
     while (value>=0f)
    {
    DieMaterial.SetVector("_Data",new Vector4(0.30f,0.59f,0.11f,value));
    value-=Time.deltaTime;
    yield return 0;
    }
     DieMaterial.SetVector("_Data",new Vector4(0.30f,0.59f,0.11f,0f));
    yield break;
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(DieMaterial!=null)
        {
           Graphics.Blit(src,dest,DieMaterial);
        }
        else
        {
            Graphics.Blit(src,dest);
        }
    }
}
