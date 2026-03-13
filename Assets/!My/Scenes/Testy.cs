using UnityEngine;

public class Testy : MonoBehaviour
{
    public bool isSimulated;

    // Update is called once per frame
    void Update()
    {
        isSimulated = InputFabric.IsSimulatorView();
    }
}
