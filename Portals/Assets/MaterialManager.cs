using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;
    private void Awake() {
        if(Instance != null && Instance != this) Destroy(this.gameObject);  
        else Instance = this;  
    }


    enum CompareOperation {
        Equal = 4, NotEqual = 5
    }
    
    [SerializeField]
    private Material[] materialsUsingStencil;
    public void InvertStencil()  {
        foreach (var material in materialsUsingStencil)
        {
            var currentCompare = (CompareOperation) material.GetInt("_CompareOperation");
            switch (currentCompare)
            {
                case CompareOperation.Equal:
                    material.SetInt("_CompareOperation", (int)CompareOperation.NotEqual);
                break;
                case CompareOperation.NotEqual:
                    material.SetInt("_CompareOperation", (int)CompareOperation.Equal);
                break;
            }
        }
    }
    
}