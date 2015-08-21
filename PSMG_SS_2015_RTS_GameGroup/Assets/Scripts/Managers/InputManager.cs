﻿using System.Collections.Generic;
using Assets.Scripts.Controllers.Objects;
using Assets.Scripts.ParameterObjects;
using Assets.Scripts.Types;
using Assets.Scripts.UserInterfaceComponents;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// The input managers defines the semantic of in-game input events and
    /// forwards these events to its listeners.
    /// It registers input from the keyboard as well as from the user interface.
    /// </summary>

    public class InputManager : MonoBehaviour, UIManager.IUIManagerListener, LevelManager.ILevelManagerListener {

        private List<IInputManagerListener> listeners;
        private bool isPaused;
        // ReSharper disable once NotAccessedField.Local
        private UserInterface userInterface;
        private GameObject mainCamera;

        public static InputManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            isPaused = false;
            listeners = new List<IInputManagerListener>();
        }

        public interface IInputManagerListener
        {
            void OnProfessionSelected(ImpType profession);
            void OnSelectNextImp();
            void OnSelectNextUnemployedImp();
        }

        public void OnGUI()
        {
            var e = Event.current;
            CheckScrollInput(e);
            CheckKeyInput(e);
        }

        private void CheckKeyInput(Event e)
        {
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.LeftAlt:
                        HighlightObjects(true);
                        break;
                    case KeyCode.Tab:
                        SelectNextImp();
                        break;
                    case KeyCode.Alpha1:
                        SelectProfession(ImpType.Spearman);
                        break;
                    case KeyCode.Alpha2:
                        SelectProfession(ImpType.Coward);
                        break;
                    case KeyCode.Alpha3:
                        SelectProfession(ImpType.LadderCarrier);
                        break;
                    case KeyCode.Alpha4:
                        SelectProfession(ImpType.Blaster);
                        break;
                    case KeyCode.Alpha5:
                        SelectProfession(ImpType.Firebug);
                        break;
                    case KeyCode.Alpha6:
                        SelectProfession(ImpType.Schwarzenegger);
                        break;
                    case KeyCode.Alpha7:
                        SelectProfession(ImpType.Unemployed);
                        break;
                    case KeyCode.Space:
                        PauseGame();
                        break;
                    case KeyCode.A:
                        MoveCameraLeft();
                        break;
                    case KeyCode.LeftArrow:
                        MoveCameraLeft();
                        break;
                    case KeyCode.D:
                        MoveCameraRight();
                        break;
                    case KeyCode.RightArrow:
                        MoveCameraRight();
                        break;
                    case KeyCode.N:
                        SelectNextUnemployedImp();
                        break;
                    case KeyCode.F:
                        IncreaseGameSpeed();
                        break;
                    case KeyCode.S:
                        DecreaseGameSpeed();
                        break;
                }
            }
            if (e.type != EventType.KeyUp) return;
            switch (e.keyCode)
            {
                case KeyCode.LeftAlt:
                    HighlightObjects(false);
                    break;
            }
        }

        private void HighlightObjects(bool highlighted)
        {
            var highlightableObjects = LevelManager.Instance.CurrentLevel.HighlightableObjects;
            highlightableObjects.ForEach(ho => ho.GetComponent<HighlightableObject>().IsBlinking = highlighted);
        }

        public void DecreaseGameSpeed()
        {
            if (Time.timeScale >= 0.1f && Time.timeScale <= 2.0f)
            {
                Time.timeScale -= 0.1f;
            }
        }

        public void IncreaseGameSpeed()
        {
            if (Time.timeScale >= 0.0f && Time.timeScale <= 1.9f)
            {
                Time.timeScale += 0.1f;
            }
        }

        private void SelectNextUnemployedImp()
        {
            listeners.ForEach(x => x.OnSelectNextUnemployedImp());
        }

        private void CheckScrollInput(Event e)
        {
            if (e.type != EventType.ScrollWheel) return;
            if (e.delta.y > 0)
            {
                MoveCameraRight();
            }
            else
            {
                MoveCameraLeft();
            }
        }

        public void MoveCameraRight()
        {
            var pos = mainCamera.transform.position;
            pos.x++;
            if (!(pos.x >= GetComponent<LevelManager>().CurrentLevel.RightMargin.transform.position.x))
            {
                mainCamera.transform.position = pos;
            }
        }

        public void MoveCameraRight(float distance)
        {
            var pos = mainCamera.transform.position;
            pos.x += distance;
            if (!(pos.x >= GetComponent<LevelManager>().CurrentLevel.RightMargin.transform.position.x))
            {
                mainCamera.transform.position = pos;
            }
        }

        public void MoveCameraLeft()
        {
            var pos = mainCamera.transform.position;
            pos.x--;
            if (!(pos.x <= GetComponent<LevelManager>().CurrentLevel.LeftMargin.transform.position.x))
            {
                mainCamera.transform.position = pos;
            }
        }

        public void MoveCameraLeft(float distance)
        {
            var pos = mainCamera.transform.position;
            pos.x -= distance;
            if (!(pos.x <= GetComponent<LevelManager>().CurrentLevel.LeftMargin.transform.position.x))
            {
                mainCamera.transform.position = pos;
            }
        }


        public void PauseGame()
        {
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
            } else {
                Time.timeScale = 0f;
                isPaused = true;
            }
        }

        private void SelectNextImp()
        {
            listeners.ForEach(x => x.OnSelectNextImp());
        }

        private void SelectProfession(ImpType impType)
        {
            listeners.ForEach(x => x.OnProfessionSelected(impType));
        }

        private void SelectProfession(int impTypeNumber)
        {
            listeners.ForEach(x => x.OnProfessionSelected((ImpType) impTypeNumber));
        }

        public void RegisterListener(IInputManagerListener listener)
        {
            listeners.Add(listener);
        }

        private void RegisterListenersForImpTrainingButtons(UserInterface userInteface)
        {
            var buttons = userInteface.ImpTrainingButtons;
            for (var i = 0; i < buttons.Length; i++)
            {
                var nr = i;
                buttons[nr].Button.onClick.AddListener(() => SelectProfession(nr));
            }
        }

        void UIManager.IUIManagerListener.OnUserInterfaceLoaded(UserInterface userInteface)
        {
            this.userInterface = userInteface;
            RegisterListenersForImpTrainingButtons(userInteface);
        }


        void LevelManager.ILevelManagerListener.OnLevelStarted(Level level)
        {
            mainCamera = level.MainCamera;
        }
    }
}
