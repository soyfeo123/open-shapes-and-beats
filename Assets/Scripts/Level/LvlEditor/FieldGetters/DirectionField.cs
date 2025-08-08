using UnityEngine;

public class DirectionField : ParamFieldGetter
{
    private TMPro.TMP_Dropdown m_field;

    public override object GetValue()
    {
        m_field = GetComponent<TMPro.TMP_Dropdown>();
        return m_field.value;
    }

    public override void SetValue(object value)
    {
        m_field = GetComponent<TMPro.TMP_Dropdown>();
        m_field.value = int.Parse(value.ToString());
    }

    private void Start()
    {
        m_field = GetComponent<TMPro.TMP_Dropdown>();
        m_field.onValueChanged.AddListener((int newVal) =>
        {
            OnValueChanged?.Invoke(newVal.ToString());
        });
    }
}
