using TMPro;
using Unity.Collections;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private Name _name;

    private void Awake()
    {
        _name.playerName.OnValueChanged += ChangeText;
    }

    private void Start()
    {
        ChangeText("", _name.playerName.Value);
    }

    private void ChangeText(FixedString64Bytes oldString, FixedString64Bytes newString)
    {
        _userName.text = newString.ToString();
    }
}
