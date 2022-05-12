//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/_Scripts/Player/Controller.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controller : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""ControlCamera"",
            ""id"": ""bb818673-69d1-4a68-80df-952ebed8f3f1"",
            ""actions"": [
                {
                    ""name"": ""RightHand"",
                    ""type"": ""Value"",
                    ""id"": ""780fd5e4-70eb-4571-a452-7512f8821818"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftHand"",
                    ""type"": ""Value"",
                    ""id"": ""bd066df5-5357-446b-b641-7b06ee3d72ea"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RightHandCharacterChange"",
                    ""type"": ""Button"",
                    ""id"": ""6547572e-08ea-4e32-a44c-195a5518df86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftHandCharacterChange"",
                    ""type"": ""Button"",
                    ""id"": ""12bc5ecb-a0d5-421e-a5b2-05b40a2ee312"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightHandTurnRight"",
                    ""type"": ""Button"",
                    ""id"": ""8e367309-f4fb-4f7d-b444-5e6133013446"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightHandTurnLeft"",
                    ""type"": ""Button"",
                    ""id"": ""53bf09a7-183c-40d6-932d-3735f19beae1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftHandTurnRight"",
                    ""type"": ""Button"",
                    ""id"": ""5ac17b26-dca9-41c1-8e87-3ca69972be1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftHandTurnLeft"",
                    ""type"": ""Button"",
                    ""id"": ""02e65b00-def1-422f-a5f9-38a94f61d095"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RigthHandShoulder"",
                    ""type"": ""Button"",
                    ""id"": ""6ee81562-7f40-48d9-8a32-261beb726ba6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeaveShoulder"",
                    ""type"": ""Button"",
                    ""id"": ""ea87e502-bce0-4360-868b-7cc9610854b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9f8e124c-0fca-4102-a659-472181a45196"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e24abba0-7b7e-4e56-97e5-04d597a6f1b4"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5f422d47-5a51-4bf5-a539-706cbac13ae8"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4511ea59-627e-48db-ae6b-c2fb47e21c42"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ef2d5a7b-9c21-4e70-8af4-2463b285165f"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b5bd8200-9063-4c40-8691-aa199c67dedf"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""be9cea37-1cb4-4179-9222-be6da1748688"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""7a55c936-47e0-44f4-8407-5adae0084c1e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""98a2e5ad-c1f1-4cd5-b3ff-9951a72db95a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""349a35dc-2de0-4aed-9450-d711a6dfe39b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""173717d1-8170-440e-af9c-83a80d24bfbc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7ae29af8-367f-4ce2-89b5-342fad37d1c0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65e40c19-aa93-4b7d-b15d-875edada768d"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandCharacterChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10ce332c-009c-414c-b836-186e5c88416d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandCharacterChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c58dcd7-9bce-4374-98bd-1892c77b0e8b"",
                    ""path"": ""<Keyboard>/#(^$)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandCharacterChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""348f559a-865f-4148-87bd-9ee57e6d90b1"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandCharacterChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6442aff-e5d5-47af-81e4-81d82c365317"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandTurnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""818e2fb7-2ba6-4196-94fa-f2b7340a1ae2"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandTurnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""375b3e69-40cb-4a90-8582-ea208986a749"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandTurnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6acb37c4-311b-43af-954d-80721c036e44"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightHandTurnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""175d3384-fc8f-4067-a89c-235d4d86c64c"",
                    ""path"": ""<XInputController>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandTurnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ca58e3e-0f50-46a1-a7b0-18d4a7b710fc"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandTurnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72bc9856-3c10-4096-825c-47c88abaf69e"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandTurnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85e90ec6-125d-4cfe-be8d-d1862aa9dd99"",
                    ""path"": ""<XInputController>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftHandTurnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4daade85-ed33-4a4a-a79a-8a55bc8eb024"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ada7c13a-a2fd-4242-b116-562b5bfacd09"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e247677-75f7-4b77-aa4f-55fcf61fd93a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ba123ab-2d8c-4b4b-a0af-3ce3344447ac"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f78bbc11-4d78-4556-a526-411d69614252"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0eb69d3c-5b56-4fa2-ab36-6041e99f0586"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c9aff4b-c0af-49e5-be87-b6d1ea8a2326"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce81952d-8cc2-4614-9306-553425dbfe30"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d777fdb8-47e9-477e-8a49-2118f1ca3f5c"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66593417-89c0-4b44-b6d0-aa4de6ebbabb"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RigthHandShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b86202ce-e7ff-49a5-b5e9-f9af8c9409ba"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeaveShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TestGrid"",
            ""id"": ""7c03d42c-b43a-47bd-8814-bbdd97e34f23"",
            ""actions"": [
                {
                    ""name"": ""Action"",
                    ""type"": ""Value"",
                    ""id"": ""b8b899ce-ea91-48a2-89ff-545448bc8e86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""04e27089-700f-4606-87e9-0199e8663d55"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f230c952-bd09-4c7b-ac63-c96650d7db16"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25ad0f74-3a83-4d15-bdf9-2bd400bceb30"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ControlCamera
        m_ControlCamera = asset.FindActionMap("ControlCamera", throwIfNotFound: true);
        m_ControlCamera_RightHand = m_ControlCamera.FindAction("RightHand", throwIfNotFound: true);
        m_ControlCamera_LeftHand = m_ControlCamera.FindAction("LeftHand", throwIfNotFound: true);
        m_ControlCamera_RightHandCharacterChange = m_ControlCamera.FindAction("RightHandCharacterChange", throwIfNotFound: true);
        m_ControlCamera_LeftHandCharacterChange = m_ControlCamera.FindAction("LeftHandCharacterChange", throwIfNotFound: true);
        m_ControlCamera_RightHandTurnRight = m_ControlCamera.FindAction("RightHandTurnRight", throwIfNotFound: true);
        m_ControlCamera_RightHandTurnLeft = m_ControlCamera.FindAction("RightHandTurnLeft", throwIfNotFound: true);
        m_ControlCamera_LeftHandTurnRight = m_ControlCamera.FindAction("LeftHandTurnRight", throwIfNotFound: true);
        m_ControlCamera_LeftHandTurnLeft = m_ControlCamera.FindAction("LeftHandTurnLeft", throwIfNotFound: true);
        m_ControlCamera_RigthHandShoulder = m_ControlCamera.FindAction("RigthHandShoulder", throwIfNotFound: true);
        m_ControlCamera_LeaveShoulder = m_ControlCamera.FindAction("LeaveShoulder", throwIfNotFound: true);
        // TestGrid
        m_TestGrid = asset.FindActionMap("TestGrid", throwIfNotFound: true);
        m_TestGrid_Action = m_TestGrid.FindAction("Action", throwIfNotFound: true);
        m_TestGrid_MousePosition = m_TestGrid.FindAction("MousePosition", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // ControlCamera
    private readonly InputActionMap m_ControlCamera;
    private IControlCameraActions m_ControlCameraActionsCallbackInterface;
    private readonly InputAction m_ControlCamera_RightHand;
    private readonly InputAction m_ControlCamera_LeftHand;
    private readonly InputAction m_ControlCamera_RightHandCharacterChange;
    private readonly InputAction m_ControlCamera_LeftHandCharacterChange;
    private readonly InputAction m_ControlCamera_RightHandTurnRight;
    private readonly InputAction m_ControlCamera_RightHandTurnLeft;
    private readonly InputAction m_ControlCamera_LeftHandTurnRight;
    private readonly InputAction m_ControlCamera_LeftHandTurnLeft;
    private readonly InputAction m_ControlCamera_RigthHandShoulder;
    private readonly InputAction m_ControlCamera_LeaveShoulder;
    public struct ControlCameraActions
    {
        private @Controller m_Wrapper;
        public ControlCameraActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @RightHand => m_Wrapper.m_ControlCamera_RightHand;
        public InputAction @LeftHand => m_Wrapper.m_ControlCamera_LeftHand;
        public InputAction @RightHandCharacterChange => m_Wrapper.m_ControlCamera_RightHandCharacterChange;
        public InputAction @LeftHandCharacterChange => m_Wrapper.m_ControlCamera_LeftHandCharacterChange;
        public InputAction @RightHandTurnRight => m_Wrapper.m_ControlCamera_RightHandTurnRight;
        public InputAction @RightHandTurnLeft => m_Wrapper.m_ControlCamera_RightHandTurnLeft;
        public InputAction @LeftHandTurnRight => m_Wrapper.m_ControlCamera_LeftHandTurnRight;
        public InputAction @LeftHandTurnLeft => m_Wrapper.m_ControlCamera_LeftHandTurnLeft;
        public InputAction @RigthHandShoulder => m_Wrapper.m_ControlCamera_RigthHandShoulder;
        public InputAction @LeaveShoulder => m_Wrapper.m_ControlCamera_LeaveShoulder;
        public InputActionMap Get() { return m_Wrapper.m_ControlCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlCameraActions set) { return set.Get(); }
        public void SetCallbacks(IControlCameraActions instance)
        {
            if (m_Wrapper.m_ControlCameraActionsCallbackInterface != null)
            {
                @RightHand.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHand;
                @RightHand.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHand;
                @RightHand.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHand;
                @LeftHand.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHand;
                @LeftHand.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHand;
                @LeftHand.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHand;
                @RightHandCharacterChange.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandCharacterChange;
                @RightHandCharacterChange.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandCharacterChange;
                @RightHandCharacterChange.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandCharacterChange;
                @LeftHandCharacterChange.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandCharacterChange;
                @LeftHandCharacterChange.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandCharacterChange;
                @LeftHandCharacterChange.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandCharacterChange;
                @RightHandTurnRight.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnRight;
                @RightHandTurnRight.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnRight;
                @RightHandTurnRight.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnRight;
                @RightHandTurnLeft.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnLeft;
                @RightHandTurnLeft.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnLeft;
                @RightHandTurnLeft.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRightHandTurnLeft;
                @LeftHandTurnRight.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnRight;
                @LeftHandTurnRight.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnRight;
                @LeftHandTurnRight.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnRight;
                @LeftHandTurnLeft.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnLeft;
                @LeftHandTurnLeft.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnLeft;
                @LeftHandTurnLeft.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeftHandTurnLeft;
                @RigthHandShoulder.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRigthHandShoulder;
                @RigthHandShoulder.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRigthHandShoulder;
                @RigthHandShoulder.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnRigthHandShoulder;
                @LeaveShoulder.started -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeaveShoulder;
                @LeaveShoulder.performed -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeaveShoulder;
                @LeaveShoulder.canceled -= m_Wrapper.m_ControlCameraActionsCallbackInterface.OnLeaveShoulder;
            }
            m_Wrapper.m_ControlCameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RightHand.started += instance.OnRightHand;
                @RightHand.performed += instance.OnRightHand;
                @RightHand.canceled += instance.OnRightHand;
                @LeftHand.started += instance.OnLeftHand;
                @LeftHand.performed += instance.OnLeftHand;
                @LeftHand.canceled += instance.OnLeftHand;
                @RightHandCharacterChange.started += instance.OnRightHandCharacterChange;
                @RightHandCharacterChange.performed += instance.OnRightHandCharacterChange;
                @RightHandCharacterChange.canceled += instance.OnRightHandCharacterChange;
                @LeftHandCharacterChange.started += instance.OnLeftHandCharacterChange;
                @LeftHandCharacterChange.performed += instance.OnLeftHandCharacterChange;
                @LeftHandCharacterChange.canceled += instance.OnLeftHandCharacterChange;
                @RightHandTurnRight.started += instance.OnRightHandTurnRight;
                @RightHandTurnRight.performed += instance.OnRightHandTurnRight;
                @RightHandTurnRight.canceled += instance.OnRightHandTurnRight;
                @RightHandTurnLeft.started += instance.OnRightHandTurnLeft;
                @RightHandTurnLeft.performed += instance.OnRightHandTurnLeft;
                @RightHandTurnLeft.canceled += instance.OnRightHandTurnLeft;
                @LeftHandTurnRight.started += instance.OnLeftHandTurnRight;
                @LeftHandTurnRight.performed += instance.OnLeftHandTurnRight;
                @LeftHandTurnRight.canceled += instance.OnLeftHandTurnRight;
                @LeftHandTurnLeft.started += instance.OnLeftHandTurnLeft;
                @LeftHandTurnLeft.performed += instance.OnLeftHandTurnLeft;
                @LeftHandTurnLeft.canceled += instance.OnLeftHandTurnLeft;
                @RigthHandShoulder.started += instance.OnRigthHandShoulder;
                @RigthHandShoulder.performed += instance.OnRigthHandShoulder;
                @RigthHandShoulder.canceled += instance.OnRigthHandShoulder;
                @LeaveShoulder.started += instance.OnLeaveShoulder;
                @LeaveShoulder.performed += instance.OnLeaveShoulder;
                @LeaveShoulder.canceled += instance.OnLeaveShoulder;
            }
        }
    }
    public ControlCameraActions @ControlCamera => new ControlCameraActions(this);

    // TestGrid
    private readonly InputActionMap m_TestGrid;
    private ITestGridActions m_TestGridActionsCallbackInterface;
    private readonly InputAction m_TestGrid_Action;
    private readonly InputAction m_TestGrid_MousePosition;
    public struct TestGridActions
    {
        private @Controller m_Wrapper;
        public TestGridActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Action => m_Wrapper.m_TestGrid_Action;
        public InputAction @MousePosition => m_Wrapper.m_TestGrid_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_TestGrid; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestGridActions set) { return set.Get(); }
        public void SetCallbacks(ITestGridActions instance)
        {
            if (m_Wrapper.m_TestGridActionsCallbackInterface != null)
            {
                @Action.started -= m_Wrapper.m_TestGridActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_TestGridActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_TestGridActionsCallbackInterface.OnAction;
                @MousePosition.started -= m_Wrapper.m_TestGridActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_TestGridActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_TestGridActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_TestGridActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public TestGridActions @TestGrid => new TestGridActions(this);
    public interface IControlCameraActions
    {
        void OnRightHand(InputAction.CallbackContext context);
        void OnLeftHand(InputAction.CallbackContext context);
        void OnRightHandCharacterChange(InputAction.CallbackContext context);
        void OnLeftHandCharacterChange(InputAction.CallbackContext context);
        void OnRightHandTurnRight(InputAction.CallbackContext context);
        void OnRightHandTurnLeft(InputAction.CallbackContext context);
        void OnLeftHandTurnRight(InputAction.CallbackContext context);
        void OnLeftHandTurnLeft(InputAction.CallbackContext context);
        void OnRigthHandShoulder(InputAction.CallbackContext context);
        void OnLeaveShoulder(InputAction.CallbackContext context);
    }
    public interface ITestGridActions
    {
        void OnAction(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
