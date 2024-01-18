using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//Note To Viewer :: code is not refactored at all,
//please ignore as I tried to make it as fast as possible with my fulltime job

public class AddressablesInitializer : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject ludoChip, rollingDie;
    GameObject _chipInstance, _dieInstance;

    public void Start()
    {
        ludoChip.InstantiateAsync().Completed += ChipLoaded;
        rollingDie.InstantiateAsync().Completed += DieLoaded;
    }

    private void ChipLoaded(AsyncOperationHandle<GameObject> operation) 
    {
        if(operation.Status == AsyncOperationStatus.Succeeded) 
        {
            _chipInstance = operation.Result;
            GameManager.Instance.PlayerChipLoadedSuccessfully(_chipInstance);
        }
        else 
        {
            Debug.LogError("LoadingAssetFailed");
        }
    }
    private void DieLoaded(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            _dieInstance = operation.Result;
            GameManager.Instance.RollingDieLoadedSuccessfully(_dieInstance);
        }
        else
        {
            Debug.LogError("LoadingAssetFailed");
        }
    }
}
