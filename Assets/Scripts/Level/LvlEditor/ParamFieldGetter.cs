using UnityEngine;
using UnityEngine.Events;

public abstract class ParamFieldGetter : MonoBehaviour
{
    public abstract object GetValue();
    public abstract void SetValue(object value);
    public UnityEvent<string> OnValueChanged;
    public virtual void Focus(int x) { }
}
//field.ActivateInputField();
//field.caretPosition = param.Value.text.Length;
//field.selectionAnchorPosition = param.Value.text.Length;
//field.selectionFocusPosition = param.Value.text.Length;