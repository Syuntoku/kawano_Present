using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Syuntoku.DigMode.Input
{
    [System.Serializable]
    static public class InputData
    {

        [Header("プレイヤー用Input")]
        static public Vector2 _InputMoveVec2;
        static public bool _bJump;
        static public bool _bdash;
        static public bool _bAction;
        static public Vector2 _middleHweel;
        static public bool _bMenu;
        static public bool _bPlusFall;
        static public bool _bTeleport;
        static public bool _bGetPark;
        static public bool _bModeChange;

        [Header("マウス用Input")]
        static public Vector2 _InputMouseDelta;
    }


    public class InputManager : MonoBehaviour
    {

        void Update()
        {
            NomalMouseInput();
        }


        public void OnMove(InputAction.CallbackContext obj)
        {
            InputData._InputMoveVec2 = obj.ReadValue<Vector2>();

        }

        public void OnJump(InputAction.CallbackContext obj)
        {

            if (obj.started)
            {
                InputData._bJump = true;
            }

            if (obj.canceled)
            {
                InputData._bJump = false;

            }
        }

        public void OnMouseDelta(InputAction.CallbackContext obj)
        {
            //InputData.m_InputMouseDelta = obj.ReadValue<Vector2>();

        }

        void NomalMouseInput()
        {
            var mouse = Mouse.current;

            if (mouse != null)
            {
                InputData._InputMouseDelta = mouse.delta.ReadValue();
            }
        }

        public void OnDash(InputAction.CallbackContext obj)
        {
            InputData._bdash = true;

            if (obj.canceled)
            {
                InputData._bdash = false;
                return;
            }
        }

        public void Action(InputAction.CallbackContext obj)
        {
            InputData._bAction = true;

            if (obj.canceled)
            {
                InputData._bAction = false;
                return;
            }
        }

        public void Hweel(InputAction.CallbackContext context)
        {
            InputData._middleHweel = context.ReadValue<Vector2>();
        }

        public void Menu(InputAction.CallbackContext context)
        {

            InputData._bMenu = true;

            if (context.canceled)
            {
                InputData._bMenu = false;
                return;
            }
        }

        public void PulusFall(InputAction.CallbackContext context)
        {

            InputData._bPlusFall = true;

            if (context.canceled)
            {
                InputData._bPlusFall = false;
                return;
            }
        }

        public void Teleport(InputAction.CallbackContext context)
        {

            InputData._bTeleport = true;

            if (context.canceled)
            {
                InputData._bTeleport = false;
                return;
            }
        }
        public void OnGetPark(InputAction.CallbackContext context)
        {

            InputData._bGetPark = true;

            if (context.canceled)
            {
                InputData._bGetPark = false;
                return;
            }
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            InputData._bModeChange = true;

            if (context.canceled)
            {
                InputData._bModeChange = false;
                return;
            }
        }
    }
}