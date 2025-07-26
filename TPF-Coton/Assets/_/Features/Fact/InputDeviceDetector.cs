using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFundation.Runtime
{
    public class InputDeviceDetector : MonoBehaviour
    {
        #region Public
        
        
        
        public enum ControlScheme
        {
            KeyboardMouse,
            PlayStation,
            Xbox,
            Switch,
            GenericGamepad,
            Touch,
            Unknown
        }
        
        public static ControlScheme CurrentControlScheme { get; private set; }
        
        public static event Action<ControlScheme> OnControlSchemeChanged;
        
        #endregion
        
        
        #region Api Unity

        private void OnEnable()
        {
            InputSystem.onDeviceChange += onDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= onDeviceChange;
        }
        
        #endregion
        
        
        #region Utils
        private void onDeviceChange(InputDevice device, InputDeviceChange change)
        {
            // On s'intéresse uniquement à l'utilisation ou au branchement du device
            if (change != InputDeviceChange.Added && change != InputDeviceChange.Reconnected && change != InputDeviceChange.ConfigurationChanged)
                return;

            DetectControlScheme(device);
        }

        public void DetectControlScheme(InputDevice device)
        {
            if (device is Gamepad gamepad)
            {
                string product = gamepad.description.product?.ToLower() ?? "";

                if (product.Contains("dualshock") || product.Contains("dualsense") || product.Contains("playstation"))
                    SetControlScheme(ControlScheme.PlayStation);
                else if (product.Contains("xbox"))
                    SetControlScheme(ControlScheme.Xbox);
                else if (product.Contains("switch") || product.Contains("joycon") || product.Contains("pro controller"))
                    SetControlScheme(ControlScheme.Switch);
                else
                    SetControlScheme(ControlScheme.GenericGamepad);
            }
            else if (device is Keyboard || device is Mouse)
            {
                SetControlScheme(ControlScheme.KeyboardMouse);
            }
            else if (device is Touchscreen)
            {
                SetControlScheme(ControlScheme.Touch);
            }
            else
            {
                SetControlScheme(ControlScheme.Unknown);
            }
        }

        private void SetControlScheme(ControlScheme scheme)
        {
            if (CurrentControlScheme != scheme)
            {
                CurrentControlScheme = scheme;
                GameManager.m_gameFacts.SetFact(GameFactKeys.InputType, scheme.ToString(), FactDictionary.FactPersistence.Normal);
                Debug.Log($"Input scheme changed to {scheme}");
            }
        }

        #endregion
    }
}
