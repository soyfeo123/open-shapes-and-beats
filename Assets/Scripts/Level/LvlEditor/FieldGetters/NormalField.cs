using UnityEngine;

public class NormalField : ParamFieldGetter
{
    private TMPro.TMP_InputField m_field;

    public override object GetValue()
    {
        m_field = GetComponent<TMPro.TMP_InputField>();
        return m_field.text;
    }

    public override void SetValue(object value)
    {
        m_field = GetComponent<TMPro.TMP_InputField>();
        m_field.text = value as string;
    }

    public override void Focus(int x)
    {
        m_field.ActivateInputField();
        m_field.caretPosition = x;
        m_field.selectionAnchorPosition = x;
        m_field.selectionFocusPosition = x;
    }

    private void Start()
    {
        m_field = GetComponent<TMPro.TMP_InputField>();
        m_field.onValueChanged.AddListener((string val) =>
        {
            OnValueChanged?.Invoke(val);
        });
    }
}
