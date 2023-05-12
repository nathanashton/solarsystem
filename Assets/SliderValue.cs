using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SliderValue : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The text shown will be formatted using this string.  {0} is replaced with the actual value")]
    private string formatText = "{0}";

    private TextMeshProUGUI tmproText;

    private void Start()
    {
        tmproText = GetComponent<TextMeshProUGUI>();
        var t = gameObject.GetComponentInParent<Slider>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(HandleValueChanged);
        tmproText.text = string.Format(formatText, t.value);

    }

    private void HandleValueChanged(float value)
    {
        tmproText.text = string.Format(formatText, value);
    }
}