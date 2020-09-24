using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 보낼 수 있는 기능을 지원

public class VirtualJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    
    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField]
    private TPSCharacterController tps;

    public enum JoyStickType
    {
        Move,
        Rotate,
    }

    public JoyStickType stickType;
    public float sensitibity = 1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // 드래그를 시작할 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
        isInput = true;
    }

    // 드래그 중일 때
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    // 드래그가 끝났을 때
    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        tps.Move(Vector2.zero);
        isInput = false;
    }

    private void ControlJoyStickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        // 캐릭터에게 입력벡터를 전달
        // Debug.Log(inputDirection.x + " / " + inputDirection.y);
        switch(stickType)
        {
            case JoyStickType.Move:
                tps.Move(inputDirection * sensitibity);
                break;
            case JoyStickType.Rotate:
                tps.LookAround(inputDirection * sensitibity);
                break;
        }

    }

    private void Update()
    {
        if(isInput)
        {
            InputControlVector();
        }
    }
}