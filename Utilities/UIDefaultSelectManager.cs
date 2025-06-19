using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

// NOTE:Navigationシステムに通すために実装したが、スティックが暴れる原因になるので非推奨
public class UIDefaultSelectManager : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _uiInputActionAsset;

    [SerializeField]
    private GameObject _defaultSelectTarget;
    [SerializeField]
    private float _delayEnableTime = 1.0f;

    // Start is called before the first frame update
    private void OnEnable()
    {
        Debug.Log($"OnEnable: {name} - {_defaultSelectTarget.name}");
        if (_uiInputActionAsset)
        {
            _uiInputActionAsset["Move"].started += OnMoveInput;
        }
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(_delayEnableTime);
        EventSystem.current.SetSelectedGameObject(_defaultSelectTarget);
    }

    private void OnDisable()
    {
        if (_uiInputActionAsset != null)
        {
            _uiInputActionAsset["Move"].started -= OnMoveInput;
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_defaultSelectTarget);
        }
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_defaultSelectTarget);
        }
    }

    public void Setup(InputActionAsset asset)
    {
        _defaultSelectTarget = gameObject;
        _uiInputActionAsset = asset;
    }
}