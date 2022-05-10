using Project;
using Project.UI;
using UnityEngine;

public class RandomOpponent : MonoBehaviour
{
    [SerializeField]
    private Mesh[] _girlsMeshs = null;
    

    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    private void OnEnable()
    {
        ResultPopup.ClaimClicked += ResultPopup_ClaimClicked;
    }

    private void OnDisable()
    {
        ResultPopup.ClaimClicked -= ResultPopup_ClaimClicked;
    }

    private void ResultPopup_ClaimClicked()
    {
        _skinnedMeshRenderer.sharedMesh = _girlsMeshs.RandomElement();
    }
}
