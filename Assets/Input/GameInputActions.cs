// GENERATED AUTOMATICALLY FROM 'Assets/Input/GameInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""Normal"",
            ""id"": ""d8530d79-73c9-4342-a5ee-636f83c25bdb"",
            ""actions"": [
                {
                    ""name"": ""ChangeColorLeftRed"",
                    ""type"": ""Button"",
                    ""id"": ""0ecc624b-d009-4d78-a826-15aa363090f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorLeftGreen"",
                    ""type"": ""Button"",
                    ""id"": ""a36e7345-991d-47b3-9616-53f720971a99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StrumLeft"",
                    ""type"": ""Button"",
                    ""id"": ""78c39cad-1c3a-42f8-aece-241d4a73e23d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorRightBlue"",
                    ""type"": ""Button"",
                    ""id"": ""37e71634-83aa-49f8-9c94-fecb3c5c6a1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorRightYellow"",
                    ""type"": ""Button"",
                    ""id"": ""be35f4b3-6b04-4b8d-a3e5-ad50be3e39bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StrumRight"",
                    ""type"": ""Button"",
                    ""id"": ""443bb9a6-60aa-4a3a-a654-663f1c802ca5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StrumWide"",
                    ""type"": ""Button"",
                    ""id"": ""ab85f1a6-0c0f-433e-b1ea-9c305ce524d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""d02e1f38-4673-49fe-8076-cb2baabb263a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ca16511-7813-4bf1-8c9d-9ec5dbeccdd7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorLeftRed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""574d3f62-9851-440a-98c2-0d069a16c58c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorLeftGreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a798baad-de05-447f-b7d5-c44cc49a18b8"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorRightBlue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""133ccc1e-456b-48c4-a5eb-6094039db4cd"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorRightYellow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af7010b9-e79a-4513-9054-5c84935e066b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82b4e7f5-8abe-4d1a-afbb-23f8e1dd17ee"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""StrumLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d7ee895-1ede-4e43-ad3f-97040e802add"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StrumRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b724e94-d57e-40e9-868c-e8a821b084ed"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StrumWide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Normal
        m_Normal = asset.FindActionMap("Normal", throwIfNotFound: true);
        m_Normal_ChangeColorLeftRed = m_Normal.FindAction("ChangeColorLeftRed", throwIfNotFound: true);
        m_Normal_ChangeColorLeftGreen = m_Normal.FindAction("ChangeColorLeftGreen", throwIfNotFound: true);
        m_Normal_StrumLeft = m_Normal.FindAction("StrumLeft", throwIfNotFound: true);
        m_Normal_ChangeColorRightBlue = m_Normal.FindAction("ChangeColorRightBlue", throwIfNotFound: true);
        m_Normal_ChangeColorRightYellow = m_Normal.FindAction("ChangeColorRightYellow", throwIfNotFound: true);
        m_Normal_StrumRight = m_Normal.FindAction("StrumRight", throwIfNotFound: true);
        m_Normal_StrumWide = m_Normal.FindAction("StrumWide", throwIfNotFound: true);
        m_Normal_Pause = m_Normal.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Normal
    private readonly InputActionMap m_Normal;
    private INormalActions m_NormalActionsCallbackInterface;
    private readonly InputAction m_Normal_ChangeColorLeftRed;
    private readonly InputAction m_Normal_ChangeColorLeftGreen;
    private readonly InputAction m_Normal_StrumLeft;
    private readonly InputAction m_Normal_ChangeColorRightBlue;
    private readonly InputAction m_Normal_ChangeColorRightYellow;
    private readonly InputAction m_Normal_StrumRight;
    private readonly InputAction m_Normal_StrumWide;
    private readonly InputAction m_Normal_Pause;
    public struct NormalActions
    {
        private @GameInputActions m_Wrapper;
        public NormalActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeColorLeftRed => m_Wrapper.m_Normal_ChangeColorLeftRed;
        public InputAction @ChangeColorLeftGreen => m_Wrapper.m_Normal_ChangeColorLeftGreen;
        public InputAction @StrumLeft => m_Wrapper.m_Normal_StrumLeft;
        public InputAction @ChangeColorRightBlue => m_Wrapper.m_Normal_ChangeColorRightBlue;
        public InputAction @ChangeColorRightYellow => m_Wrapper.m_Normal_ChangeColorRightYellow;
        public InputAction @StrumRight => m_Wrapper.m_Normal_StrumRight;
        public InputAction @StrumWide => m_Wrapper.m_Normal_StrumWide;
        public InputAction @Pause => m_Wrapper.m_Normal_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Normal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NormalActions set) { return set.Get(); }
        public void SetCallbacks(INormalActions instance)
        {
            if (m_Wrapper.m_NormalActionsCallbackInterface != null)
            {
                @ChangeColorLeftRed.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftRed;
                @ChangeColorLeftRed.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftRed;
                @ChangeColorLeftRed.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftRed;
                @ChangeColorLeftGreen.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftGreen;
                @ChangeColorLeftGreen.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftGreen;
                @ChangeColorLeftGreen.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorLeftGreen;
                @StrumLeft.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumLeft;
                @StrumLeft.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumLeft;
                @StrumLeft.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumLeft;
                @ChangeColorRightBlue.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightBlue;
                @ChangeColorRightBlue.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightBlue;
                @ChangeColorRightBlue.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightBlue;
                @ChangeColorRightYellow.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightYellow;
                @ChangeColorRightYellow.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightYellow;
                @ChangeColorRightYellow.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRightYellow;
                @StrumRight.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumRight;
                @StrumRight.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumRight;
                @StrumRight.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumRight;
                @StrumWide.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumWide;
                @StrumWide.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumWide;
                @StrumWide.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrumWide;
                @Pause.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_NormalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeColorLeftRed.started += instance.OnChangeColorLeftRed;
                @ChangeColorLeftRed.performed += instance.OnChangeColorLeftRed;
                @ChangeColorLeftRed.canceled += instance.OnChangeColorLeftRed;
                @ChangeColorLeftGreen.started += instance.OnChangeColorLeftGreen;
                @ChangeColorLeftGreen.performed += instance.OnChangeColorLeftGreen;
                @ChangeColorLeftGreen.canceled += instance.OnChangeColorLeftGreen;
                @StrumLeft.started += instance.OnStrumLeft;
                @StrumLeft.performed += instance.OnStrumLeft;
                @StrumLeft.canceled += instance.OnStrumLeft;
                @ChangeColorRightBlue.started += instance.OnChangeColorRightBlue;
                @ChangeColorRightBlue.performed += instance.OnChangeColorRightBlue;
                @ChangeColorRightBlue.canceled += instance.OnChangeColorRightBlue;
                @ChangeColorRightYellow.started += instance.OnChangeColorRightYellow;
                @ChangeColorRightYellow.performed += instance.OnChangeColorRightYellow;
                @ChangeColorRightYellow.canceled += instance.OnChangeColorRightYellow;
                @StrumRight.started += instance.OnStrumRight;
                @StrumRight.performed += instance.OnStrumRight;
                @StrumRight.canceled += instance.OnStrumRight;
                @StrumWide.started += instance.OnStrumWide;
                @StrumWide.performed += instance.OnStrumWide;
                @StrumWide.canceled += instance.OnStrumWide;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public NormalActions @Normal => new NormalActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface INormalActions
    {
        void OnChangeColorLeftRed(InputAction.CallbackContext context);
        void OnChangeColorLeftGreen(InputAction.CallbackContext context);
        void OnStrumLeft(InputAction.CallbackContext context);
        void OnChangeColorRightBlue(InputAction.CallbackContext context);
        void OnChangeColorRightYellow(InputAction.CallbackContext context);
        void OnStrumRight(InputAction.CallbackContext context);
        void OnStrumWide(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
