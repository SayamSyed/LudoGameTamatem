using UnityEngine;
using RandomOrg.CoreApi;
using RandomOrg.CoreApi.Errors;

public class RandomAPI : MonoBehaviour
{
    private string url = "https://www.random.org/clients/http/api/";

    public static int GetRandomNumber()
    {
        RandomOrgClient roc = RandomOrgClient.GetRandomOrgClient("dd73ec3b-341c-44ef-9b0d-5181ca3c0e74"/*,0, 10000, false*/);
        try
        {
            int[] response = roc.GenerateIntegers(1, 1, 6);
            return response[0];
        }
        catch (RandomOrgCacheEmptyException)
        {
            print("maybe we got an error. hmm");
            // in case of true random number not found we use a pseudo random number generator
            int rand = Random.Range(1, 7);
            return rand;
        }
    }
}
