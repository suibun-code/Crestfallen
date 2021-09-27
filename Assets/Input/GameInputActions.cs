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
                    ""name"": ""Strum"",
                    ""type"": ""Button"",
                    ""id"": ""78c39cad-1c3a-42f8-aece-241d4a73e23d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorRed"",
                    ""type"": ""Button"",
                    ""id"": ""0ecc624b-d009-4d78-a826-15aa363090f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorGreen"",
                    ""type"": ""Button"",
                    ""id"": ""a36e7345-991d-47b3-9616-53f720971a99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorBlue"",
                    ""type"": ""Button"",
                    ""id"": ""37e71634-83aa-49f8-9c94-fecb3c5c6a1a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeColorPurple"",
                    ""type"": ""Button"",
                    ""id"": ""be35f4b3-6b04-4b8d-a3e5-ad50be3e39bd"",
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
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorRed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""574d3f62-9851-440a-98c2-0d069a16c58c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorGreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a798baad-de05-447f-b7d5-c44cc49a18b8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorBlue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""133ccc1e-456b-48c4-a5eb-6094039db4cd"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ChangeColorPurple"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d624f1d3-5f69-4d7a-bd27-5f290c3cb899"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Strum"",
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
        m_Normal_Strum = m_Normal.FindAction("Strum", throwIfNotFound: true);
        m_Normal_ChangeColorRed = m_Normal.FindAction("ChangeColorRed", throwIfNotFound: true);
        m_Normal_ChangeColorGreen = m_Normal.FindAction("ChangeColorGreen", throwIfNotFound: true);
        m_Normal_ChangeColorBlue = m_Normal.FindAction("ChangeColorBlue", throwIfNotFound: true);
        m_Normal_ChangeColorPurple = m_Normal.FindAction("ChangeColorPurple", throwIfNotFound: true);
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
    private readonly InputAction m_Normal_Strum;
    private readonly InputAction m_Normal_ChangeColorRed;
    private readonly InputAction m_Normal_ChangeColorGreen;
    private readonly InputAction m_Normal_ChangeColorBlue;
    private readonly InputAction m_Normal_ChangeColorPurple;
    private readonly InputAction m_Normal_Pause;
    public struct NormalActions
    {
        private @GameInputActions m_Wrapper;
        public NormalActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Strum => m_Wrapper.m_Normal_Strum;
        public InputAction @ChangeColorRed => m_Wrapper.m_Normal_ChangeColorRed;
        public InputAction @ChangeColorGreen => m_Wrapper.m_Normal_ChangeColorGreen;
        public InputAction @ChangeColorBlue => m_Wrapper.m_Normal_ChangeColorBlue;
        public InputAction @ChangeColorPurple => m_Wrapper.m_Normal_ChangeColorPurple;
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
                @Strum.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrum;
                @Strum.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrum;
                @Strum.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnStrum;
                @ChangeColorRed.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRed;
                @ChangeColorRed.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRed;
                @ChangeColorRed.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorRed;
                @ChangeColorGreen.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorGreen;
                @ChangeColorGreen.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorGreen;
                @ChangeColorGreen.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorGreen;
                @ChangeColorBlue.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorBlue;
                @ChangeColorBlue.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorBlue;
                @ChangeColorBlue.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorBlue;
                @ChangeColorPurple.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorPurple;
                @ChangeColorPurple.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorPurple;
                @ChangeColorPurple.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnChangeColorPurple;
                @Pause.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_NormalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Strum.started += instance.OnStrum;
                @Strum.performed += instance.OnStrum;
                @Strum.canceled += instance.OnStrum;
                @ChangeColorRed.started += instance.OnChangeColorRed;
                @ChangeColorRed.performed += instance.OnChangeColorRed;
                @ChangeColorRed.canceled += instance.OnChangeColorRed;
                @ChangeColorGreen.started += instance.OnChangeColorGreen;
                @ChangeColorGreen.performed += instance.OnChangeColorGreen;
                @ChangeColorGreen.canceled += instance.OnChangeColorGreen;
                @ChangeColorBlue.started += instance.OnChangeColorBlue;
                @ChangeColorBlue.performed += instance.OnChangeColorBlue;
                @ChangeColorBlue.canceled += instance.OnChangeColorBlue;
                @ChangeColorPurple.started += instance.OnChangeColorPurple;
                @ChangeColorPurple.performed += instance.OnChangeColorPurple;
                @ChangeColorPurple.canceled += instance.OnChangeColorPurple;
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
        void OnStrum(InputAction.CallbackContext context);
        void OnChangeColorRed(InputAction.CallbackContext context);
        void OnChangeColorGreen(InputAction.CallbackContext context);
        void OnChangeColorBlue(InputAction.CallbackContext context);
        void OnChangeColorPurple(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
